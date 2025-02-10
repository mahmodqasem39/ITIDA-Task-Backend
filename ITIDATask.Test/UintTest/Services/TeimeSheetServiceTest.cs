using AutoMapper;
using ITIDATask.DAL.Entities;
using ITIDATask.Repositories.Interfaces;
using ITIDATask.Services;
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
    private readonly TimesheetService _timesheetService;

    public TimesheetServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _timesheetRepoMock = new Mock<IGenericRepository<Timesheet>>();
        _timesheetService = new TimesheetService(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task SubmitRegisterdDateInFuture_ReturnsFailedResult()
    {
        var date = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var submitModel = new Timesheet
        {
            RegisterDate = date,
            LoginTime = TimeOnly.FromDateTime(DateTime.Now),
            LogoutTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(8)),
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
            LoginTime = TimeOnly.FromDateTime(DateTime.Now),
            LogoutTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(8)),
            UserId = "user"
        };

        var result = await _timesheetService.SubmitRegisterdTime(submitModel);

        Assert.False(result.Success);
        Assert.Equal("Logout time must be after login time.", result.Message);
    }

    [Fact]
    public async Task SubmitRegisterdTime_TimesheetAlreadySubmitted_ReturnsExistedResult()
    {
        var submitModel = new Timesheet
        {
            RegisterDate = DateOnly.FromDateTime(DateTime.Now),
            LoginTime = TimeOnly.FromDateTime(DateTime.Now),
            LogoutTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(8)),
            UserId = "user"
        };

        _timesheetRepoMock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Timesheet, bool>>>()))
            .ReturnsAsync(new List<Timesheet>());

        _timesheetRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Timesheet>()))
                    .Returns(Task.CompletedTask);


        var result = await _timesheetService.SubmitRegisterdTime(submitModel);

        Assert.False(result.Success);
        Assert.Equal("This date Is submited befor for this user", result.Message);
    }

    [Fact]
    public async Task SubmitRegisterdTime_ShouldAddNewEntry_WhenNotPreviouslySubmitted()
    {
        var timesheet = new Timesheet
        {
            RegisterDate = DateOnly.FromDateTime(DateTime.Now),
            LoginTime = TimeOnly.FromDateTime(DateTime.Now),
            LogoutTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(8)),
            UserId = "user"
        };

        _timesheetRepoMock
            .Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Timesheet, bool>>>()))
            .ReturnsAsync(new List<Timesheet>());

        _timesheetRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Timesheet>()))
                            .Returns(Task.CompletedTask);

       // _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).Returns(Task);

        // Act
        var result = await _timesheetService.SubmitRegisterdTime(timesheet);

        // Assert
        Assert.True(result.Success);
        _timesheetRepoMock.Verify(repo => repo.AddAsync(It.IsAny<Timesheet>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}