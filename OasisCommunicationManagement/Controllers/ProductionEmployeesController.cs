using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OasisCommunicationManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;
using System.Web.Services.Description;
using System.Windows.Forms;


namespace OasisCommunicationManagement.Controllers
{
    public class ProductionEmployeesController : Controller
    {
        // GET: ProductionEmployees
        private string OasisConnectionManager;
        private SqlConnection OasisConnection;
        public ActionResult Index()
        {

            List<notificationModel> Noticafition = new List<notificationModel>();

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            DataTable dataTable = new DataTable();
            OasisConnection.Open();

            var query = "select * from messages where FK_RecieverID=" + Convert.ToInt32(Session["id"]);

            SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);
            int count = 0;
            using (SqlDataReader rd = SelectCommand.ExecuteReader())
            {
                Session["totalMsg"] = rd.FieldCount;

                while (rd.Read())
                {
                    if (rd["MsgStatus"].ToString() == "Sent")
                    {
                        count = count + 1;
                    }

                }

                Session["msgNum"] = count;

            }

            if (OasisConnection.State == ConnectionState.Open)
            {
                OasisConnection.Close();





                OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

                OasisConnection = new SqlConnection(OasisConnectionManager);

                OasisConnection.Open();

                var queryNotification = "select * from Notifications where RecieverRole='" + "ProductionEmployee" + "'";

                SqlCommand SelectNotificatoinCommand = new SqlCommand(queryNotification, OasisConnection);

                using (SqlDataReader rd = SelectNotificatoinCommand.ExecuteReader())
                {

                    while (rd.Read())
                    {
                        notificationModel notify = new notificationModel();
                        notify.id = Convert.ToInt32(rd["id"]);

                        notify.RecieverRole = rd["RecieverRole"].ToString();
                        notify.Notification = rd["Notification"].ToString();
                        notify.datesent = Convert.ToDateTime(rd["datesent"]);
                        Noticafition.Add(notify);
                    }

                }

            }

            return View(Noticafition);
        }



        public ActionResult NotificationDetail(int NotificationID)
        {

            List<TaskModel> Noticafition = new List<TaskModel>();
            Session["notificationID"] = NotificationID;
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            DataTable dataTable = new DataTable();
            OasisConnection.Open();

            var query = "select * from Notifications where id='" + NotificationID + "'";

            SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);
            notificationModel notify = new notificationModel();

            using (SqlDataReader rd = SelectCommand.ExecuteReader())
            {

                if (rd.Read())
                {

                    notify.id = Convert.ToInt32(rd["id"]);
                    notify.Fk_TaskID = Convert.ToInt32(rd["Fk_TaskID"]);
                    notify.RecieverRole = rd["RecieverRole"].ToString();
                    notify.Notification = rd["Notification"].ToString();
                    notify.datesent = Convert.ToDateTime(rd["datesent"]);

                    OasisConnection.Close();

                    return View(notify);


                }
                else
                {
                    notify.RecieverRole = "no data";

                    return View(notify);
                }
            }
        }
    

        public ActionResult taskProgress(int NotificationID)
        {

            List<TaskModel> Noticafition = new List<TaskModel>();
            TaskModel notify = new TaskModel();
            Session["notificationID"] = NotificationID;
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            DataTable dataTable = new DataTable();
            OasisConnection.Open();

            var query = "select Fk_TaskID from Notifications where id='" + NotificationID + "'";

            SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);

            using (SqlDataReader rd = SelectCommand.ExecuteReader())
            {

                if (rd.Read())
                {

                    if (OasisConnection.State == ConnectionState.Open)
                    {
                        var queryTask= "select * from Tasks where id='" + Convert.ToInt32(rd["Fk_TaskID"]) + "'";
                        OasisConnection.Close();

                        SqlCommand SelectTaskCommand = new SqlCommand(queryTask, OasisConnection);
                        OasisConnection.Open();

                        SqlDataReader rdTask = SelectTaskCommand.ExecuteReader();

                        if(rdTask.Read())
                        {
                            notify.id = Convert.ToInt32(rdTask["id"]);

                            notify.TaskStatus = rdTask["TaskStatus"].ToString();
                            notify.Taskdetail = rdTask["Taskdetail"].ToString();
                            notify.dateAssigned = Convert.ToDateTime(rdTask["dateAssigned"]);
                            notify.DueDate = Convert.ToDateTime(rdTask["DueDate"]);

                            if (rdTask["Fk_EmployeeID"]!=null)
                            {
                                notify.Fk_EmployeeID = Convert.ToInt32(rdTask["Fk_EmployeeID"]);

                            }

                            if (OasisConnection.State==ConnectionState.Open)
                            {
                                OasisConnection.Close();

                            }
                        }

                    }
                   
                  
                }
            }
            return View(notify);
        }

       
        [HttpPost]
        public ActionResult Comments(string Comment)
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;
            OasisConnection = new SqlConnection(OasisConnectionManager);
            var query = "insert into Comments (Fk_commentor_ID,Fk_task_ID,comments,DateCommented,commenter) values(@Fk_commentor_ID,@Fk_task_ID,@comments,@DateCommented,@commenter)";
            SqlCommand insertcommand = new SqlCommand(query, OasisConnection);
            insertcommand.Parameters.AddWithValue("@Fk_commentor_ID", Convert.ToInt32(Session["id"]));
            insertcommand.Parameters.AddWithValue("@Fk_task_ID", Convert.ToInt32( Session["taskID"]));
            insertcommand.Parameters.AddWithValue("@comments", Comment);
            insertcommand.Parameters.AddWithValue("@DateCommented", DateTime.Now);
            insertcommand.Parameters.AddWithValue("@commenter", @Session["username"]);

            OasisConnection.Open();
            int results = insertcommand.ExecuteNonQuery();
            return RedirectToAction("taskProgress", new { notificationID = Convert.ToInt32(Session["notificationID"]) } );
        }


        public ActionResult Task()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Task(string TaskDetail)
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            var SelectGetQuery = "select * from GetTask where Fk_NoticationID='" + Convert.ToInt32(Session["notificationID"]) + "'";
            SqlCommand selecttcommand = new SqlCommand(SelectGetQuery, OasisConnection);

            OasisConnection.Open();

            SqlDataReader rdTask = selecttcommand.ExecuteReader();

            if(rdTask.Read())
            {
                var query = "insert into Tasks (TaskDetail,dateAssigned,DueDate,TaskStatus,Fk_GetTask) values(@TaskDetail,@dateAssigned,@DueDate,@TaskStatus,@Fk_GetTask)";
                SqlCommand insertcommand = new SqlCommand(query, OasisConnection);
                insertcommand.Parameters.AddWithValue("@TaskDetail", TaskDetail);
                insertcommand.Parameters.AddWithValue("@TaskStatus", "Accepted");
                insertcommand.Parameters.AddWithValue("@dateAssigned", DateTime.Now);
                insertcommand.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(7));
                insertcommand.Parameters.AddWithValue("@Fk_GetTask", Convert.ToInt32(rdTask["id"]));


                Session["taskID"] = Convert.ToInt32(rdTask["id"]);


                if (OasisConnection.State==ConnectionState.Open)
                {
                    OasisConnection.Close();
                    OasisConnection.Open();
                    int results = insertcommand.ExecuteNonQuery();


                    if (results > 0)
                    {

                        if (OasisConnection.State == ConnectionState.Open)
                        {
                            OasisConnection.Close();


                            var queryInsertNotification = "insert into Notifications (RecieverRole,Notification,datesent,Fk_senderID) values(@RecieverRole,@Notification,@datesent,@Fk_senderID)";
                            SqlCommand insertSendNotificationcommand = new SqlCommand(queryInsertNotification, OasisConnection);
                            insertSendNotificationcommand.Parameters.AddWithValue("@Fk_senderID", Convert.ToInt32(Session["id"]));
                            insertSendNotificationcommand.Parameters.AddWithValue("@RecieverRole", "Manager");
                            insertSendNotificationcommand.Parameters.AddWithValue("@Notification", Session["username"] + " Has Accepted a task");

                            insertSendNotificationcommand.Parameters.AddWithValue("@datesent", DateTime.Now);

                            OasisConnection.Open();

                            int resultsNoti = insertSendNotificationcommand.ExecuteNonQuery();

                            if (resultsNoti > 0)
                            {



                                if (OasisConnection.State == ConnectionState.Open)
                                {
                                    OasisConnection.Close();

                                                var queryInsertNotificationEmployee = "insert into Notifications (RecieverRole,Notification,datesent,Fk_senderID,Fk_TaskID) values(@RecieverRole,@Notification,@datesent,@Fk_senderID,@Fk_TaskID)";
                                                SqlCommand insertSendNotificationEmployeecommand = new SqlCommand(queryInsertNotificationEmployee, OasisConnection);
                                                insertSendNotificationEmployeecommand.Parameters.AddWithValue("@Fk_senderID", Convert.ToInt32(Session["id"]));
                                                insertSendNotificationEmployeecommand.Parameters.AddWithValue("@RecieverRole", "ProductionDepartment");
                                                insertSendNotificationEmployeecommand.Parameters.AddWithValue("@Notification", Session["username"] + " Has set a task for " + TaskDetail);
                                                insertSendNotificationEmployeecommand.Parameters.AddWithValue("@datesent", DateTime.Now);
                                                insertSendNotificationEmployeecommand.Parameters.AddWithValue("@Fk_TaskID", Convert.ToInt32(Session["taskID"]));

                                                OasisConnection.Open();

                                                int resultsNotiEmpl = insertSendNotificationEmployeecommand.ExecuteNonQuery();

                                                if (resultsNotiEmpl > 0)
                                                {
                                                    return RedirectToAction("index");

                                                }
                                                else
                                                {
                                                    MessageBox.Show("Notification not sent");
                                                    return RedirectToAction("index");

                                                }

                                            }


                                        }

                                    }

                                }
                                else
                                {
                                    MessageBox.Show("Notification not sent");
                                    return RedirectToAction("index");

                                }

                            }

                            return RedirectToAction("just");


                        }
                        else
                        {
                            if (OasisConnection.State == ConnectionState.Open)
                            {
                                OasisConnection.Close();
                                return RedirectToAction("just");

                            }

                            return RedirectToAction("just");


                        }


                    }
                   

        public ActionResult StartTask(int TaskID)
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;
            OasisConnection = new SqlConnection(OasisConnectionManager);
            Session["taskId"] = TaskID;
            var updateQuery = "Update Tasks SET TaskStatus='" + "On Progress" + " ," + " Fk_EmployeeID=" + Convert.ToInt32(Session["id"]) + "' where id='" + TaskID + " '";

            SqlCommand UpdateComand = new SqlCommand(updateQuery, OasisConnection);

                 OasisConnection.Open();

                UpdateComand.CommandType = CommandType.Text;
                UpdateComand.ExecuteNonQuery();
                OasisConnection.Close();
            if (OasisConnection.State == ConnectionState.Open)
            {
                OasisConnection.Close();

                var updateQ = "Update Notifications SET isActive=No where Fk_taskID='" + TaskID + "'";

                SqlCommand UpdateIsActive = new SqlCommand(updateQ, OasisConnection);
                OasisConnection.Open();

                UpdateIsActive.CommandType = CommandType.Text;
                UpdateIsActive.ExecuteNonQuery();
                OasisConnection.Close();

            }


            return RedirectToAction("taskProgress", new { notificationID = Convert.ToInt32(Session["taskId"]) });
        }

        [HttpPost]
        public ActionResult UpdateTask(String NewTask)
        {

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;
            OasisConnection = new SqlConnection(OasisConnectionManager);
           
            
                var updateQuery = "Update Tasks SET TaskStatus='" + NewTask + "' ,Fk_EmployeeID='" + Convert.ToInt32(Session["id"]) + "' where id='" + Convert.ToInt32(Session["taskId"]) + " '";

            SqlCommand UpdateComand = new SqlCommand(updateQuery, OasisConnection);

            OasisConnection.Open();

            UpdateComand.CommandType = CommandType.Text;
            UpdateComand.ExecuteNonQuery();
            OasisConnection.Close();

            return RedirectToAction("taskProgress", new { notificationID = Convert.ToInt32(Session["taskId"]) });
        }

        public ActionResult DeleteTask()
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;
            OasisConnection = new SqlConnection(OasisConnectionManager);

            var DeleteChildQ = "Delete from GetTask where Fk_NoticationID ='" + Convert.ToInt32(Session["notificationID"]) + "'";
            SqlCommand DeleteChildCommand = new SqlCommand(DeleteChildQ, OasisConnection);
            OasisConnection.Open();
            DeleteChildCommand.CommandType = CommandType.Text;
            DeleteChildCommand.ExecuteNonQuery();

            if (OasisConnection.State == ConnectionState.Open)
            {
                OasisConnection.Close();

            }

            var DeleteQ = "Delete from notifications where Id =' " + Convert.ToInt32(Session["notificationID"]) + "'";
            SqlCommand DeleteCommand = new SqlCommand(DeleteQ, OasisConnection);
            OasisConnection.Open();
            DeleteCommand.CommandType = CommandType.Text;
            DeleteCommand.ExecuteNonQuery();


            return RedirectToAction("Index");

        }
    }
}