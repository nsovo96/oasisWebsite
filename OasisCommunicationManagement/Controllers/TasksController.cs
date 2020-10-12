using BusinessLogic;
using DataAccessLayer.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace OasisCommunicationManagement.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        public ActionResult NewTask(string tasktype)
        {
            Session.Remove("tasktype");
            Session["tasktype"] = tasktype;


            return View();
        }

     


       
        [HttpPost]
        public ActionResult OasisTask(string NewTask)
        {


            Task MynewTask = new Task("Insert_no_model");

            MynewTask.NewTask(Convert.ToInt32(Session["userId"]), NewTask);


            return RedirectToAction("GetTask");
;
        }
        public ActionResult TaskColobaration()
        {
            Task notifications = new Task("Select");

            List<Tasks> pNotiList = new List<Tasks>();


            pNotiList = notifications.GetTasks();


            List<Users> userManager = new List<Users>();


            UserAccounts AccountList = new UserAccounts("Select");

            Session["userdetails"] = AccountList.Login();

            return View(pNotiList);

        }



        public ActionResult GetTask()
        {
            Task notifications = new Task("Select");

            List<Tasks> pNotiList = new List<Tasks>();

            pNotiList = notifications.GetTasks();

            Session.Remove("taskDetail");

            return View(pNotiList);


        }

        static  int  Taskid;
        static    List<Comments> pNotiList = new List<Comments>();

        public ActionResult Colaboration(int TaskId)
        {
            Taskid = TaskId;
            Session.Remove("taskID");

                
            Coloboration coloborations = new Coloboration("Select");

            pNotiList = coloborations.GetComments(TaskId);
            Session["taskID"] = TaskId;
            return View(pNotiList);
        }


        [HttpPost]
        public ActionResult MyColaboration(string Comment)
        {

            Coloboration coloborations = new Coloboration("Insert");

            coloborations.SendCollab(Convert.ToInt32(Session["userId"]), Taskid, Comment, Session["taskDetail"].ToString());

            return RedirectToAction("Colaboration", new { Taskid = Taskid }); ;
        }


        public ActionResult AddNotification(int TaskID)
        {

            Task Mytask = new Task("Select");
            
             Session.Remove("taskID");


            Tasks t = new Tasks();


            List<Tasks> TasksValues = Mytask.GetTasks();


            foreach (var myt in TasksValues)
            {
                if(myt.id== TaskID)
                {
                    t = myt;
                }
            }
            Session["taskID"] = TaskID;


            return View(t);
        }


        [HttpPost]
        public ActionResult AddNotification(string Notification)
        {
            NotificationsDisplay notifications = new NotificationsDisplay();
            notifications.CreateAnotification("Has finished the task New Data " +  Notification  , "Manager");
            return RedirectToAction("Index", "Home");
        }






        static int EditTaskID;
        public ActionResult UpdateTask(int TaskID)
        {
            EditTaskID = TaskID;
            Session.Remove("EditaskID");

            if(Session["UserRole"].ToString()== "ProccessMaintananceEmployee")
            {
                return RedirectToAction("AddNotification", new { TaskID = TaskID });

            }
            else
            {
                Task updateTask = new Task("Update");

                updateTask.Update(TaskID);


                return RedirectToAction("GetTask");

            }

        
        }


        public ActionResult EmployeeDailyTask(int UserID)
        {

            return View();
        }

        public ActionResult ManagementCheckList()
        {

            return View();
        }


        [HttpPost]
        public ActionResult ManagementCheckListB(
            decimal _4_Softner,
           decimal _5_1_Micron_Preasure,
           decimal _6_Membrane_Preasure,
           decimal _7_Flow_Rate_RAW,
             decimal _2_PF_5_Micron,
           decimal _1_PF_10_Micron,
           decimal TDs_Main,
           decimal TDs_clean,
           decimal _3_Sand_Filter


            )
        {
            CheckList checkList = new CheckList("InsertB");
            checkList.AddcheckListB(
               _4_Softner,
            _5_1_Micron_Preasure,
            _6_Membrane_Preasure,
            _7_Flow_Rate_RAW,
              _2_PF_5_Micron,
            _1_PF_10_Micron,
            TDs_Main,
            TDs_clean,
            _3_Sand_Filter);


            return RedirectToAction("ManagementCheckList");
        }

        public ActionResult GetCheckList()
        {
            List<ManagementCheckList> managementCheckLists = new List<ManagementCheckList>();
            CheckList checkList = new CheckList("Select");
            managementCheckLists = checkList.GetManagementCheckLists();
            return View(managementCheckLists);
        }

        static int listID;
        public ActionResult EditCheckList(int ChecklistID)
        {
            listID = ChecklistID;

            return View();
        }
        [HttpPost]
        public ActionResult EditCheckList(decimal _4_Softner = 0,
           decimal _5_1_Micron_Preasure = 0,
           decimal _6_Membrane_Preasure = 0,
           decimal _7_Flow_Rate_RAW = 0,
           string _8_Flow_Rate_Clean = "",
           string _9_Pump_Functional = "",
           decimal Conductive = 0,
           decimal Clean_Water_Meter_Reading = 0,
           decimal Litres_Purified = 0,
            decimal _3_Sand_Filter = 0,
           decimal _2_PF_5_Micron = 0,
           decimal _1_PF_10_Micron = 0,
           decimal Clean_tank_level = 0,
           decimal Mun_Tank_level = 0,
           string Silcone_Crystal_Color ="",
           string Ozone = "",
           decimal TDs_Main = 0,
           decimal TDs_clean = 0)
        {
            List<ManagementCheckList> managementCheckLists = new List<ManagementCheckList>();
            CheckList checkList = new CheckList();


            checkList.UpdateString
                (listID,
               
                _4_Softner,
            _5_1_Micron_Preasure,
            _6_Membrane_Preasure,
            _7_Flow_Rate_RAW,
            _8_Flow_Rate_Clean,
            _9_Pump_Functional,
            Conductive,
            Clean_Water_Meter_Reading,
            Litres_Purified,

              _2_PF_5_Micron,
            _1_PF_10_Micron,
            Clean_tank_level,
            Mun_Tank_level,
            Silcone_Crystal_Color,
            Ozone,
            TDs_Main,
            TDs_clean,
            _3_Sand_Filter);


            return RedirectToAction("GetCheckList");
        }


        public ActionResult Delete(int ChecklistID)
        {
            CheckList CheckLists = new CheckList("Delete");




            DialogResult dr = MessageBox.Show("Are you sure to delete row?", "Confirmation", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                CheckLists.Delete(ChecklistID);
                return RedirectToAction("GetCheckList");

            }
            else if (dr == DialogResult.No)
            {
            }


            return RedirectToAction("GetCheckList");



        }
    }

 
   
}