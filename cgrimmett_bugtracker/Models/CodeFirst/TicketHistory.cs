using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cgrimmett_bugtracker.Models.CodeFirst
{
    public class TicketHistory
    {
        public TicketHistory()
        {

        }

        public int Id { get; set; }
        public int TicketId { get; set; }
        [Required]
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset ChangeDate { get; set; }
        public string UserId { get; set; }

    }
}