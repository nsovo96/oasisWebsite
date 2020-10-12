using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OasisCommunicationManagement.Models
{
    public class notificationModel
    {
        public int id { get; set; }
        public  string RecieverRole { get; set; }
        public string Notification { get; set; }
        public DateTime datesent { get; set; }

        public int Fk_senderID { get; set; }
        public int Fk_TaskID { get; set; }

    }
}