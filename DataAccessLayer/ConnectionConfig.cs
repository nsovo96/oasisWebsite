using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ConnectionConfig<T>
    {
        string ConnectionStr = "";
        public ConnectionState OasisConnection()
        {
            ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OasisDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection oasisconnectionString = new SqlConnection(ConnectionStr);
            oasisconnectionString.Open();
            return oasisconnectionString.State;
        }


       


        public void updateQuery(Stack<T> stack)
        {
         
            
            

        }



        public string saveToDatabase()
        {



            return "success";

        }





        public void saveChanges(T ModelTypeValues, string sqlcom, string selectCommnad = "*")
        {













            T table = ModelTypeValues;
            List<T> tableData = new List<T>();
            StringBuilder builderCols = new StringBuilder();
            StringBuilder builderRows = new StringBuilder();
            tableData.Add(table);










            //    var data = tableData;
            //    T myclass = new T();
            //    string query = Query(ModelTypeValues.GetType().Name, sqlcom, selectCommnad);
            //    for (int i = 0; i < tableData.Count; i++)
            //    {
            //        count = new int[tableData.Count];
            //        builderCols = new StringBuilder("( ");

            //        List<T> cols = new List<T>();
            //        cols.Add(table);
            //        builderRows = new StringBuilder("values ( ");

            //        foreach (var r in data)
            //        {
            //            List<T> rows = new List<T>();
            //            rows.Add(table);
            //            var row = rows;


            //            builderCols.Append(r.ToString());


            //            foreach (var c in row)
            //            {
            //                builderRows.Append("@" + c.GetType().GetProperties());



            //                switch (sqlcom)
            //                {
            //                    case "Insert":

            //                        break;

            //                }


            //                SqlCommand insertCommand = new SqlCommand(query, oasisconnectionString);

            //                insert(c.GetType().Name, Convert.ChangeType(r, r.GetType()).ToString(), insertCommand);

            //                builderRows.Append(" , ");



            //            }
            //            builderCols.Append(" , ");

            //        }
            //        builderRows.Append(" )");


            //        builderCols.Append(" )");

            //    }

            //    string finalQuery = query + " " + builderCols.ToString() + " " + builderRows.ToString();

            //}

        }
    }
}
