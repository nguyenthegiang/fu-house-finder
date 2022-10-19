using DataAccess;
using DataAccess.DTO;
using Repositories.IRepository;

namespace Repositories.Repositories
{
    public class UserRepository : IUserReposiotry
    {
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
                (int)register.RoleId
            );
            return user;
        }
    }
}
