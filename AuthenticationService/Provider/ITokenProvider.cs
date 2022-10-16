using AuthenticationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Provider
{
    public interface ITokenProvider
    {
        public string GenerateJwtTokenUser(Parents user);
        public string GenerateJwtTokenEmployee(Staff staff);
    }
}
