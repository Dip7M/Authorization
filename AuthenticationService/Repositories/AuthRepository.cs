using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Repositories
{
    public class AuthRepository : IAuthRepo
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(AuthRepository));
        private static List<Parents> _parents = new List<Parents>()
        {
            new Parents {RegId = 1, Email="Alok@gmail.com", Pwd="Alok@123"},
            new Parents {RegId = 2,Email="Dip@gmail.com", Pwd="Dip@123"},
        };
           
        private static List<Staff> _staff = new List<Staff>()
        {
            new Staff(){StaffId=11, Username="Karthik",Password="Karthik@123"},
            new Staff(){StaffId=12, Username="Gurupreet",Password="Gurupreet@123"},
            
        };


        public LoggerManager logger = new LoggerManager();

        //Repository functions for getting all users
        public List<Parents> GetAllParents()
        {
            try
            {
                logger.Info("Repository : User list sent ");
                return _parents;

            }
            catch (Exception e)
            {
                logger.Error("Repository : Unable to fetch the users" + e.Message);
                return null;
            }
           
        }

        //Repository functions for getting all users
        public List<Staff> GetAllStaffs()
        {
            try
            {
                logger.Info("Repository : List Sent!");
                return _staff;

            }
            catch (Exception e)
            {
                logger.Error("Repository : Unable to fetch the employees" + e.Message);
                return null;
            }
        }

        //Functionality for UserLogin().
        public bool ParentsLogin(Parents user)
        {
            try
            {
                foreach(Parents model in _parents)
                {
                    if(model.RegId == user.RegId && model.Email==user.Email && model.Pwd == user.Pwd)
                    {
                        logger.Info("Repository : Model Found UserDTO!");
                        return true;
                    }
                }

            }
            catch(Exception e)
            {
                logger.Error("Repository : User Authentication Failed due to " + e.Message);
            }
            return false;
        }

        //Functionality for EmployeeLogin()
        public bool StaffLogin(Staff staff)
        {
            try
            {
                foreach (Staff model in _staff)
                {
                    if (model.StaffId == staff.StaffId && model.Username == staff.Username && model.Password == staff.Password)
                    {
                        logger.Info("Repository : Model FOund EmployeeDTO");
                        return true;
                    }
                }

            }
            catch (Exception e)
            {
                logger.Error("Employee Authentication Failed due to " + e.Message);
            }
            return false;
        }

       
    }
    
}
