using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cgrimmett_bugtracker.Models.CodeFirst
{
    public class Notification
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        [Required]
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public string NotifyUserId { get; set; }

        public virtual ApplicationUser NotifyUser { get; set; }
    }
}