using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseFinder_API.Authentication
{
    public interface IAuthentication
    {
        public string Authenticate(ResponseDTO user);
    }
}
