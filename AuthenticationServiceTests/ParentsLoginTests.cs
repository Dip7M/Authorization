using AuthenticationService.Controllers;
using AuthenticationService.Models;
using AuthenticationService.Provider;
using AuthenticationService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using AuthenticationService;
using System;

namespace AuthenticationServiceTests
{
    public class ParentsLoginTests
    {
        private AuthController _AuthController;
        private Mock<IAuthProvider> _AuthProviderMock;
        private string _Token = "token";

        private Mock<IConfiguration> _Config;
        private Mock<IAuthRepo> _AuthRepository;
        private IAuthProvider _AuthProvider;
        private Mock<ITokenProvider> _TokenProvider;
        public static List<Staff> Staff;
        private LoggerManager _logger = new LoggerManager();

       

        [SetUp]
        public void Setup()
        {
            _logger.Info("User Login SETUP Started!");
            _AuthProviderMock = new Mock<IAuthProvider>();
            _AuthController = new AuthController(_AuthProviderMock.Object);
            _Config = new Mock<IConfiguration>();
            _Config.Setup(s => s["Jwt:Key"]).Returns("AuthenticationServiceSecretKey");
            _AuthRepository = new Mock<IAuthRepo>();
            _TokenProvider = new Mock<ITokenProvider>();

            _TokenProvider.Setup(s => s.GenerateJwtTokenUser(It.IsAny<Parents>())).Returns(_Token);


            _AuthProvider = new AuthProvider(_AuthRepository.Object, _TokenProvider.Object,_Config.Object);
            
        }

        [TestCase(1, "Alok@gmail.com", "Alok@123")]
        [TestCase(2, "Dip@gmail.com", "Dip@123")]
        public void AuthRepo_ParentsLogin_PassTest(int RegId ,string Email, string Password)
        {
            try
            {
                _logger.Info("First Positive Test - AuthRepo_ParentsLogin_PassTest");
                IAuthRepo authRepo = new AuthRepository();
                Parents user = new Parents()
                {
                    RegId = RegId,
                    Email = Email,
                    Pwd = Password

                };
                var actualValue = authRepo.ParentsLogin(user);
                Assert.IsTrue(actualValue);
            }
            catch(Exception ex)
            {
                _logger.Error("Exception Occured while Testing AuthRepo_ParentsLogin_PassTest() " + ex.Message);
            }
           
        }

        [TestCase(1, "Alok@gmail.com", "Wrong Password")]
        [TestCase(2, "Dip@gmail.com", "Wrong Password")]
        public void AuthRepo_UserLogin_FailTest(int RegId, string Email,string Password)
        {
            try
            {
                _logger.Info("Second Test - AuthRepo_ParentsLogin_FailTest ");
                IAuthRepo authRepo = new AuthRepository();
                Parents user = new Parents()
                {
                    RegId = RegId,
                    Email = "srinu",
                    Pwd = "nanisdfsd"

                };
                var actualValue = authRepo.ParentsLogin(user);
                Assert.IsFalse(actualValue);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured while Testing AuthRepo_ParentsLogin_FailTest() " + ex.Message);
            }


        }

        [TestCase(11,"vibhu", "Vibhu@123#")]
        [TestCase(12,"rishi", "Rishi@123#")]
        public void AuthProvider_ValidData_LoginProviderMethod_PassTest_ReturnsJwtToken(int RegId,string Email, string Password)
        {
            try
            {
                _logger.Info("Third Test - AuthProvider_ValidData_LoginProviderMethod_PassTest_ReturnsJwtToken");
                _AuthRepository.Setup(s => s.ParentsLogin(It.IsAny<Parents>())).Returns(false);
                Parents user = new Parents()
                {
                    RegId = RegId,
                    Email = Email,
                    Pwd = Password

                };
                var token = _AuthProvider.LoginProviderParents(user);
                Assert.IsNotNull(token);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured while Testing AuthProvider_ValidData_LoginProviderMethod_PassTest_ReturnsJwtToken() " + ex.Message);
            }

        }

        

        [TestCase(11,"vibhu", "Wrong Password")]
        [TestCase(12,"rishi", "Wrong Password")]
        public void AuthProvider_InValidData_LoginProviderMethod_FailTest_ReturnsExceptionOccured(int RegId, string Email, string Password)
        {

            try
            {
                _logger.Info("Fourth Test - AuthProvider_InValidData_LoginProviderMethod_FailTest_ReturnsExceptionOccured");
                _AuthRepository.Setup(s => s.ParentsLogin(It.IsAny<Parents>())).Returns(false);

                //_AuthProviderMock.Setup(s => s.LoginProviderUser(It.IsAny<User>())).Returns("Invalid Credentials");
                Parents user = new Parents()
                {
                    RegId = RegId,
                    Email = Email,
                    Pwd = Password

                };
                var token = _AuthProvider.LoginProviderParents(user);
                Assert.AreEqual(token, "Exception Occured");
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured while Testing AuthProvider_InValidData_LoginProviderMethod_FailTest_ReturnsExceptionOccured() " + ex.Message);
            }
           
        }


        [TestCase(11,"vibhu", "Vibhu@123#")]
        [TestCase(12,"rishi", "Rishi@123#")]
        public void AuthController_UserLoginMethod_ValidData_PassTest(int RegId, string Email, string Password)
        {


            try
            {
                _logger.Info("Fifth Test -  AuthController_UserLoginMethod_ValidData_PassTest");
                Parents user = new Parents()
                {
                    RegId = RegId,
                    Email = Email,
                    Pwd = Password

                };
                var Response = _AuthController.ParentsLogin(user) as ObjectResult;
                Assert.AreEqual(200, Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured while Testing  AuthController_ParentsLoginMethod_ValidData_PassTest() " + ex.Message);
            }

           
        }

        [TestCase(11,"vibhu", "Wrong Password")]
        [TestCase(12,"rishi", "Wrong Password")]
        public void AuthController_UserLoginMethoSd_InValidData_FailTest_InvalidCredetialResult(int RegId, string Email, string Password)
        {

            try
            {
                _logger.Info("Sixth Test -  AuthController_UserLoginMethoSd_InValidData_FailTest_InvalidCredetialResult");
                _AuthRepository.Setup(s => s.ParentsLogin(It.IsAny<Parents>())).Returns(false);

                _AuthProviderMock.Setup(s => s.LoginProviderParents(It.IsAny<Parents>())).Returns("Invalid Credentials");

                _AuthController = new AuthController(_AuthProviderMock.Object);
                Parents user = new Parents()
                {
                    RegId = RegId,
                    Email = Email,
                    Pwd = Password

                };
                var Response = _AuthController.ParentsLogin(user) as ObjectResult;
                Assert.AreEqual(404, Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured while Testing  AuthController_ParentsLoginMethoSd_InValidData_FailTest_InvalidCredetialResult() " + ex.Message);
            }
           
        }

        [TestCase(11,"vibhujain", "Wrong Password")]
        [TestCase(12,"rishianand", "Wrong Password")]
        public void AuthController_UserLoginMethod_InValidData_FailTest_ExceptionResult(int RegId, string Email, string Password)
        {
            try
            {
                _logger.Info("Seventh Test -  AuthController_ParentsLoginMethod_InValidData_FailTest_ExceptionResult");
                _AuthRepository.Setup(s => s.ParentsLogin(It.IsAny<Parents>())).Returns(false);

                _AuthProviderMock.Setup(s => s.LoginProviderParents(It.IsAny<Parents>())).Returns("Exception Occured");

                _AuthController = new AuthController(_AuthProviderMock.Object);
                Parents user = new Parents()
                {
                    RegId = RegId,
                    Email = Email,
                    Pwd = Password

                };
                var Response = _AuthController.ParentsLogin(user) as StatusCodeResult;
                Assert.AreEqual(500, Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured while Testing  AuthController_UserLoginMethod_InValidData_FailTest_ExceptionResult() " + ex.Message);
            }
           
        }

        
    }
}