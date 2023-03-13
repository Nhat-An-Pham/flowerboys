using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Constant;
using Models.Models;
using Models.UserDTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Traibanhoa.Modules.CustomerModule.Interface;
using Traibanhoa.Modules.UserModule.Interface;
using Traibanhoa.Modules.UserModule.Request;

namespace Traibanhoa.Modules.UserModule
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly AppSetting _appSettings;
        public UserService(IUserRepository UserRepository, ICustomerRepository customerRepository, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _UserRepository = UserRepository;
            _customerRepository = customerRepository;
            _appSettings = optionsMonitor.CurrentValue;
        }

        public async Task<ICollection<User>> GetAll()
        {
            return await _UserRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }

        public Task<ICollection<User>> GetUsersBy()
        {
            return _UserRepository.GetUsersBy(x => x.IsBlocked == false, options: o => o.OrderByDescending(x => x.UpdatedDate).ToList());
        }


        public async Task<Guid?> AddNewUser(CreateUserRequest UserRequest)
        {

            ValidationResult result = new CreateUserRequestValidator().Validate(UserRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }
            var newUser = new User();

            newUser.UserId = Guid.NewGuid();
            newUser.Username = UserRequest.Username;
            newUser.Name = UserRequest.Name;
            newUser.Email = UserRequest.Email;
            newUser.Password = UserRequest.Password;
            newUser.Phonenumber = UserRequest.Phonenumber;
            newUser.Gender = UserRequest.Gender;
            newUser.Avatar = UserRequest.Avatar;
            newUser.Role = UserRequest.Role;
            newUser.CreatedDate = DateTime.Now;
            newUser.UpdatedDate = DateTime.Now;
            newUser.IsGoogle = UserRequest.IsGoogle;
            newUser.IsBlocked = false;

            await _UserRepository.AddAsync(newUser);
            return newUser.UserId;
        }

        public async Task UpdateUser(UpdateUserRequest userRequest)
        {
            try
            {
                var userUpdate = GetUserByID(userRequest.UserId).Result;

                if (userUpdate == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                ValidationResult result = new UpdateUserRequestValidator().Validate(userRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                userUpdate.Username = userRequest.Username;
                userUpdate.Name = userRequest.Name;
                userUpdate.Password = userRequest.Password;
                userUpdate.Phonenumber = userRequest.Phonenumber;
                userUpdate.Avatar = userRequest.Avatar;
                userUpdate.IsBlocked = userRequest.IsBlocked;
                userUpdate.UpdatedDate = DateTime.Now;

                await _UserRepository.UpdateAsync(userUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task DeleteUser(Guid? userDeleteID)
        {
            try
            {
                if (userDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                User userDelete = _UserRepository.GetFirstOrDefaultAsync(x => x.UserId == userDeleteID && x.IsBlocked == false).Result;

                if (userDelete == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                userDelete.IsBlocked = true;
                await _UserRepository.UpdateAsync(userDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<User> GetUserByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.UserId == id);
            if (user == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return user;
        }

        #region Authentication

        public async Task<string> GenerateToken(LoginDTO login)
        {
            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Username == login.Username && x.Password == EncryptPassword(login.Password));
            if (user == null)
            {
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Username == login.Username && x.Password == EncryptPassword(login.Password));

                if (customer != null && customer.IsBlocked == false)
                {
                    var jwtTokenHandlerCustomer = new JwtSecurityTokenHandler();

                    var secretKeyBytesCustomer = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

                    var tokenDescriptionCustomer = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", customer.CustomerId.ToString()),
                    new Claim(ClaimTypes.Name, customer.Username),
                    new Claim(ClaimTypes.Email, customer.Email == null ? "" : customer.Email),
                    new Claim("Phone Number", customer.Phonenumber),
                    new Claim("Displayname", customer.Displayname == null ? "" : customer.Displayname),
                    new Claim("Name", customer.Name),
                    new Claim(ClaimTypes.Gender,customer.Gender.ToString()),
                    new Claim("Avatar", customer.Avatar  == null ? "" : customer.Avatar),
                    new Claim(ClaimTypes.Role, RoleConst.CUSTOMER)

                }),
                        
                        Expires = DateTime.UtcNow.AddMinutes(60),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytesCustomer), SecurityAlgorithms.HmacSha512Signature)
                    };

                    var tokenCustomer = jwtTokenHandlerCustomer.CreateToken(tokenDescriptionCustomer);
                    return jwtTokenHandlerCustomer.WriteToken(tokenCustomer);
                }
            }
            else if (user.IsBlocked == false)
            {

                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);


                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email == null ? "" : user.Email),
                    new Claim("Phone Number", user.Phonenumber),
                    new Claim("Displayname", user.Displayname == null ? "" : user.Displayname),
                    new Claim("Name", user.Name),
                    new Claim(ClaimTypes.Gender,user.Gender.ToString()),
                    new Claim("Avatar", user.Avatar  == null ? "" : user.Avatar),
                    new Claim(ClaimTypes.Role, user.Role == 1 ? RoleConst.MANAGER : RoleConst.STAFF )
                }),

                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)

                };
                var token = jwtTokenHandler.CreateToken(tokenDescription);
                return jwtTokenHandler.WriteToken(token);
            }
            return null;
        }

        public async Task<string> GenerateGoolgleToken(LoginGoogleDTO loginGoogle)
        {

            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Email == loginGoogle.Email);
            if (user == null)
            {
                var customer = await _customerRepository.GetFirstOrDefaultAsync(x => x.Email == loginGoogle.Email);

                if (customer != null && customer.IsBlocked == false)
                {
                    var jwtTokenHandlerCustomer = new JwtSecurityTokenHandler();

                    var secretKeyBytesCustomer = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

                    var tokenDescriptionCustomer = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[] {

                    new Claim("Id", customer.CustomerId.ToString()),
                    new Claim(ClaimTypes.Name, customer.Username),
                    new Claim(ClaimTypes.Email, customer.Email == null ? "" : customer.Email),
                    new Claim("Phone Number", customer.Phonenumber),
                    new Claim("Displayname", customer.Displayname == null ? "" : customer.Displayname),
                    new Claim("Name", customer.Name),
                    new Claim(ClaimTypes.Gender,customer.Gender.ToString()),
                    new Claim("Avatar", customer.Avatar  == null ? "" : customer.Avatar),
                    new Claim(ClaimTypes.Role, RoleConst.CUSTOMER)

                }),
                        Expires = DateTime.UtcNow.AddMinutes(60),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytesCustomer), SecurityAlgorithms.HmacSha512Signature)
                    };
                    var tokenCustomer = jwtTokenHandlerCustomer.CreateToken(tokenDescriptionCustomer);
                    return jwtTokenHandlerCustomer.WriteToken(tokenCustomer);
                }
                else if (customer == null)
                {
                    var cus = new Customer
                    {
                        Email = loginGoogle.Email,
                        Displayname = loginGoogle.Displayname,
                        Avatar = loginGoogle.Avatar
                    };
                    cus.CustomerId = Guid.NewGuid();
                    cus.IsBlocked = false;
                    cus.IsGoogle = true;
                    await _customerRepository.AddAsync(cus);
                    var jwtTokenHandlerCustomer = new JwtSecurityTokenHandler();

                    var secretKeyBytesCustomer = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

                    var tokenDescriptionCustomer = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[] {


                    new Claim("Id", cus.CustomerId.ToString()),
                    new Claim(ClaimTypes.Name, cus.Username == null ? "": cus.Username),
                    new Claim(ClaimTypes.Email, cus.Email),
                    new Claim("Phone Number", cus.Phonenumber == null ? "": cus.Phonenumber),
                    new Claim("Displayname", cus.Displayname == null ? "" : cus.Displayname),
                    new Claim("Name",cus.Name == null ? "": cus.Name),
                    new Claim(ClaimTypes.Gender,cus.Gender.ToString() == null ? "": cus.Gender.ToString()),
                    new Claim("Avatar", cus.Avatar  == null ? "" : cus.Avatar),
                    new Claim(ClaimTypes.Role, RoleConst.CUSTOMER)

                }),
                        Expires = DateTime.UtcNow.AddMinutes(60),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytesCustomer), SecurityAlgorithms.HmacSha512Signature)
                    };
                    var tokenCustomer = jwtTokenHandlerCustomer.CreateToken(tokenDescriptionCustomer);
                    return jwtTokenHandlerCustomer.WriteToken(tokenCustomer);
                }
            }
            else if (user.IsBlocked == false)
            {

                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {

                    new Claim("Id", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username == null ? "": user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Phone Number", user.Phonenumber == null ? "": user.Phonenumber),
                    new Claim("Displayname", user.Displayname),
                    new Claim("Name", user.Name == null ? "": user.Name),
                    new Claim(ClaimTypes.Gender,user.Gender.ToString() == null ? "": user.Gender.ToString()),
                    new Claim("Avatar", user.Avatar  == null ? "" : user.Avatar),
                    new Claim(ClaimTypes.Role, user.Role == 1 ? RoleConst.MANAGER : RoleConst.STAFF )
                }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
                };

                var token = jwtTokenHandler.CreateToken(tokenDescription);
                return jwtTokenHandler.WriteToken(token);


            }
            return null;
        }
        public async Task Register(RegisterDTO register)
        {
            var cus = GetCustomerByUsername(register.Username);
            var user = GetUserByUsername(register.Username);
            if (cus != null || user != null) throw new Exception(ErrorMessage.UserError.USER_EXISTED);

            register.Password = EncryptPassword(register.Password);
            var customer = new Customer
            {
                Username = register.Username,
                Gender = register.Gender,
                Name = register.Name,
                Displayname = register.Name,
                Password = register.Password,
                Phonenumber = register.Phonenumber
            };
            customer.CustomerId = Guid.NewGuid();
            customer.IsBlocked = false;
            customer.CreatedDate = DateTime.Now;
            await _customerRepository.AddAsync(customer);
        }

        public string EncryptPassword(string plainText)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptPassword(string plainText)
        {
            var key = "b14ca5898a4e4133bbce2ea2315a1916";
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        #endregion

        public Customer GetCustomerByUsername(string? username)
        {
            return _customerRepository.GetFirstOrDefaultAsync(x => x.Username == username).Result;
        }
        public User GetUserByUsername(string? username)
        {
            return _UserRepository.GetFirstOrDefaultAsync(x => x.Username == username).Result;
        }
    }
}
