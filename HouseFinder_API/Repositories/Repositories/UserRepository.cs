using DataAccess;
using DataAccess.DTO;
using Repositories.IRepository;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class UserRepository : IUserReposiotry
    {
        //Get list of landlords
        public List<UserDTO> GetLandlords() => UserDAO.GetLandlords();
        //Get list of staffs
        public List<UserDTO> GetStaffs() => UserDAO.GetStaffs();

        public UserDTO GetUserByID(string UserId) => UserDAO.GetUserById(UserId);
        public ResponseDTO Login(LoginDTO login)
        {
            ResponseDTO user;
            if (login.FacebookUserId != null)
            {
                user = UserDAO.LoginFacebook(login.FacebookUserId);
            }
            else if (login.GoogleUserId != null)
            {
                user = UserDAO.LoginGoogle(login.GoogleUserId);
            }
            else
            {
                user = UserDAO.LoginUsername(login.Email, login.Password);
            }
            return user;
        }
        public ResponseDTO Register(RegisterDTO register)
        {
            ResponseDTO user = UserDAO.Register(
                register.FacebookUserId, 
                register.GoogleUserId, 
                register.Email, 
                register.DisplayName,
                (int)register.RoleId,
                register.IdentityCardFrontSideImageLink,
                register.IdentityCardBackSideImageLink, 
                register.PhoneNumber,
                register.FacebookUrl
            );
            return user;
        }
    }
}
