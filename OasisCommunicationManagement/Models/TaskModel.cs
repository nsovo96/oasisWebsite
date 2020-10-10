using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OasisCommunicationManagement.Models
{
    public class TaskModel
    {
        public int id { get; set; }
        public int Fk_NotificationID { get; set; }
        public int Fk_EmployeeID { get; set; }

        public DateTime dateAssigned { get; set; }
        public DateTime DateFinished  { get; set; }

        public DateTime DueDate { get; set; }

       public string TaskStatus { get; set; }

        public string Taskdetail { get; set; }

        public string comments { get; set; }

       

    }
}