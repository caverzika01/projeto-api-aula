using _2TDSPK.Database.Models;
using _2TDSPK.Repository.Interface;
using _2TDSPK.API.DTO.Request;

namespace _2TDSPK.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserLike> _userLikeRepository;

        public UserService(IRepository<User> userRepository, IRepository<UserLike> userLikeRepository)
        {
            _userRepository = userRepository;
            _userLikeRepository = userLikeRepository;
        }


        public void CreateUser(UserRequest userRequest)
        {
            if (userRequest is null) new Exception("Usuario nao pode ser nulo");

            var userExists = _userRepository.GetAll().Where(x => x.Email == userRequest.Email).Count() > 0;

            if (!userExists)
            {
                User user = new User(userRequest.Email, userRequest.Password);

                _userRepository.Add(user);
            }     
        }

        public void UserLike(UserLike userLike)
        {
            _userLikeRepository.Add(userLike);
        }
    }
}
