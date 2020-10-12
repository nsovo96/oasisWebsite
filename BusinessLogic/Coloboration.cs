using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
  public  class Coloboration
    {

        List<Comments> TaskM = new List<Comments>();
        string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OASISDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False  ";
        SqlConnection oasisconnectionString;

        public Comments Item;
        public string key = "";
        //my constructors
        public Coloboration(Comments Item, string key)

        {


            this.Item = Item;
            this.key = key;
        }

        public Coloboration(string key)
        {
            this.key = key;
        }


        public Coloboration()
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
                     query = "insert into Comments (Fk_commentor_ID,Fk_task_ID,comments,DateCommented) values(@Fk_commentor_ID,@Fk_task_ID,@comments,@DateCommented)";
                    break;

                case "Select":


                    query = "Select * from Comments where Fk_task_ID='" + id +  "'";
                    break;
                case "Delete":


                    query = "Delete from Comments where id=" + id;
                    break;

                case "Update":

                    string FirsQ1 = createQuery(Item.GetType().Name.ToString(), key);

                    Type typeMethod1 = Item.GetType();

                    MethodInfo[] methodInfo1 = typeMethod1.GetMethods();

                    StringBuilder FinalQuery1 = new StringBuilder(FirsQ1);

                    StringBuilder ValuesQuery1 = new StringBuilder("");

                    StringBuilder ColomnQer1 = new StringBuilder("");


                    ColomnQer1.Append(


                         methodInfo1[12].Name.Replace("get_", "") + " = " + "'" + Item.comments + "'" +
                        " ," + methodInfo1[4].Name.Replace("get_", "") + " = " + "'" + Item.Fk_commentor_ID + "'"

                        +
                        " ," + methodInfo1[6].Name.Replace("get_", "") + " = " + "'" + Item.Fk_task_ID + "'"
                        +
                        " ," + methodInfo1[8].Name.Replace("get_", "") + " = " + "'" + Item.DateCommented + "'"
                       
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
        public void SendCollab(int fk_id,int fk_taskid,string comment, string taskdetail)
        {


            SqlCommand insertcommand = GetSqlCommand(fk_taskid);


            insertcommand.Parameters.AddWithValue("@Comments", comment);
            insertcommand.Parameters.AddWithValue("@Fk_commentor_ID", fk_id);
            insertcommand.Parameters.AddWithValue("@DateCommented", DateTime.Now);
            insertcommand.Parameters.AddWithValue("@Fk_task_ID", fk_taskid);

            oasisconnectionString.Open();

            int results = insertcommand.ExecuteNonQuery();

            if (results > 0)
            {
                oasisconnectionString.Close();

                var query = "Select * from users where id='" + fk_id + "'";

                oasisconnectionString.Open();

                SqlCommand command = new SqlCommand(query, oasisconnectionString);
                NotificationsDisplay notifications = new NotificationsDisplay();

                using (SqlDataReader rd = command.ExecuteReader())
                {
                    if (rd.Read())
                    {

                       

                    }




                }
            }

            oasisconnectionString.Close();


        }

        //select all products in our database
        public List<Comments> GetComments(int TaskId)
        {
            List<Comments> ListTask = new List<Comments>();
            SqlCommand Selectcommand = GetSqlCommand(TaskId);
            oasisconnectionString.Open();
            int counter = 0;

            using (SqlDataReader rd = Selectcommand.ExecuteReader())
            {
                while (rd.Read())
                {
                    Comments tasksL = new Comments();

                    tasksL.comments = rd["comments"].ToString();

                    tasksL.DateCommented = Convert.ToDateTime(rd["DateCommented"]);

                    tasksL.Fk_commentor_ID = Convert.ToInt32(rd["Fk_commentor_ID"]);
                    tasksL.id = Convert.ToInt32(rd["id"]);
                    tasksL.Fk_task_ID = Convert.ToInt32(rd["Fk_task_ID"]);


                    ListTask.Add(tasksL);

                    counter += 1;
                }


            }

            oasisconnectionString.Close();

            return ListTask;

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

