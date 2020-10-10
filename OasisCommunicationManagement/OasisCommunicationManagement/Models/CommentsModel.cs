using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OasisCommunicationManagement.Models
{
    public class CommentsModel
    {
        public int id { get; set; }

        [Required]
        public string comments { get; set; }
        public int Fk_commentor_ID { get; set; }
        public  int Fk_task_ID { get; set; }
        public DateTime DateCommented { get; set; }
        public string commenter { get; set; }

    }
}