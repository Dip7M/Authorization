using AuthenticationService.Models;
using AuthenticationService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;



namespace AuthenticationService.Provider
{
    public class AuthProvider : IAuthProvider
    {
        private  readonly IAuthRepo _authRepository = null;
        private readonly ITokenProvider _tokenProvider = null;
        private log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(AuthProvider));

        public LoggerManager logger = new LoggerManager();
        public IConfiguration Configuration {get;}
        //construtor AuthProvider
        public AuthProvider(IAuthRepo authRepository, ITokenProvider tokenProvider,IConfiguration configuration)
        {
            this._authRepository = authRepository;
            this._tokenProvider = tokenProvider;
            this.Configuration = configuration;
        }

        //Business Logic for LoginUser
        public string LoginProviderParents(Parents user)
        {
            logger.Info("User Provider - "+user.Email + " requested Login provider user");
            string token = null;
            try
            {
                List<Parents> Item =    _authRepository.GetAllParents(); // Getting List of all Parents.
                Parents users = new Parents();
                users = null;
                foreach (var model in Item)
                {
                    _logger.Info(model.RegId+" " +model.Email + " " + model.Pwd);

                    if (user.RegId==model.RegId && user.Email == model.Email && user.Pwd == model.Pwd) // checking if Parent is present of not
                    {
                        users = model;
                        logger.Info("User Provider - " + user.Email + " Matched.");
                        break;
                    }

                }
                if (users == null)
                {
                    token = "Invalid Credentials";
                    logger.Error("User Provider - " + user.Email + " Invalid Credentials - RegId or Email or Password is Incorrect!");
                    throw new Exception("Invalid Credentials - Email or Password is Incorrect!");
                }
                
                bool authorizeSucceed = _authRepository.ParentsLogin(users); // Second verification of credentials with DTO model.
                if (authorizeSucceed)
                {
                    logger.Info(users.Email + " Authenticated successfully");
                    token = _tokenProvider.GenerateJwtTokenUser(user); // JWT token generated
                    return token;
                }
                else
                {
                    logger.Error("User Provider - " + users.Email + " Attempted to login with invalid credentials. No token generated!");
                    token = "Invalid Credentials";
                    throw new Exception(users.Email + " Attempted to login with invalid credentials"); // Exception thrown for invalid credentials
                }

            }
            catch(Exception e)
            {
                
                logger.Error("User Provider - Some Exception occured " + e.Message);//Exception handled
            }
            if (token == null)
            {
                token = "Exception Occured";
            }
            return token;

        }
        
        //    END of LoginProviderUser().


        //Business Logic for EmployeeLogin
        public string LoginProviderStaff(Staff staff)
        {
            logger.Info("Employee Provider - " + staff.Username + " requested Login provider employee");
            string token = null;
            try
            {
                List<Staff> Item = _authRepository.GetAllStaffs(); //Getting list of all employees
                Staff staffs = new Staff();
                foreach (var model in Item)
                {

                    if (staff.Username == model.Username && staff.Password == model.Password)//checking credentials of employee
                    {
                        staffs = model;
                        logger.Info("Employee Provider - " + staff.Username + " Matched.");
                        break;
                    }
                    else
                    {
                        token = "Invalid Credentials"; //Setting token with Invalid credentials string for handling response result.
                        logger.Error("Employee Provider - " + staff.Username + " Invalid Credentials - Username or Password is Incorrect!");
                        throw new Exception("Invalid Credentials - Username or Password is Incorrect!");
                    }
                }
                bool authorizeSucceed = _authRepository.StaffLogin(staffs);  //second confirmation of same employee present or not.
                if (authorizeSucceed)
                {
                    logger.Info("Employee Provider - " + staffs.Username + " Authenticated successfully");
                    token = _tokenProvider.GenerateJwtTokenEmployee(staffs);   //JWT token Generated
                    return token;
                }
                else
                {
                    token = "Invalid Credentials"; //Setting token with Invalid credentials string for handling response result.
                    logger.Warn("Employee Provider - " + staffs.Username + " Attempted to login with invalid credentials. No token generated!");
                    throw new Exception(staffs.Username + " Attempted to login with invalid credentials"); // exception thrown
                }

            }
            catch (Exception e)
            {
                logger.Warn("Employee Provider - Some Exception occured " + e.Message);  //Exception Handled
            }
            if (token == null)
            {
                logger.Error("Employee Provider - Some Exception occured and token is not generated so assigning Exception Occured string to token ");
                token = "Exception Occured"; //Setting token with Exception Occured  string for handling response result.
            }
            return token;
        }
        
    }
}
