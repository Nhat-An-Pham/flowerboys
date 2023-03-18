using System;

namespace Traibanhoa.Modules.UserModule.Response
{
    public class CurrentUserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public int? Gender { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
    }
}
