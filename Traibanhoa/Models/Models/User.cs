using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
            RequestBaskets = new HashSet<RequestBasket>();
        }

        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Displayname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phonenumber { get; set; }
        public bool? Gender { get; set; }
        public string Avatar { get; set; }
        public int? Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsGoogle { get; set; }
        public bool? IsBlocked { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<RequestBasket> RequestBaskets { get; set; }
    }
}
