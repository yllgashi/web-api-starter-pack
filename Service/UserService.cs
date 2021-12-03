using Models;
using Repository;
using System;

namespace Service
{
    public class UserService
    {
        private UserRepository userRepository;
        public ResponseModel Create(User user)
        {
            userRepository = new UserRepository();
            return userRepository.Create(user);
        }
    }
}
