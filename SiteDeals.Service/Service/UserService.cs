//using Microsoft.AspNetCore.Cryptography.KeyDerivation;
//using SiteDeals.Core.Model;
//using SiteDeals.Repository.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SiteDeals.Service.Service
//{
//    public class UserService
//    {
//        private readonly UserRepository _userRepository;
//        public UserService(UserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }

//        public async Task<User> Register(User user)
//        {
//            user.CreatedAt = DateTime.Now;
//            user.PasswordHash = await Hash(user.PasswordHash).ConfigureAwait(false);
//            await _userRepository.AddAsync(user).ConfigureAwait(false);
//            return user;
//        }

//        //public async Task<User> Authenticate(string usernameOrEmail, string password)
//        //{
//        //    var hashedPassword = await Hash(password).ConfigureAwait(false);
//        //    return await _userRepository.Authenticate(usernameOrEmail, hashedPassword).ConfigureAwait(false);
//        //}

//        public async Task<string> Hash(string text)
//        {
//            var salt = Encoding.UTF8.GetBytes("saltBaeNusret");
//            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
//            password: text,
//            salt: salt,
//            prf: KeyDerivationPrf.HMACSHA256,
//            iterationCount: 100000,
//            numBytesRequested: 256 / 8));
//            return hash;
//        }
//    }
//}
