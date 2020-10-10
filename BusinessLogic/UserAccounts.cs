using DataAccessLayer;
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



 public   class UserAccounts
    {

        List<Users> Users = new List<Users>();
        string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OASISDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False  ";
        SqlConnection oasisconnectionString;

        public Users Item;
        public string key = "";
        //my constructors
        public UserAccounts(Users Item, string key)

        {
            this.Item = Item;
            this.key = key;
        }

        public UserAccounts(string key)
        {
            this.key = key;
        }


        public UserAccounts()
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


                    query = "Select * from Users";
                    break;
                case "Delete":


                    query = "Delete from Product where id=" + id;
                    break;

                case "Update":

                    string FirsQ1 = createQuery(Item.GetType().Name.ToString(), key);

                    Type typeMethod1 = Item.GetType();

                    MethodInfo[] methodInfo1 = typeMethod1.GetMethods();

                    StringBuilder FinalQuery1 = new StringBuilder(FirsQ1);

                    StringBuilder ValuesQuery1 = new StringBuilder("");

                    StringBuilder ColomnQer1 = new StringBuilder("");


                    ColomnQer1.Append(


                         methodInfo1[2].Name.Replace("get_", "") + " = " + "'" + Item.FullNames + "'" +
                        " ," + methodInfo1[4].Name.Replace("get_", "") + " = " + "'" + Item.Email + "'"

                        +
                        " ," + methodInfo1[6].Name.Replace("get_", "") + " = " + "'" + Item.UserRole + "'"
                        +
                        " ," + methodInfo1[8].Name.Replace("get_", "") + " = " + "'" + Item.Password + "'"
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
        public string Register()
        {

            Type typeMethod = Item.GetType();

            MethodInfo[] methodInfo = typeMethod.GetMethods();

            switch (key)
            {
                case "Insert":


                    SqlCommand insertcommand = GetSqlCommand();

                    insertcommand.Parameters.AddWithValue("@" + methodInfo[2].Name.Replace("get_", ""), Item.FullNames);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[4].Name.Replace("get_", ""), Item.Email);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[6].Name.Replace("get_", ""), Item.UserRole);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[8].Name.Replace("get_", ""), Item.Password);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[10].Name.Replace("get_", ""), Item.isActive);

                    oasisconnectionString.Open();

                    int results = insertcommand.ExecuteNonQuery();

                    if (results > 0)
                    {

                        return " did it";
                    }
                    else
                    {

                        return "im sorry we cant";

                    }


            }
            oasisconnectionString.Close();

            return "";

        }

        //select all products in our database
        public List<Users> Login()
        {
            List<Users> ListUsers = new List<Users>();
            SqlCommand Selectcommand = GetSqlCommand();
            oasisconnectionString.Open();
            int counter = 0;

            using (SqlDataReader rd = Selectcommand.ExecuteReader())
            {
                while (rd.Read())
                {
                    Users user = new Users();

                    user.Email = rd["Email"].ToString();

                    user.isActive = rd["isActive"].ToString();
                    user.FullNames = rd["FullNames"].ToString();
                    user.Password = rd["Password"].ToString();
                    user.UserRole = rd["UserRole"].ToString();
                    user.id = Convert.ToInt32(rd["id"]);

                    ListUsers.Add(user);

                    counter += 1;
                }

            }

            oasisconnectionString.Close();

            return ListUsers;

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
