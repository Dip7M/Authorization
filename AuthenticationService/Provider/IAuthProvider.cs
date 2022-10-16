using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Provider
{
    public interface IAuthProvider
    {
        public string LoginProviderParents(Parents user);
        public string LoginProviderStaff(Staff staff);

       
    }
}
