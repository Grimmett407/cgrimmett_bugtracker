using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cgrimmett_bugtracker.Models.CodeFirst
{
    public class Project
    {
        public Project()
        {
            this.IsActive = true;
            this.Tickets = new HashSet<Ticket>();
            this.Users = new HashSet<ApplicationUser>();
            
        }

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string AuthorId { get; set; }
        public string AssignedId { get; set; }

        public virtual ApplicationUser Assigned { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}