using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Repositories
{
    //Repository
    public interface IAuthRepo
    {
        public List<Parents> GetAllParents();
        public List<Staff> GetAllStaffs();
        public bool ParentsLogin(Parents user);
        public bool StaffLogin(Staff staff);

    }
}
