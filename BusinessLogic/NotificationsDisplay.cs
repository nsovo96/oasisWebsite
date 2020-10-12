using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusinessLogic
{
   public class NotificationsDisplay
    {
        List<Notifications> notify = new List<Notifications>();
        string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OASISDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False  ";
        SqlConnection oasisconnectionString;

        public Notifications Item;
        public string key = "";
        //my constructors
        public NotificationsDisplay(Notifications Item, string key)

        {
            this.Item = Item;
            this.key = key;
        }

        public NotificationsDisplay(string key)
        {
            this.key = key;
        }


        public NotificationsDisplay()
        {
        }

        //create a function that generate a query for our sql connections
        public string createQuery(string TableName, string sqlcom)
        {
            StringBuilder query = new StringBuilder(sqlcom);

            switch (sqlcom)
            {
                case "Insert":
                    query.Append(" into " + TableName);
                    break;
                case "Select":
                    query.Append(" * from " + TableName);
                    break;
                case "Update":

                    query.Append(" " + TableName + " Set ");
                    break;


            }

            return query.ToString();
        }

        //do a final addition to my query to best fit the command
        public string Finalizequery(int id = 0)
        {

            string query = "";

            switch (key)
            {
                case "Insert":
                    string FirsQ = createQuery(Item.GetType().Name.ToString(), key);

                    Type typeMethod = Item.GetType();

                    MethodInfo[] methodInfo = typeMethod.GetMethods();

                    StringBuilder FinalQuery = new StringBuilder(FirsQ);

                    StringBuilder ValuesQuery = new StringBuilder("(");

                    StringBuilder ColomnQer = new StringBuilder(" ( ");


                    for (int i = 3; i < methodInfo.Count() - 4; i = i + 2)
                    {
                        ColomnQer.Append("@" + methodInfo[i].Name.Replace("get_", "") + ",");
                        ColomnQer.Replace("set_", "");
                    }


                    string dirtyQ = ColomnQer.Remove(ColomnQer.Length - 1, 1).ToString();
                    ColomnQer.Append(")");
                    string CeanQ = ColomnQer.Replace("@", "").ToString();
                    query = FinalQuery.Append(" " + CeanQ + " values " + dirtyQ).ToString() + ")";


                    break;

                case "Select":


                    query = "Select * from Notifications";
                    break;
                case "Delete":


                    query = "Delete from Notifications where id=" + id;
                    break;

                case "Update":

                    string FirsQ1 = createQuery(Item.GetType().Name.ToString(), key);

                    Type typeMethod1 = Item.GetType();

                    MethodInfo[] methodInfo1 = typeMethod1.GetMethods();

                    StringBuilder FinalQuery1 = new StringBuilder(FirsQ1);

                    StringBuilder ValuesQuery1 = new StringBuilder("");

                    StringBuilder ColomnQer1 = new StringBuilder("");


                    ColomnQer1.Append(


                         methodInfo1[2].Name.Replace("get_", "") + " = " + "'" + Item.RecieverRole+ "'" +
                        " ," + methodInfo1[4].Name.Replace("get_", "") + " = " + "'" + Item.Notification + "'"

                        +
                        " ," + methodInfo1[6].Name.Replace("get_", "") + " = " + "'" + Item.datesent + "'"
                        +
                        " ," + methodInfo1[8].Name.Replace("get_", "") + " = " + "'" + Item.Fk_TaskID + "'"
                        +
                        " ," + methodInfo1[10].Name.Replace("get_", "") + " = " + "'" + Item.isActive + "'"

                        );


                    string ValueStrin = ColomnQer1.Replace("set_", "").ToString();


                    query = FinalQuery1.Append(" " + ValueStrin + " where id=" + id).ToString(); ;


                    break;

            }



            return query;
        }

        //set an sql commmand to do the operations in our database
        public SqlCommand GetSqlCommand(int id = 0)
        {
            // var updateQuery = "Update Tasks SET TaskStatus=Accepted ,Fk_EmployeeID='" + Convert.ToInt32(Session["id"]) + "' where id='" + Convert.ToInt32(Session["taskId"]) + " '";

            string query = Finalizequery(id);

            oasisconnectionString = new SqlConnection(ConnectionStr);

            SqlCommand command = new SqlCommand(query, oasisconnectionString);


            return command;
        }


        //insert to database command
        public string CreateAnotification(string NotificationDetail, string recieverRole)
        {



            var queryInsertNotification = "insert into Notifications (RecieverRole,Notification,datesent) values(@RecieverRole,@Notification,@datesent)";

            SqlConnection sqlConnection = new SqlConnection(ConnectionStr);

            SqlCommand insertcommand = new SqlCommand(queryInsertNotification, sqlConnection);

                    insertcommand.Parameters.AddWithValue("@RecieverRole", recieverRole);
                    insertcommand.Parameters.AddWithValue("@Notification" , NotificationDetail);
                    insertcommand.Parameters.AddWithValue("@datesent"  , DateTime.Now.ToLocalTime());

                    sqlConnection.Open();

                    int results = insertcommand.ExecuteNonQuery();

                    if (results > 0)
                    {
                sqlConnection.Close();

                return " did it";

            }
            else
                    {
                sqlConnection.Close();

                return "im sorry we cant";

                    }

        }
           

        

        //select all products in our database
        public List<Notifications> dispalayNotifications ()
        {
            List<Notifications> ListNotification = new List<Notifications>();
            SqlCommand Selectcommand = GetSqlCommand();
            oasisconnectionString.Open();
            int counter = 0;

            using (SqlDataReader rd = Selectcommand.ExecuteReader())
            {
                while (rd.Read())
                {
                    Notifications noticat = new Notifications();

                    noticat.RecieverRole = rd["RecieverRole"].ToString();

                    noticat.isActive = rd["isActive"].ToString();
                    noticat.Notification = rd["Notification"].ToString();
                    noticat.datesent = Convert.ToDateTime(rd["datesent"]);

                    noticat.id = Convert.ToInt32(rd["id"]);

                    ListNotification.Add(noticat);

                    counter += 1;
                }

            }

            oasisconnectionString.Close();

            return ListNotification;

        }

        public void Delete(int UserID)
        {
            SqlCommand Deletecommand = GetSqlCommand(UserID);
            oasisconnectionString.Open();
            // int counter = 0;

            Deletecommand.CommandType = CommandType.Text;

            Deletecommand.ExecuteNonQuery();
            oasisconnectionString.Close();

        }

        public Notifications GetNotificationsID(int id)
        {
            SqlCommand Selectcommand = GetSqlCommand();
            oasisconnectionString.Open();
            Notifications notifications = new Notifications();

            using (SqlDataReader rd = Selectcommand.ExecuteReader())
            {
                while(rd.Read())
                {
                    if(Convert.ToInt32(rd["id"])==id)
                    {
                        notifications.RecieverRole = rd["RecieverRole"].ToString();

                        notifications.isActive = rd["isActive"].ToString();
                        notifications.Notification = rd["Notification"].ToString();
                        notifications.datesent = Convert.ToDateTime(rd["datesent"]);

                        notifications.id = Convert.ToInt32(rd["id"]);

                    }


                }

            }

            oasisconnectionString.Close();


            return notifications;

        }
        public void Update(int UserID)
        {

            SqlCommand Updatecommand = GetSqlCommand(UserID);
            oasisconnectionString.Open();
            // int counter = 0;

            Updatecommand.CommandType = CommandType.Text;

            Updatecommand.ExecuteNonQuery();
            oasisconnectionString.Close();
        }
    }
}
