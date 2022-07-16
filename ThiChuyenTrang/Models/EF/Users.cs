using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ThiChuyenTrang.Models.EF
{
    public partial class Users
    {
        public Users()
        {
            Category = new HashSet<Category>();
            NewsCreatedByNavigation = new HashSet<News>();
            NewsUpdatedByNavigation = new HashSet<News>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public bool? Role { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<News> NewsCreatedByNavigation { get; set; }
        public virtual ICollection<News> NewsUpdatedByNavigation { get; set; }
    }
}
