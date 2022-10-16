using AuthenticationService.Models;
using AuthenticationService.Provider;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AuthController : ControllerBase
    {
        private IAuthProvider _authProvider;
        public LoggerManager logger = new LoggerManager();
        public AuthController(IAuthProvider authProvider)
        {
            this._authProvider = authProvider;
        }

        /// HttpPost endpoint for User Login
         
        [HttpPost]
        [Route("Login/User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ParentsLogin([FromBody] Parents model)
        {
            logger.Info("Controller :" + model.Email + " requested to Userlogin");
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            try
            {
                var token = _authProvider.LoginProviderParents(model);
                if (token == "Invalid Credentials")
                {
                    logger.Error("Controller :"+model.Email + "Failed to login due to invalid credentials");
                    return new NotFoundObjectResult("Invalid Credentials");
                }
                else if(token == "Exception Occured")
                {
                    logger.Error("Controller :" + model.Email + "Failed to login due to some Exception");
                    return new StatusCodeResult(500);
                }
                else
                {
                    logger.Info("Controller :" + model.Email + " logged in successfully");
                    return Ok(token);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Controller : Exception occured in AuthController while calling authProvider as :" + ex.Message);
                return new StatusCodeResult(500);
            }
        }

       
        ///  HttpPost endpoint for User Login
        
        [HttpPost]
        [Route("Login/Employee")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult StaffLogin([FromBody] Staff model)
        {
            logger.Info("Controller :" + model.Username + " requested to login");
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            try
            {
                var token = _authProvider.LoginProviderStaff(model);
                if (token == "Invalid Credentials")
                {
                    logger.Error("Controller : " + model.Username + "Failed to login due to invalid credentials");
                    return new NotFoundObjectResult("Invalid Credentials");
                }
                else if (token == "Exception Occured")
                {
                    logger.Error("Controller : " + model.Username + "Failed to login due to Exception!");
                    return new StatusCodeResult(500);
                }
                else
                {
                    logger.Info("Controller : " + model.Username + " logged in successfully");
                    return Ok(token);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Controller : Exception occured in AuthController while calling authProvider as :" + ex.Message);
                return new StatusCodeResult(500);
            }
        }
       
    }
}
