using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cgrimmett_bugtracker.Models.CodeFirst
{
    public class TicketAttachment
    {
        public TicketAttachment()
        {
                
        }

        public int Id { get; set; }
        public int TicketId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string MediaUrl { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string AuthorUserId { get; set; }
        public string Description { get; set; }
    }
}