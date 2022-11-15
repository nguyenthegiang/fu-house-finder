using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IUserReposiotry
    {
        UserDTO GetUserByID(string UserId);
        public ResponseDTO Login(LoginDTO login);
        public ResponseDTO Register(RegisterDTO register);
        public List<UserDTO> GetLandlords();
        //public List<UserDTO> GetStaffs();
        public int CountTotalLandlord();
    }
}
