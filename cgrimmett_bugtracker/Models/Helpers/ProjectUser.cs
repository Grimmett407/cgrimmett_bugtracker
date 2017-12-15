using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cgrimmett_bugtracker.Models.Helpers
{
    public class ProjectUser
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string UserId { get; set; }
    }
}