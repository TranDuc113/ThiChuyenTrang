using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ThiChuyenTrang.Models.EF
{
    public partial class Category
    {
        public Category()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Slug { get; set; }

        public virtual Users CreatedByNavigation { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
