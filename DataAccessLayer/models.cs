using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
     public  class Product 
    {

        public int id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Decimal CurrentPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }


    public class Users
    {
        public int id { get; set; }



        [Required]
        [StringLength(100, ErrorMessage = "Enter a valid name", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string FullNames { get; set; }




        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string UserRole { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Enter password")]
        public string Password { get; set; }


        public string isActive { get; set; }
    }



    public class Tasks
    {
        public int id { get; set; }
        public int Fk_EmployeeID { get; set; }

        public DateTime dateAssigned { get; set; }
        public DateTime DateFinished { get; set; }

        public DateTime DueDate { get; set; }

        public string TaskStatus { get; set; }
        [Required]
        public string Taskdetail { get; set; }
        public string TaskType { get; set; }

    }


    public class Notifications
    {
        public int id { get; set; }
        public string RecieverRole { get; set; }
        public string Notification { get; set; }
        public DateTime datesent { get; set; }
        public int Fk_TaskID { get; set; }
        public string isActive { get; set; }

        
    }


    public class Comments
    {
        public int id { get; set; }
        [Required]
        public string comments { get; set; }
        public int Fk_commentor_ID { get; set; }
        public int Fk_task_ID { get; set; }
        public DateTime DateCommented { get; set; }

    }


    public class ManagementCheckList
    {

    
  public int  Id { get; set; }

         public DateTime  date  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal TDs_clean { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal TDs_Main { get; set; }
        [Required]
        public string Ozone { get; set; }
        [Required]
        public string Silcone_Crystal_Color { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal Mun_Tank_level { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal Clean_tank_level { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal _1_PF_10_Micron  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal _2_PF_5_Micron  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal _3_Sand_Filter  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal _4_Softner  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal _5_1_Micron_Preasure  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal _6_Membrane_Preasure  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal _7_Flow_Rate_RAW  { get; set; }
        [Required]
        public string    _8_Flow_Rate_Clean  { get; set; }
        [Required]
        public string   _9_Pump_Functional  { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal Conductive { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal Clean_Water_Meter_Reading { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal Litres_Purified_Since_Previous_Reading { get; set; }
        [Required]
        [Range(typeof(decimal), "1", "100", ErrorMessage = "Invalid ")]
        public decimal Attendence { get; set; }

     public string   CheckListType { get; set; }
    }

}