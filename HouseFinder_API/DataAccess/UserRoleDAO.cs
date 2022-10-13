using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObjects;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserRoleDAO
    {
        public static IEnumerable<RoleDTO> GetUserRoles()
        {
            IEnumerable<RoleDTO> roles;
            try
            {
                using (var context = new FUHouseFinderContext())
                {
                    //Get by Id, include Address
                    MapperConfiguration config;
                    config = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfile()));
                    roles = context.UserRoles.ProjectTo<RoleDTO>(config);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return roles;
        }
    }
}
