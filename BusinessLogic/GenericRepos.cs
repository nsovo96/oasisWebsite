using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;
using DataAccessLayer.Models;

namespace BusinessLogic
{
    public class genericProduct
    {
        List<Product> products = new List<Product>();
        string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OASISDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False  ";
        SqlConnection oasisconnectionString;

        public Product Item;
        public string key = "";
        //my constructors
        public genericProduct(Product Item,string key)

        {
            this.Item = Item;
            this.key = key;
        }

        public genericProduct(string key)
        {
            this.key = key;
        }


        public genericProduct()
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
                case  "Update":

                    query.Append(" " + TableName + " Set ");
                    break;
               

            }

            return query.ToString();
        }

        //do a final addition to my query to best fit the command
        public string Finalizequery(int id=0)
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


                    query = "Select * from Product";
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


                             methodInfo1[2].Name.Replace("get_", "") + " = " + "'" + Item.ProductName + "'"+ 
                            " ," + methodInfo1[4].Name.Replace("get_", "") + " = " + "'" + Item.Quantity + "'"

                            +
                            " ," + methodInfo1[6].Name.Replace("get_", "") + " = " + "'" + Item.CurrentPrice + "'"
                            +
                            " ," + methodInfo1[8].Name.Replace("get_", "") + " = " + "'"+ Item.SalePrice +"'"
                            +
                            " ," + methodInfo1[10].Name.Replace("get_", "") + " = " + "'"+ Item.Description + "'"
                            +
                            " ," + methodInfo1[12].Name.Replace("get_", "") + " = " + "'" + Item.Image + "'"

                            );


                      string ValueStrin=  ColomnQer1.Replace("set_", "").ToString();


                    query = FinalQuery1.Append(" " +  ValueStrin + " where id=" + id).ToString(); ;


                    break;

            }



            return query;
        }

        //set an sql commmand to do the operations in our database
        public SqlCommand GetSqlCommand(int id=0)
        {
           // var updateQuery = "Update Tasks SET TaskStatus=Accepted ,Fk_EmployeeID='" + Convert.ToInt32(Session["id"]) + "' where id='" + Convert.ToInt32(Session["taskId"]) + " '";

            string query = Finalizequery(id);

            oasisconnectionString = new SqlConnection(ConnectionStr);

            SqlCommand command = new SqlCommand(query, oasisconnectionString);


            return command;
        }

        
        //insert to database command
        public string Insert()
        {

            Type typeMethod = Item.GetType();

            MethodInfo[] methodInfo = typeMethod.GetMethods();








            switch (key)
            {
                case "Insert":


                    SqlCommand insertcommand = GetSqlCommand();

                    insertcommand.Parameters.AddWithValue("@" + methodInfo[2].Name.Replace("get_", ""), Item.ProductName);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[4].Name.Replace("get_", ""), Item.Quantity);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[6].Name.Replace("get_", ""), Item.CurrentPrice);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[8].Name.Replace("get_", ""), Item.SalePrice);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[10].Name.Replace("get_", ""), Item.Description);
                    insertcommand.Parameters.AddWithValue("@" + methodInfo[12].Name.Replace("get_", ""), Item.Image);

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
        public  List<Product> SelectAll()
        {
            List<Product> listProduct= new List<Product>();
            SqlCommand Selectcommand = GetSqlCommand();
            oasisconnectionString.Open();
            int counter = 0;


            using (SqlDataReader rd = Selectcommand.ExecuteReader())
            {
                while (rd.Read())
                {
                    Product Product = new Product();

                    Product.ProductName = rd["ProductName"].ToString();

                    Product.Quantity =Convert.ToInt32( rd["Quantity"]);
                    Product.CurrentPrice = Convert.ToDecimal( rd["CurrentPrice"]);
                    Product.SalePrice = Convert.ToDecimal(rd["SalePrice"]);
                    Product.Description = rd["Description"].ToString();
                    Product.Image = rd["Image"].ToString();
                    Product.id = Convert.ToInt32(rd["id"]);

                    listProduct.Add(Product);

                    counter += 1;
                }

            }

            oasisconnectionString.Close();

            return listProduct;

        }

        public void Delete (int ProductID)
        {
            List<Product> listProduct = new List<Product>();
            SqlCommand Deletecommand = GetSqlCommand(ProductID);
            oasisconnectionString.Open();
            // int counter = 0;

            Deletecommand.CommandType = CommandType.Text;

            Deletecommand.ExecuteNonQuery();
            oasisconnectionString.Close();

        }


        public void edit (int productID)
        {



            List<Product> listProduct = new List<Product>();
            SqlCommand Updatecommand = GetSqlCommand(productID);
            oasisconnectionString.Open();
            // int counter = 0;

            Updatecommand.CommandType = CommandType.Text;

            Updatecommand.ExecuteNonQuery();
            oasisconnectionString.Close();



        }

        public void UpdateQuantity(int id, int quantity)
        {

            string query = "Update Product Set Quantity ='" + quantity + "' where id='" +id + "'";



            oasisconnectionString = new SqlConnection(ConnectionStr);

            SqlCommand command = new SqlCommand(query, oasisconnectionString);

            oasisconnectionString.Open();
            // int counter = 0;

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
            oasisconnectionString.Close();
        }

    }
 }