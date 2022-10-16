using AuthenticationService.Controllers;
using AuthenticationService.Models;
using AuthenticationService.Provider;
using AuthenticationService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using AuthenticationService;

namespace AuthenticationServiceTests
{
    class StaffLoginTests
    {
        private AuthController _AuthController;
        private Mock<IAuthProvider> _AuthProviderMock;
        private string _Token = "token";

        //private string _TokenNull = null;

        private Mock<IConfiguration> _Config;
        private Mock<IAuthRepo> _AuthRepository;
        private IAuthProvider _AuthProvider;
        private Mock<ITokenProvider> _TokenProvider;
        // public static List<Employee> Employees;
        private LoggerManager _logger = new LoggerManager();
        //Setup Test 

        [SetUp]
        public void Setup()
        {
            _logger.Info("Setup Started StaffLoginTests");
           
            _Config = new Mock<IConfiguration>();
            _Config.Setup(s => s["Jwt:Key"]).Returns("AuthenticationServiceSecretKey");
            _AuthRepository = new Mock<IAuthRepo>();
            _TokenProvider = new Mock<ITokenProvider>();
            _AuthProviderMock = new Mock<IAuthProvider>();
            _AuthController = new AuthController(_AuthProviderMock.Object);
            _TokenProvider.Setup(s => s.GenerateJwtTokenEmployee(It.IsAny<Staff>())).Returns(_Token);


            _AuthProvider = new AuthProvider(_AuthRepository.Object, _TokenProvider.Object,_Config.Object);

        }

        //TestCases

        [TestCase(11, "Karthik", "Karthik@123")]
        [TestCase(12, "Gurupreet", "Gurupreet@123")]
        public void AuthRepo_StaffLogin_PassTest(long StaffId, string Username, string Password)
        {
            try
            {
                _logger.Info("First TestCase -  AuthRepo_StaffLogin_PassTest");
                IAuthRepo authRepo = new AuthRepository();
                Staff staff = new Staff()
                {
                    StaffId = StaffId,
                    Username = Username,
                    Password = Password

                };
                var actualValue = authRepo.StaffLogin(staff);
                Assert.IsTrue(actualValue);
            }catch(Exception ex)
            {
                _logger.Error("Exception Occured in  AuthRepo_StaffLogin_PassTest() - " + ex.Message);
            }
           
        }

        [TestCase(11, "garima", "Wrong Password")]
        [TestCase(12, "naveen", "Wrong Password")]
        public void AuthRepo_StaffLogin_FailTest(long StaffId, string Username, string Password)
        {

            try
            {
                _logger.Info("Second TestCase - AuthRepo_StaffLogin_FailTest");
                IAuthRepo authRepo = new AuthRepository();
                Staff staff = new Staff()
                {
                    StaffId = StaffId,
                    Username = Username,
                    Password = Password

                };
                var actualValue = authRepo.StaffLogin(staff);
                Assert.IsFalse(actualValue);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured in  AuthRepo_StaffLogin_FailTest() - " + ex.Message);
            }
            

        }

        [TestCase("garima", "Garima@123#")]
        [TestCase("naveen", "Naveen@123#")]
        public void AuthProvider_ValidData_LoginProviderMethod_PassTest_ReturnsJwtToken(string Username, string Password)
        {
            try
            {
                _logger.Info("Third TestCase - AuthProvider_ValidData_LoginProviderMethod_PassTest_ReturnsJwtToken");
                _AuthRepository.Setup(s => s.StaffLogin(It.IsAny<Staff>())).Returns(true);
                Staff staff = new Staff()
                {

                    Username = Username,
                    Password = Password

                };
                var token = _AuthProvider.LoginProviderStaff(staff);
                Assert.IsNotNull(token);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured in  AuthProvider_ValidData_LoginProviderMethod_PassTest_ReturnsJwtToken() - " + ex.Message);
            }
         
        }



        [TestCase("garima", "Wrong Password")]
        [TestCase("naveen", "Wrong Password")]
        public void AuthProvider_InValidData_LoginProviderMethod_FailTest_ReturnsExceptionOccured(string Username, string Password)
        {
            try
            {
                _logger.Info("Fourth TestCase - AuthProvider_InValidData_LoginProviderMethod_FailTest_ReturnsExceptionOccured");
                _AuthRepository.Setup(s => s.StaffLogin(It.IsAny<Staff>())).Returns(false);

                //_AuthProviderMock.Setup(s => s.LoginProviderEmployee(It.IsAny<Employee>())).Returns("Exception Occured");
                Staff staff = new Staff()
                {

                    Username = Username,
                    Password = Password

                };
                var token = _AuthProvider.LoginProviderStaff(staff);
                Assert.AreEqual(token, "Exception Occured");
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured in  AuthProvider_InValidData_LoginProviderMethod_FailTest_ReturnsExceptionOccured() - " + ex.Message);
            }
           
        }


        [TestCase("garima", "Garima@123#")]
        [TestCase("naveen", "Naveen@123#")]
        public void AuthController_StaffLoginMethod_ValidData_PassTest(string Username, string Password)
        {
            try
            {
                _logger.Info("Fifth TestCase -  AuthController_StaffLoginMethod_ValidData_PassTest");
                Staff staff = new Staff()
                {

                    Username = Username,
                    Password = Password

                };
                var Response = _AuthController.StaffLogin(staff) as ObjectResult;
                Assert.AreEqual(200, Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured in  AuthController_StaffLoginMethod_ValidData_PassTest() - " + ex.Message);
            }
           
        }

        [TestCase("garima", "Wrong Password")]
        [TestCase("naveen", "Wrong Password")]
        public void AuthController_StaffLoginMethoSd_InValidData_FailTest_InvalidCredetialResult(string Username, string Password)
        {
            try
            {
                _logger.Info("Sixth TestCase - AuthController_StaffLoginMethoSd_InValidData_FailTest_InvalidCredetialResult");
                _AuthRepository.Setup(s => s.StaffLogin(It.IsAny<Staff>())).Returns(false);

                _AuthProviderMock.Setup(s => s.LoginProviderStaff(It.IsAny<Staff>())).Returns("Invalid Credentials");

                _AuthController = new AuthController(_AuthProviderMock.Object);
                Staff Staff = new Staff()
                {

                    Username = Username,
                    Password = Password

                };
                var Response = _AuthController.StaffLogin(Staff) as ObjectResult;
                Assert.AreEqual(404, Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured in AuthController_StaffLoginMethoSd_InValidData_FailTest_InvalidCredetialResult() - " + ex.Message);
            }
           
        }

        [TestCase("garima", "Wrong Password")]
        [TestCase("naveen", "Wrong Password")]
        public void AuthController_StaffLoginMethod_InValidData_FailTest_ExceptionResult(string Username, string Password)
        {
            try
            {
                _logger.Info("Seventh TestCase - AuthController_StaffLoginMethod_InValidData_FailTest_ExceptionResult");
                _AuthRepository.Setup(s => s.StaffLogin(It.IsAny<Staff>())).Returns(false);

                _AuthProviderMock.Setup(s => s.LoginProviderStaff(It.IsAny<Staff>())).Returns("Exception Occured");

                _AuthController = new AuthController(_AuthProviderMock.Object);
                Staff staff = new Staff()
                {

                    Username = Username,
                    Password = Password

                };
                var Response = _AuthController.StaffLogin(staff) as StatusCodeResult;
                Assert.AreEqual(500, Response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.Error("Exception Occured in AuthController_StaffLoginMethod_InValidData_FailTest_ExceptionResult() - " + ex.Message);
            }
            
        }
    }
}
