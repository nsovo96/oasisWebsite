using OasisCommunicationManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OasisCommunicationManagement.Controllers
{
    public class ManagerController : Controller
    {

        public int hrNum = 0;
        public int PrNum = 0;
        public int InNum = 0;
        public int MNum = 0;
        public int ProdNum = 0;
        // GET: ProductionDepartment
        private string OasisConnectionManager;
        private SqlDataAdapter OasisAdapter;
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

                var queryNotification = "select * from Notifications where RecieverRole='" + "Manager" + "'";

                SqlCommand SelectNotificatoinCommand = new SqlCommand(queryNotification, OasisConnection);

                using (SqlDataReader rd = SelectNotificatoinCommand.ExecuteReader())
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

            }

            return View(Noticafition);
        }
        public ActionResult Inbox()
        {

            List<MessagesModel> inbox = new List<MessagesModel>();

            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);

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
            return View();
        }




















        //public ActionResult just()
        //{
        //    List<MessagesModel> notification = new List<MessagesModel>();

        //    OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

        //    OasisConnection = new SqlConnection(OasisConnectionManager);

        //    OasisConnection.Open();

        //    var query = "select * from notications where RecieverRole='" + Session["role"].ToString() + "'";
        //    SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);
        //    using (SqlDataReader rd = SelectCommand.ExecuteReader())
        //    {
        //        while (rd.Read())
        //        {
        //            MessagesModel message = new MessagesModel();
        //            message.SenderID = Convert.ToInt32(rd["SenderID"]);
        //            message.REcieverID = Convert.ToInt32(rd["FK_RecieverID"]);
        //            Session["senderId"] = Convert.ToInt32(rd["SenderID"]);
        //            message.Attancement = rd["Attancement"].ToString();
        //            message.messages = rd["Message"].ToString();
        //            message.DateSent = Convert.ToDateTime(rd["DateSent"]);
        //            notification.Add(message);
        //        }
        //    }




        //                return View();
        //}






        public ActionResult GetData()
        {

            //// List<Product> products = new List<Product>();

            // OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            // OasisConnection = new SqlConnection(OasisConnectionManager);

            // OasisConnection.Open();

            // var query = "select * from product ";
            // SqlCommand SelectCommand = new SqlCommand(query, OasisConnection);
            // using (SqlDataReader rd = SelectCommand.ExecuteReader())
            // {
            //     while (rd.Read())
            //     {
            //     //    Product pr = new Product();

            //         pr.Quantity = Convert.ToInt32(rd["Quantity"]);
            //         pr.ProductName = rd["ProductName"].ToString();
            //         pr.CurrentPrice =Convert.ToInt32( rd["CurrentPrice"]);
            //         products.Add(pr);
            //         Session["reports"] = products;

            //     }
            // }

            // return Json(products, JsonRequestBehavior.AllowGet);

            return View();
        }

        public class Ratio

        {

            public int Hrmanager { get; set; }
            public int Prmanager { get; set; }
            public int invmanager { get; set; }
            public int PrEmployee { get; set; }
            public int manager { get; set; }




        }

        public ActionResult OasisReports()
        {
            return View();

        }

        public ActionResult ProductReports()
        {
            OasisConnectionManager = ConfigurationManager.ConnectionStrings["OasisConnectionString"].ConnectionString;

            OasisConnection = new SqlConnection(OasisConnectionManager);




            var query = "insert into Notifications (RecieverRole,Notification,datesent,Fk_senderID) values(@RecieverRole,@Notification,@datesent,@Fk_senderID)";
            SqlCommand insertcommand = new SqlCommand(query, OasisConnection);
            insertcommand.Parameters.AddWithValue("@Fk_senderID", Convert.ToInt32(Session["id"]));
            insertcommand.Parameters.AddWithValue("@RecieverRole", "ProductionDepartment");
            insertcommand.Parameters.AddWithValue("@Notification", "Manager" + Session["username"] + "Has Sent a sent notification about products" );
            insertcommand.Parameters.AddWithValue("@datesent", DateTime.Now);
            OasisConnection.Open();

            int results = insertcommand.ExecuteNonQuery();

            if (results > 0)
            {
                return RedirectToAction("Index");

            }
            return View("g");

        }


        public ActionResult notifications()
        {


            return View();

        }

    }
}