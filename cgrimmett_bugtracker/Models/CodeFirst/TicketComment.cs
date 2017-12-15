using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cgrimmett_bugtracker.Models.CodeFirst
{
    public class TicketComment
    {

        public TicketComment()
        {
                
        }

        public int Id { get; set; }
        public int TicketId { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string AuthorUserId { get; set; }

        public virtual Ticket ticket { get; set; }
        public virtual ApplicationUser AuthorUser { get; set; }
    }
}