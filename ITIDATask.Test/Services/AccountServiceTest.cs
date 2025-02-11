using AutoMapper;
using Azure;
using ITIDATask.DAL.Entities;
using ITIDATask.Repositories.Interfaces;
using ITIDATask.Services;
using ITIDATask.Utitlites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITIDATask.Test.Services
{
    public class AccountServiceTest
    {

        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<IOptionsMonitor<AppSettings>> _appSettingsMock;
        private readonly AccountService _accountService;

        public AccountServiceTest()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null); _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object, Mock.Of<IHttpContextAccessor>(),Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
            _appSettingsMock = new Mock<IOptionsMonitor<AppSettings>>();
            _appSettingsMock.Setup(ap => ap.CurrentValue).Returns(new AppSettings { Secret = "testKey" });
            _accountService = new AccountService(_userManagerMock.Object, _signInManagerMock.Object, _appSettingsMock.Object);

        }


        [Fact]
        public async Task RegisterAsync_UserAlreadyExists()
        { 
            var registerModel = new RegisterModel { Email = "test@test.com", Password = "P@ssw0rd!", MobileNumber = "01234567890", Name = "Test" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(registerModel.Email)).ReturnsAsync(new ApplicationUser());
            var result = await _accountService.RegisterAsync(registerModel);

            Assert.False(result.Success);
            Assert.Equal("user already exisit",result.Message);
        }

        [Fact]
        public async Task RegisterAsync_UserCreatedSuccessfully()
        {
            var registerModel = new RegisterModel { Email = "test@test.com", Password = "P@ssw0rd!", MobileNumber = "01234567890", Name = "Test" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(registerModel.Email)).ReturnsAsync((ApplicationUser)null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registerModel.Password)).ReturnsAsync(IdentityResult.Success);
            var result = await _accountService.RegisterAsync(registerModel);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task ValidateUserAsync_InvalidUser()
        {
            var loginModel = new LoginModel { Email = "invalid@example.com", Password = "WrongPassword" };
            _userManagerMock.Setup(um => um.Users).Returns((new List<ApplicationUser>()).AsQueryable().BuildMock());
            var result = await _accountService.ValidateUserAsync(loginModel);

            Assert.False(result.Success);
            Assert.Equal("Invalid User!.", result.Message);
        }

        [Fact]
        public async Task ValidateUserAsync_ValidUserInvalidPassword()
        {
            var loginModel = new LoginModel { Email = "test@test.com", Password = "WrongPassword" };
            var user = new ApplicationUser { Email = "test@test.com", UserName = "testuser" };
            _userManagerMock.Setup(um => um.Users).Returns((new List<ApplicationUser> { user }).AsQueryable().BuildMock());
            _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, loginModel.Password, true)).ReturnsAsync(SignInResult.Failed);

            var result = await _accountService.ValidateUserAsync(loginModel);

            Assert.False(result.Success);
            Assert.Equal("Invalid Password", result.Message);
        }

        [Fact]
        public async Task ValidateUserAsync_ValidUserValidPassword_ReturnsSucceeded()
        {
            var loginModel = new LoginModel { Email = "test@example.com", Password = "CorrectPassword" };
            var user = new ApplicationUser { Id = "1", Email = "test@example.com", UserName = "testuser", PhoneNumber = "1234567890", Name = "Test User" };
            _userManagerMock.Setup(um => um.Users).Returns((new List<ApplicationUser> { user }).AsQueryable().BuildMock());
            _signInManagerMock.Setup(sm => sm.CheckPasswordSignInAsync(user, loginModel.Password, true)).ReturnsAsync(SignInResult.Success);

            var result = await _accountService.ValidateUserAsync(loginModel);

            Assert.True(result.Success);
            Assert.NotNull(result.Payload);
        }
    }
}
