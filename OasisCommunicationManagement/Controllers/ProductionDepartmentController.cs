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
    public class ProductionDepartmentController : Controller
    {
        // GET: ProductionDepartment
        private string OasisConnectionManager;
        private SqlConnection OasisConnection;
        public ActionResult Inbox()
        {

            List<MessagesModel> inbox = new List<MessagesModel>();

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            DataTable dataTable = new DataTable();
            OasisConnection.Open();

            var query = "select * from messages where FK_RecieverID=" + Convert.ToInt32(Session["id"]);

            SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);

            using (SqlDataReader rd = SelectCommand.ExecuteReader())
            {
                Session["messages"] = new List<MessagesModel>(inbox);

                List<MessagesModel> msg = (List<MessagesModel>)Session["cart"];

                while (rd.Read())
                {
                    MessagesModel message = new MessagesModel();

                    message.SenderID = Convert.ToInt32(rd["SenderID"]);
                    message.REcieverID = Convert.ToInt32(rd["FK_RecieverID"]);
                    message.Attancement = rd["Attancement"].ToString();
                    message.messages = rd["Message"].ToString();
                    message.DateSent = Convert.ToDateTime(rd["DateSent"]);
                    Session["FK_RecieverID"] = rd["FK_RecieverID"];
                    Session["senderid"] = rd["SenderID"];
                    inbox.Add(message);
                }

            }
            return View(inbox);


        }


        public ActionResult Chat(int RecieverID)
        {
            Session["senderId"] = RecieverID;
            List<MessagesModel> inbox = new List<MessagesModel>();

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            DataTable dataTable = new DataTable();
            OasisConnection.Open();

            var updateQuery = "Update messages SET MsgStatus='" + "Read" + "'";

            var query = "select * from messages ";

            SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);


            using (SqlDataReader rd = SelectCommand.ExecuteReader())
            {



                while (rd.Read())
                {

                    if (Convert.ToInt32(rd["SenderID"]) == RecieverID)
                    {
                        MessagesModel message = new MessagesModel();
                        message.SenderID = Convert.ToInt32(rd["SenderID"]);
                        message.REcieverID = Convert.ToInt32(rd["FK_RecieverID"]);
                        Session["FK_RecieverID"] = Convert.ToInt32(rd["SenderID"]);

                        message.Attancement = rd["Attancement"].ToString();
                        message.messages = rd["Message"].ToString();
                        message.DateSent = Convert.ToDateTime(rd["DateSent"]);
                        inbox.Add(message);



                    }
                    else if (Convert.ToInt32(rd["SenderID"]) == Convert.ToInt32(Session["id"]))
                    {

                        MessagesModel message = new MessagesModel();
                        message.SenderID = Convert.ToInt32(rd["SenderID"]);
                        message.REcieverID = Convert.ToInt32(rd["FK_RecieverID"]);
                        Session["FK_RecieverID"] = Convert.ToInt32(rd["SenderID"]);
                        message.Attancement = rd["Attancement"].ToString();
                        message.messages = rd["Message"].ToString();
                        message.DateSent = Convert.ToDateTime(rd["DateSent"]);
                        inbox.Add(message);

                    }
                }

            }

            OasisConnection.Close();
            SqlCommand UpdateComand = new SqlCommand(updateQuery, OasisConnection);

            if (OasisConnection.State == ConnectionState.Closed)
            {
                OasisConnection.Open();

                UpdateComand.CommandType = CommandType.Text;
                UpdateComand.ExecuteNonQuery();
                OasisConnection.Close();

            }




            return View(inbox);
        }

        [HttpPost]
        public ActionResult SendMessage(String message)
        {

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            var query = "insert into Messages (SenderID,FK_RecieverID,Attancement,message,DateSent,MsgStatus) values(@SenderID,@FK_RecieverID,@Attancement,@message,@DateSent,@MsgStatus)";
            SqlCommand insertcommand = new SqlCommand(query, OasisConnection);
            insertcommand.Parameters.AddWithValue("@SenderID", Convert.ToInt32(Session["id"]));
            insertcommand.Parameters.AddWithValue("@FK_RecieverID", Convert.ToInt32(Session["FK_RecieverID"]));
            insertcommand.Parameters.AddWithValue("@Attancement", "llll");
            insertcommand.Parameters.AddWithValue("@message", message);
            insertcommand.Parameters.AddWithValue("@MsgStatus", "Sent");


            insertcommand.Parameters.AddWithValue("@DateSent", DateTime.Now);
            OasisConnection.Open();

            int results = insertcommand.ExecuteNonQuery();

            if (results > 0)
            {
                return RedirectToAction("chat", new { RecieverID = Convert.ToInt32(Session["senderId"]) });
            }
            else
            {
                return RedirectToAction("chat", new { RecieverID = Convert.ToInt32(Session["senderId"]) });

            }
        }
        public ActionResult just()
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

                var queryNotification = "select * from Notifications where RecieverRole='" + "ProductionDepartment" + "'";

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


        public ActionResult Notifications()
        {
            List<notificationModel> Noticafition = new List<notificationModel>();

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            DataTable dataTable = new DataTable();
            OasisConnection.Open();

            var query = "select * from Notifications where RecieverRole='" + "ProductionDepartment" + "'";

            SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);

            using (SqlDataReader rd = SelectCommand.ExecuteReader())
            {

                while (rd.Read())
                {
                    notificationModel notify = new notificationModel();

                    notify.Fk_senderID = Convert.ToInt32(rd["Fk_SenderID"]);
                    notify.RecieverRole = rd["RecieverRole"].ToString();
                    notify.Notification = rd["Notification"].ToString();
                    notify.datesent = Convert.ToDateTime(rd["datesent"]);
                    Noticafition.Add(notify);
                }

            }
            return View(Noticafition);
        }


        public ActionResult task(int taskID)
        {
            List<notificationModel> Noticafition = new List<notificationModel>();
            TaskModel notify = new TaskModel();
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);
            Session["taskId"] = taskID;
            DataTable dataTable = new DataTable();
            OasisConnection.Open();

            var query = "select * from Tasks where id='" + taskID + "'";

            SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);

            using (SqlDataReader rd = SelectCommand.ExecuteReader())
            {

                if (rd.Read())
                {
                    notify.Taskdetail = rd["TaskDetail"].ToString();
                    notify.DueDate = Convert.ToDateTime(rd["Duedate"]);
                }

            }
            return View(notify);
        }

        [HttpPost]
        public ActionResult Task(String TaskDetail)
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

            var updateQuery = "Update Tasks SET TaskStatus='Accepted' ,Fk_EmployeeID='" + Convert.ToInt32(Session["id"]) + "' where id='" + Convert.ToInt32(Session["taskId"]) + " '";

            SqlCommand UpdateComand = new SqlCommand(updateQuery, OasisConnection);

            OasisConnection.Open();

            UpdateComand.CommandType = CommandType.Text;
            UpdateComand.ExecuteNonQuery();

            if (OasisConnection.State == ConnectionState.Open)
            {
                OasisConnection.Close();


                var queryInsertNotification = "insert into Notifications (RecieverRole,Notification,datesent,Fk_taskID) values(@RecieverRole,@Notification,@datesent,@Fk_taskID)";
                SqlCommand insertSendNotificationcommand = new SqlCommand(queryInsertNotification, OasisConnection);


                insertSendNotificationcommand.Parameters.AddWithValue("@Fk_taskID", Convert.ToInt32(Session["taskId"]));
                insertSendNotificationcommand.Parameters.AddWithValue("@RecieverRole", "Manager");
                insertSendNotificationcommand.Parameters.AddWithValue("@Notification", Session["username"] + " Has Accepted a task and have asked for " + TaskDetail);
                insertSendNotificationcommand.Parameters.AddWithValue("@datesent", DateTime.Now);

                OasisConnection.Open();

                int resultsNoti = insertSendNotificationcommand.ExecuteNonQuery();

                if (resultsNoti > 0)
                {
                
                                if (OasisConnection.State == ConnectionState.Open)
                                {

                                    var queryInsertNotificationEmployee = "insert into Notifications (RecieverRole,Notification,datesent,Fk_TaskID) values(@RecieverRole,@Notification,@datesent,@Fk_TaskID)";
                                    SqlCommand insertSendNotificationEmployeecommand = new SqlCommand(queryInsertNotificationEmployee, OasisConnection);
                                    insertSendNotificationEmployeecommand.Parameters.AddWithValue("@RecieverRole", "ProductionEmployee");
                                    insertSendNotificationEmployeecommand.Parameters.AddWithValue("@Notification", Session["username"] + " Has set a task for " + TaskDetail);
                                    insertSendNotificationEmployeecommand.Parameters.AddWithValue("@datesent", DateTime.Now);
                                    insertSendNotificationEmployeecommand.Parameters.AddWithValue("@Fk_TaskID", Convert.ToInt32(Session["Fk_taskID"]));
                                    OasisConnection.Close();
                                    OasisConnection.Open();

                                    int resultsNotiEmpl = insertSendNotificationEmployeecommand.ExecuteNonQuery();
                                }


                            }
                return RedirectToAction("just");

            }else
            {
                return RedirectToAction("Shared", "error");
            }
        }


        public ActionResult ProductionTaskProgress(int NotificationID)
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
                    string TakID = rd["Fk_TaskID"].ToString();

                    var queryTask = "select * from Tasks where id='" + Convert.ToInt32(rd["Fk_TaskID"]) + "'";

                    if (OasisConnection.State == ConnectionState.Open)
                    {
                        
                              OasisConnection.Close();

                    

                            SqlCommand SelectTaskCommand = new SqlCommand(queryTask, OasisConnection);
                            OasisConnection.Open();

                            SqlDataReader rdTask = SelectTaskCommand.ExecuteReader();

                            if (rdTask.Read())
                            {
                            string emplID = rdTask["TaskStatus"].ToString();


                            if (emplID != "Sent")
                            {
                                notify.id = Convert.ToInt32(rdTask["id"]);
                                notify.TaskStatus = rdTask["TaskStatus"].ToString();
                                notify.Taskdetail = rdTask["Taskdetail"].ToString();
                                notify.dateAssigned = Convert.ToDateTime(rdTask["dateAssigned"]);
                                notify.DueDate = Convert.ToDateTime(rdTask["DueDate"]);
                             
                                notify.Fk_EmployeeID = Convert.ToInt32(rdTask["Fk_EmployeeID"]);

                            }
                            else
                            {
                                return RedirectToAction("NotificationDetailProduction", new { NotificationID = NotificationID });

                            }

                            if (OasisConnection.State == ConnectionState.Open)
                                {
                                    OasisConnection.Close();

                                }
                            
                            

                        }
                        


                    }
                }
                

            }
            return View(notify);
        }

        public ActionResult StartProductionTask(int TaskID)
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

            return RedirectToAction("ProductionTaskProgress", new { notificationID = Convert.ToInt32(Session["taskId"]) });
        }

        public ActionResult NotificationDetailProduction(int NotificationID)
        {

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
                        notify.Fk_TaskID= Convert.ToInt32(rd["Fk_TaskID"]);
                        notify.RecieverRole = rd["RecieverRole"].ToString();
                        notify.Notification = rd["Notification"].ToString();
                        notify.datesent = Convert.ToDateTime(rd["datesent"]);

                        OasisConnection.Close();

                        return View(notify);


                    }else
                    {
                        notify.RecieverRole = "no data";

                        return View(notify);
                    }
                }
             
            }
        }

        public ActionResult StartTaskProduction(int TaskID)
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;
            OasisConnection = new SqlConnection(OasisConnectionManager);

            string status = "On Progress";
            Session["taskId"] = TaskID;
            var updateQuery = "Update Tasks SET TaskStatus='" + status + "' , Fk_EmployeeID='" + Convert.ToInt32(Session["id"]) + "' where id='" + TaskID + " '";

            SqlCommand UpdateComand = new SqlCommand(updateQuery, OasisConnection);

            OasisConnection.Open();

            UpdateComand.CommandType = CommandType.Text;
            UpdateComand.ExecuteNonQuery();
            OasisConnection.Close();

            if(OasisConnection.State==ConnectionState.Open)
            {
                OasisConnection.Close();

                var updateQ = "Update Notifications SET isActive=No where Fk_taskID='" + TaskID + "'";

                SqlCommand UpdateIsActive = new SqlCommand(updateQ, OasisConnection);
                OasisConnection.Open();

                UpdateIsActive.CommandType = CommandType.Text;
                UpdateIsActive.ExecuteNonQuery();
                OasisConnection.Close();

            }
            return RedirectToAction("ProductionTaskProgress", new { notificationID = Convert.ToInt32( Session["notificationID"]) });
        }

        [HttpPost]
        public ActionResult UpdateTaskProduction(String NewTask)
        {

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;
            OasisConnection = new SqlConnection(OasisConnectionManager);


            var updateQuery = "Update Tasks SET TaskStatus='" + NewTask + "' ,Fk_EmployeeID='" + Convert.ToInt32(Session["id"]) + "' where id='" + Convert.ToInt32(Session["taskId"]) + " '";

            SqlCommand UpdateComand = new SqlCommand(updateQuery, OasisConnection);

            OasisConnection.Open();

            UpdateComand.CommandType = CommandType.Text;
            UpdateComand.ExecuteNonQuery();
            OasisConnection.Close();

            return RedirectToAction("ProductionTaskProgress", new { notificationID = Convert.ToInt32(Session["notificationID"]) });
        }

        public ActionResult DeleteTaskProduction()
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

        [HttpPost]
        public ActionResult Comments(string Comment)
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;
            OasisConnection = new SqlConnection(OasisConnectionManager);
            var query = "insert into Comments (Fk_commentor_ID,Fk_task_ID,comments,DateCommented,commenter) values(@Fk_commentor_ID,@Fk_task_ID,@comments,@DateCommented,@commenter)";
            SqlCommand insertcommand = new SqlCommand(query, OasisConnection);
            insertcommand.Parameters.AddWithValue("@Fk_commentor_ID", Convert.ToInt32(Session["id"]));
            insertcommand.Parameters.AddWithValue("@Fk_task_ID", Convert.ToInt32(Session["taskID"]));
            insertcommand.Parameters.AddWithValue("@comments", Comment);
            insertcommand.Parameters.AddWithValue("@DateCommented", DateTime.Now);
            insertcommand.Parameters.AddWithValue("@commenter", @Session["username"]);

            OasisConnection.Open();
            int results = insertcommand.ExecuteNonQuery();
            return RedirectToAction("ProductionTaskProgress", new { notificationID = Convert.ToInt32(Session["notificationID"]) });
        }
    }

    
}