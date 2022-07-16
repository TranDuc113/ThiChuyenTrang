using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ThiChuyenTrang.Models.EF
{
    public partial class News
    {
        public News()
        {
            File = new HashSet<File>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool? IsDelete { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? Views { get; set; }
        public string Slug { get; set; }

        public virtual Category Category { get; set; }
        public virtual Users CreatedByNavigation { get; set; }
        public virtual Users UpdatedByNavigation { get; set; }
        public virtual ICollection<File> File { get; set; }
    }
}
