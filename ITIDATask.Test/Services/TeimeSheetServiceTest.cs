using AutoMapper;
using ITIDATask.DAL.Entities;
using ITIDATask.Repositories.Interfaces;
using ITIDATask.Services;
using ITIDATask.Utitlites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class TimesheetServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IGenericRepository<Timesheet>> _timesheetRepoMock;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly TimesheetService _timesheetService;

    public TimesheetServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _timesheetRepoMock = new Mock<IGenericRepository<Timesheet>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        _timesheetService = new TimesheetService(_unitOfWorkMock.Object, _mapperMock.Object,_mockUserManager.Object);
    }


    #region submit registerd time
    [Fact]
    public async Task SubmitRegisterdDateInFuture_ReturnsFailedResult()
    {
        var submitModel = new Timesheet
        {
            RegisterDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            LoginTime = TimeOnly.Parse("08:00"),
            LogoutTime = TimeOnly.Parse("17:00"),
            UserId = "user"
        };

        var result = await _timesheetService.SubmitRegisterdTime(submitModel);

        Assert.False(result.Success);
        Assert.Equal("Date cannot be in the future", result.Message);
    }

    [Fact]
    public async Task SubmitRegisterdTime_LogoutTimeBeforeLoginTime_ReturnsFailedResult()
    {
        var submitModel = new Timesheet
        {
            RegisterDate = DateOnly.FromDateTime(DateTime.Now),
            LoginTime = TimeOnly.Parse("17:00"),
            LogoutTime = TimeOnly.Parse("8:00"),
            UserId = "user"
        };

        var result = await _timesheetService.SubmitRegisterdTime(submitModel);

        Assert.False(result.Success);
        Assert.Equal("Logout time must be after login time.", result.Message);
    }

    [Fact]
    public async Task SubmitRegisterdTime_ShouldReturnError_WhenSubmittedBefore()
    {

        var timesheet = new Timesheet
        {
            RegisterDate = DateOnly.FromDateTime(DateTime.Now),
            LoginTime = TimeOnly.Parse("08:00"),
            LogoutTime = TimeOnly.Parse("17:00"),
            UserId = "UserId",
        };

        var existingTimesheet = new List<Timesheet> { timesheet };

        _timesheetRepoMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Timesheet, bool>>>()))
                                    .ReturnsAsync(existingTimesheet);

        _unitOfWorkMock.Setup(uow => uow.GetRepository<Timesheet>()).Returns(_timesheetRepoMock.Object);

        var result = await _timesheetService.SubmitRegisterdTime(timesheet);


        Assert.False(result.Success);
        Assert.Equal("This date Is submited befor for this user", result.Message);

        _timesheetRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Timesheet>()), Times.Never);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SubmitRegisterdTime_ShouldAddNewEntry_WhenNotPreviouslySubmitted()
    {
        var timesheet = new Timesheet
        {
            RegisterDate = DateOnly.FromDateTime(DateTime.Now),
            LoginTime = TimeOnly.Parse("08:00"),
            LogoutTime = TimeOnly.Parse("17:00"),
            UserId = "UserId",
        };

        _timesheetRepoMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Timesheet, bool>>>()))
                                   .ReturnsAsync(new List<Timesheet>());

        _unitOfWorkMock.Setup(uow => uow.GetRepository<Timesheet>()).Returns(_timesheetRepoMock.Object);

        _timesheetRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Timesheet>())).Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _timesheetService.SubmitRegisterdTime(timesheet);

        Assert.True(result.Success);
        _timesheetRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Timesheet>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
    #endregion

    [Fact]
    public async Task GetAllRegisteredTime_UserDoesNotExist_ReturnsNotFound()
    {
        var userId = "test-user";

        var user = new ApplicationUser { Id = userId };

        //_mockUserManager.Setup(x => x.Users).Returns(new List<ApplicationUser>().AsQueryable());
        _mockUserManager.Setup(x => x.Users).Returns(new List<ApplicationUser> { user }.AsQueryable());

        var result = await _timesheetService.GetAllRegiterdTime(userId);

        Assert.False(result.Success);
        Assert.Equal("user not exisit", result.Message);
    }

    [Fact]
    public async Task GetAllRegisteredTime_UserExists_ReturnsTimesheets()
    {
        var userId = "userId";
        var user = new ApplicationUser { Id = userId };
        var timesheets = new List<Timesheet> { new Timesheet { UserId = userId } };
        var timesheetsDto = new List<TimeSheetModel> { new TimeSheetModel() };

        _timesheetRepoMock.Setup(r => r.FindAsync(x => x.UserId == userId)).ReturnsAsync(timesheets);
        _unitOfWorkMock.Setup(u => u.GetRepository<Timesheet>()).Returns(_timesheetRepoMock.Object);
        _mapperMock.Setup(m => m.Map<IEnumerable<TimeSheetModel>>(timesheets)).Returns(timesheetsDto);

        var result = await _timesheetService.GetAllRegiterdTime(userId);
        Assert.True(result.Success);
    }


    [Fact]
    public async Task UpdateSubmitedTime_ValidModel_UpdatesTimesheet()
    {
        var model = new UpdateSubmitedTimetModel();
        var timesheet = new Timesheet();
        var repoMock = new Mock<IGenericRepository<Timesheet>>();

        _mapperMock.Setup(m => m.Map<Timesheet>(model)).Returns(timesheet);
        repoMock.Setup(r => r.UpdateAsync(timesheet)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.GetRepository<Timesheet>()).Returns(repoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _timesheetService.UpdateSubmitedTime(model);
        Assert.True(result.Success);

    }

    [Fact]
    public async Task DeleteSubmitedTime_InvalidId_ReturnsNotFound()
    {
        var timesheetId = 1;
        var repoMock = new Mock<IGenericRepository<Timesheet>>();

        repoMock.Setup(r => r.GetByIdAsync(timesheetId)).ReturnsAsync((Timesheet)null);
        _unitOfWorkMock.Setup(u => u.GetRepository<Timesheet>()).Returns(repoMock.Object);
        var result = await _timesheetService.DeleteSubmitedTime(timesheetId);

        Assert.False(result.Success);
        Assert.Equal("Item Not Found!.", result.Message);
    }

    [Fact]
    public async Task DeleteSubmitedTime_DeleteTimesheet()
    {
        var timesheetId = 1;
        var timesheet = new Timesheet { Id = timesheetId };
        var repoMock = new Mock<IGenericRepository<Timesheet>>();

        repoMock.Setup(r => r.GetByIdAsync(timesheetId)).ReturnsAsync(timesheet);
        repoMock.Setup(r => r.DeleteAsync(timesheetId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.GetRepository<Timesheet>()).Returns(repoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        var result = await _timesheetService.DeleteSubmitedTime(timesheetId);
        Assert.True(result.Success);
    }


}