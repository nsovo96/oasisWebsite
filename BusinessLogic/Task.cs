using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
  public  class Task
    {

        List<Tasks> TaskM = new List<Tasks>();
        string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OASISDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False  ";
        SqlConnection oasisconnectionString;

        public Tasks Item;
        public string key = "";
        //my constructors
        public Task(Tasks Item, string key)

        {
            

            this.Item = Item;
            this.key = key;
        }

        public Task(string key)
        {
            this.key = key;
        }


        public Task()
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


                    query = "Select * from Tasks";
                    break;
                case "Delete":


                    query = "Delete from Tasks where id=" + id;
                    break;

                case "Update":

                    query = "Update Tasks SET TaskStatus='" + " Completed" + " ," + " DateFinished=" + DateTime.Now + "' where id='" + id + " '";

                   


                    break;

                      case "Insert_no_model":
                     query = "insert into Tasks (TaskDetail,dateAssigned,DueDate,TaskStatus,TaskType,Fk_EmployeeID) values(@TaskDetail,@dateAssigned,@DueDate,@TaskStatus,@TaskType,@Fk_EmployeeID)";

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
        public string NewTask(int fk_id,string typeofTask, string taskdetail = "")
        {

                    SqlCommand insertcommand = GetSqlCommand();
               

             if(taskdetail=="")
            {
                insertcommand.Parameters.AddWithValue("@TaskDetail", Item.Taskdetail);
                insertcommand.Parameters.AddWithValue("@dateAssigned", DateTime.Now);




                insertcommand.Parameters.AddWithValue("@DateFinished", DateTime.Now.AddDays(20));
                insertcommand.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(7));
                insertcommand.Parameters.AddWithValue("@TaskStatus", "Started");
                insertcommand.Parameters.AddWithValue("@TaskType", typeofTask);

                insertcommand.Parameters.AddWithValue("@Fk_EmployeeID", fk_id);
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

                            if (rd["UserRole"].ToString() == "Manager")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + Item.Taskdetail, "Employees");
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + Item.Taskdetail, "Boss");


                            }
                            else if (rd["UserRole"].ToString() == "Boss")
                            {

                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + Item.Taskdetail, "Manager");


                            }
                            else if (rd["UserRole"].ToString() == "ProccessAreaEmployee")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + Item.Taskdetail, "Manager");

                            }
                            else if (rd["UserRole"].ToString() == "FrontEndEmployee")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + Item.Taskdetail, "Manager");

                            }
                            else if (rd["UserRole"].ToString() == "StorageAreaEmployee")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + Item.Taskdetail, "Manager");

                            }
                        }




                    }
                }

            }
            else
            {
                insertcommand.Parameters.AddWithValue("@TaskDetail", taskdetail);
                insertcommand.Parameters.AddWithValue("@dateAssigned", DateTime.Now);
                insertcommand.Parameters.AddWithValue("@DateFinished", DateTime.Now.AddDays(20));
                insertcommand.Parameters.AddWithValue("@DueDate", DateTime.Now.AddDays(7));
                insertcommand.Parameters.AddWithValue("@TaskStatus", "Started");
                insertcommand.Parameters.AddWithValue("@TaskType", typeofTask);
                insertcommand.Parameters.AddWithValue("@Fk_EmployeeID", fk_id);


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

                            if (rd["UserRole"].ToString() == "Manager")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + taskdetail, "Employees");
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + taskdetail, "Boss");


                            }
                            else if (rd["UserRole"].ToString() == "Boss")
                            {

                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + taskdetail, "Manager");


                            }
                            else if (rd["UserRole"].ToString() == "ProccessAreaEmployee")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + taskdetail, "Manager");

                            }
                            else if (rd["UserRole"].ToString() == "FrontEndEmployee")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + taskdetail, "Manager");

                            }
                            else if (rd["UserRole"].ToString() == "StorageAreaEmployee")
                            {
                                notifications.CreateAnotification(rd["fullNames"].ToString() + " has started a task for " + taskdetail, "Manager");

                            }
                        }




                    }
                }
            }

                   

                   

            oasisconnectionString.Close();

            return "";

        }

        //select all products in our database
        public List<Tasks> GetTasks()
        {
            List<Tasks> ListTask = new List<Tasks>();
            SqlCommand Selectcommand = GetSqlCommand();
            oasisconnectionString.Open();
            int counter = 0;

            using (SqlDataReader rd = Selectcommand.ExecuteReader())
            {
                while (rd.Read())
                {
                    Tasks tasksL = new Tasks();

                    tasksL.Taskdetail = rd["Taskdetail"].ToString();

                    tasksL.dateAssigned =Convert.ToDateTime( rd["dateAssigned"]);

                        string date = rd["DateFinished"].ToString();


                    if(date!="")
                    {
                        tasksL.DueDate = Convert.ToDateTime(rd["DueDate"]);

                    }


                    tasksL.TaskStatus = rd["TaskStatus"].ToString();
                    tasksL.id = Convert.ToInt32(rd["id"]);

                    tasksL.Fk_EmployeeID = Convert.ToInt32(rd["Fk_EmployeeID"]);
                    tasksL.TaskType = rd["TaskType"].ToString();

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

