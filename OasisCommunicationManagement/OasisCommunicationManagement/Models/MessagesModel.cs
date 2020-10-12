using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OasisCommunicationManagement.Models
{
    public class MessagesModel
    {
        public int id { get; set; }
        public int SenderID { get; set; }
        public int REcieverID { get; set; }

        public DateTime DateSent { get; set; }
        public string messages { get; set; }
        public String Attancement { get; set; }


    }
}