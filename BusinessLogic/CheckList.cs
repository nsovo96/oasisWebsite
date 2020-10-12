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
   public class CheckList
    {
        List<ManagementCheckList> TaskM = new List<ManagementCheckList>();
        string ConnectionStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OASISDB.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False  ";
        SqlConnection oasisconnectionString;

        public ManagementCheckList Item;
        public string key = "";
        //my constructors
        public CheckList(ManagementCheckList Item, string key)

        {


            this.Item = Item;
            this.key = key;
        }

        public CheckList(string key)
        {
            this.key = key;
        }


        public CheckList()
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




        public void UpdateString(int id,
               decimal _4_Softner,
           decimal _5_1_Micron_Preasure,
           decimal _6_Membrane_Preasure,
           decimal _7_Flow_Rate_RAW,
           string _8_Flow_Rate_Clean,
           string _9_Pump_Functional,
           decimal Conductive,
           decimal Clean_Water_Meter_Reading,
           decimal Litres_Purified_Since,

             decimal _2_PF_5_Micron,
           decimal _1_PF_10_Micron,
           decimal Clean_tank_level,
           decimal Mun_Tank_level,
           string Silcone_Crystal_Color,
           string Ozone,
           decimal TDs_Main,
           decimal TDs_clean,
           decimal _3_Sand_Filter)
        {


              string query = "Update ManagementCheckList SET  _4_Softner = '" + _4_Softner + "' ," + " _5_1_Micron_Preasure='" + _5_1_Micron_Preasure + "' ," + " _6_Membrane_Preasure = '" + _6_Membrane_Preasure + "' ," + "_7_Flow_Rate_RAW='" + _7_Flow_Rate_RAW + "' ," + " _8_Flow_Rate_Clean = '" + _8_Flow_Rate_Clean + "' ," + " _9_Pump_Functional='" + _9_Pump_Functional + "' ," + " Conductive='" + Conductive + "' ," + " _3_Sand_Filter ='" + _3_Sand_Filter + "' ," + "  _2_PF_5_Micron='" + _2_PF_5_Micron + "' ," + " Clean_tank_level='" + Clean_tank_level + "' ," + " Mun_Tank_level = '" + Mun_Tank_level + "' ," + " Silcone_Crystal_Color='" + Silcone_Crystal_Color + "' ," + " Ozone='" + Ozone + "' ," + " TDs_Main='" + TDs_Main + "' ," + " TDs_clean='" + TDs_clean + "' ," + " Litres_Purified_Since_Previous_Reading='" + Litres_Purified_Since + "' ," + " Clean_Water_Meter_Reading='" + Clean_Water_Meter_Reading + "' ," + " _1_PF_10_Micron='" + _1_PF_10_Micron + "' where id='" + id + "'";


     


            oasisconnectionString = new SqlConnection(ConnectionStr);

            SqlCommand command = new SqlCommand(query, oasisconnectionString);
            oasisconnectionString.Open();
            // int counter = 0;

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();

           
        }
        //do a final addition to my query to best fit the command
        public string Finalizequery(int id)
        {

            string query = "";

            switch (key)
            {
                case "InsertA":


                    query = "insert into ManagementCheckList (date,_4_Softner,_5_1_Micron_Preasure,_6_Membrane_Preasure,_7_Flow_Rate_RAW,_8_Flow_Rate_Clean,_9_Pump_Functional,Conductive,Clean_Water_Meter_Reading,Litres_Purified_Since_Previous_Reading,CheckListType,_3_Sand_Filter,_2_PF_5_Micron,_1_PF_10_Micron,Clean_tank_level,Mun_Tank_level,Silcone_Crystal_Color,Ozone,TDs_Main,TDs_clean) " +
                   " values" +
                        "(@date,@_4_Softner,@_5_1_Micron_Preasure,@_6_Membrane_Preasure,@_7_Flow_Rate_RAW,@_8_Flow_Rate_Clean,@_9_Pump_Functional,@Conductive,@Clean_Water_Meter_Reading,@Litres_Purified_Since_Previous_Reading,@CheckListType,@_3_Sand_Filter,@_2_PF_5_Micron,@_1_PF_10_Micron,@Clean_tank_level,@Mun_Tank_level,@Silcone_Crystal_Color,@Ozone,@TDs_Main,@TDs_clean)";


                    break;
                case "InsertB":

                         query = "insert into ManagementCheckList (date,_4_Softner,_5_1_Micron_Preasure,_6_Membrane_Preasure,_7_Flow_Rate_RAW,_8_Flow_Rate_Clean,_9_Pump_Functional,Conductive,Clean_Water_Meter_Reading,Litres_Purified_Since_Previous_Reading,CheckListType,_3_Sand_Filter,_2_PF_5_Micron,_1_PF_10_Micron,Clean_tank_level,Mun_Tank_level,Silcone_Crystal_Color,Ozone,TDs_Main,TDs_clean) " +
                        "values" +
                        "(@date,@_4_Softner,@_5_1_Micron_Preasure,@_6_Membrane_Preasure,@_7_Flow_Rate_RAW,@_8_Flow_Rate_Clean,@_9_Pump_Functional,@Conductive,@Clean_Water_Meter_Reading,@Litres_Purified_Since_Previous_Reading,@CheckListType,@_3_Sand_Filter,@_2_PF_5_Micron,@_1_PF_10_Micron,@Clean_tank_level,@Mun_Tank_level,@Silcone_Crystal_Color,@Ozone,@TDs_Main,@TDs_clean)";


                    break;

                case "Select":


                    query = "Select * from ManagementCheckList";
                    break;
                case "Delete":


                    query = "Delete from ManagementCheckList where id=" + id;
                    break;

                case "Update":


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
        public void AddcheckListB(
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



            decimal MunTanklevelPercentage = ((_1_PF_10_Micron + _2_PF_5_Micron + _3_Sand_Filter + _4_Softner)/200)*100;
            decimal cleanTankLevelPercentage = ((_5_1_Micron_Preasure + _6_Membrane_Preasure)/400)*100;

            string OzoneStatus = "";

            if(MunTanklevelPercentage<=50)
            {
                OzoneStatus = "Not OK";
            }else if((MunTanklevelPercentage>50 ) && MunTanklevelPercentage <=100)
            {
                OzoneStatus = "OK";

            }else
            {
                OzoneStatus = "Critical Effected";
            }

            //color meanings 
            //Purple The tank is clean
            //Blue Slighly clean will need to be changed soon so that they can prepare for anytihing that need to be changed on the shop
            //pink its not clean, an employee need to change it

            string CrystalColor = "";

            if((cleanTankLevelPercentage<50))
            {
                CrystalColor = "Pink";
            }else if((cleanTankLevelPercentage>=50) && (cleanTankLevelPercentage<=100) )
            {
                CrystalColor = "Purple";
            }

            //The pump cannot pump from a tank which is not clean and the ozone is not ok.
            //We need to dertimine if the ozone is ok and txhe tank is clean so that we can be notified that the pump is not pumping water
            string PumpFuncntionality = "";

            if ((CrystalColor == "Purple") && (OzoneStatus == "OK"))
            {
                PumpFuncntionality = "OK";
            }
            else if ((CrystalColor == "Pink") && (OzoneStatus == "OK"))
            {
                PumpFuncntionality = "Not OK, Tank Need to be Changed";

            }
            else if ((CrystalColor == "Purple") && (OzoneStatus == "Not OK"))
            {
                PumpFuncntionality = "Not OK, Ozone need to be Udjusted";
            }
            else if ((CrystalColor == "Pink") && (OzoneStatus == "Not OK"))
            {
                PumpFuncntionality = "Not OK";
            }

            //we need to dertimine how much clean water was produced at the day
            //clean water are only produced when he pump Functionality is ok
            //single tank hold up to 100 liters of water, so it can be filled to any point in liters(can only clean up 100 liters of water at a time
            //to get the percentage of the clean water produced at a day, we will use the clean tank percentage.
            //We will also use the Mun tank level, this is the percentage from the value entered by the manager based on how he used the scale at oasis to measure the psi unit.
            decimal CleanWaterTotalReading = 0;
            decimal totalPercentage =0;
            decimal CleanWaterTotalFlowrateCapacity =0;

            if (PumpFuncntionality=="OK")
            {
                CleanWaterTotalFlowrateCapacity = (_7_Flow_Rate_RAW / 100) * 100*25; // in percentages
                //how much in percentages can the tannk hold and clean water
                totalPercentage = (((MunTanklevelPercentage + cleanTankLevelPercentage)/200)*100); //in percentages

                //we need to get clean water reading,this  gives a reading of much water was cleaned
                CleanWaterTotalReading = (CleanWaterTotalFlowrateCapacity / totalPercentage) * 100; //in liters
            }
            else
            {
                CleanWaterTotalFlowrateCapacity = 0;

            }



            // we need to find out how much clean water was produced on that day over raw water, oasis clean upto 100 liters of water per day

            decimal DayCleanedWater = CleanWaterTotalReading / _7_Flow_Rate_RAW * 100 ; //in liters


            // we need to get the conductivity

            //we get the conductivity by mesuring the TDS of clean water and the Tds of raw water. this value are entered by the manager.this will be divided by the total factors effected the change of water value
            //
            decimal conductivity = ((TDs_clean + TDs_Main) / (_1_PF_10_Micron + _2_PF_5_Micron + _3_Sand_Filter + _4_Softner + _5_1_Micron_Preasure + _6_Membrane_Preasure)); //in liter/psi


            NotificationsDisplay notifications = new NotificationsDisplay();

            if(CrystalColor== "Pink")
            {
                notifications.CreateAnotification("Crystal color is Pink", "ProccessMaintananceEmployee");
            }

            if(OzoneStatus=="Not OK")
            {
                notifications.CreateAnotification("Ozone is not ok", "ProccessMaintananceEmployee");

            }



            SqlCommand insertcommand = GetSqlCommand();
            insertcommand.Parameters.AddWithValue("@date", DateTime.Now);

            insertcommand.Parameters.AddWithValue("@_4_Softner", _4_Softner);
            insertcommand.Parameters.AddWithValue("@_5_1_Micron_Preasure", _5_1_Micron_Preasure);

            insertcommand.Parameters.AddWithValue("@_6_Membrane_Preasure", _6_Membrane_Preasure);
            insertcommand.Parameters.AddWithValue("@_7_Flow_Rate_RAW", _7_Flow_Rate_RAW);
            insertcommand.Parameters.AddWithValue("@_8_Flow_Rate_Clean", CleanWaterTotalFlowrateCapacity);

            insertcommand.Parameters.AddWithValue("@_9_Pump_Functional", PumpFuncntionality);
            insertcommand.Parameters.AddWithValue("@Conductive", conductivity);
            insertcommand.Parameters.AddWithValue("@Clean_Water_Meter_Reading", CleanWaterTotalReading);
            insertcommand.Parameters.AddWithValue("@CheckListType", "B");

            insertcommand.Parameters.AddWithValue("@Litres_Purified_Since_Previous_Reading", DayCleanedWater);

            insertcommand.Parameters.AddWithValue("@_3_Sand_Filter", _3_Sand_Filter);
            insertcommand.Parameters.AddWithValue("@_2_PF_5_Micron", _2_PF_5_Micron);

            insertcommand.Parameters.AddWithValue("@_1_PF_10_Micron", _1_PF_10_Micron);
            insertcommand.Parameters.AddWithValue("@Clean_tank_level", cleanTankLevelPercentage);
            insertcommand.Parameters.AddWithValue("@Mun_Tank_level", MunTanklevelPercentage);

            insertcommand.Parameters.AddWithValue("@Silcone_Crystal_Color", CrystalColor);
            insertcommand.Parameters.AddWithValue("@Ozone", OzoneStatus);

            insertcommand.Parameters.AddWithValue("@TDs_Main", TDs_Main);
            insertcommand.Parameters.AddWithValue("@TDs_clean", TDs_clean);
            oasisconnectionString.Open();

            int results = insertcommand.ExecuteNonQuery();

            if (results > 0)
            {
                oasisconnectionString.Close();
            }

        }

        public List<ManagementCheckList> GetManagementCheckLists()
        {
            List<ManagementCheckList> managementCheckLists = new List<ManagementCheckList>();
            SqlCommand Selectcommand = GetSqlCommand();
            oasisconnectionString.Open();
            int counter = 0;

            using (SqlDataReader rd = Selectcommand.ExecuteReader())
            {
                while (rd.Read())
                {
                    ManagementCheckList checkList = new ManagementCheckList();
                    checkList.Id = Convert.ToInt32(rd["Id"]);

                    checkList.date = Convert.ToDateTime(rd["date"]);
                    checkList.TDs_clean = Convert.ToDecimal(rd["TDs_clean"]);
                    checkList.TDs_Main = Convert.ToDecimal(rd["TDs_Main"]);
                    checkList.Ozone = rd["Ozone"].ToString();
                    checkList.Silcone_Crystal_Color = rd["Silcone_Crystal_Color"].ToString();
                    checkList.Mun_Tank_level = Convert.ToDecimal(rd["Mun_Tank_level"]);
                    checkList.Clean_tank_level = Convert.ToDecimal(rd["Clean_tank_level"]);
                    checkList._1_PF_10_Micron = Convert.ToDecimal(rd["_1_PF_10_Micron"]);
                    checkList._2_PF_5_Micron = Convert.ToDecimal(rd["_2_PF_5_Micron"]);
                    checkList._3_Sand_Filter = Convert.ToDecimal(rd["_3_Sand_Filter"]);
                    checkList._4_Softner = Convert.ToDecimal(rd["_4_Softner"]);
                    checkList._5_1_Micron_Preasure = Convert.ToDecimal(rd["_5_1_Micron_Preasure"]);
                    checkList._6_Membrane_Preasure = Convert.ToDecimal(rd["_6_Membrane_Preasure"]);
                    checkList._7_Flow_Rate_RAW = Convert.ToDecimal(rd["_7_Flow_Rate_RAW"]);
                    checkList._8_Flow_Rate_Clean = rd["_8_Flow_Rate_Clean"].ToString();
                    checkList._9_Pump_Functional =rd["_9_Pump_Functional"].ToString();
                    checkList.Conductive = Convert.ToDecimal(rd["Conductive"]);
                    checkList.Clean_Water_Meter_Reading = Convert.ToDecimal(rd["Clean_Water_Meter_Reading"]);
                    checkList.Litres_Purified_Since_Previous_Reading = Convert.ToDecimal(rd["Litres_Purified_Since_Previous_Reading"]);
                    checkList.CheckListType = rd["CheckListType"].ToString();
                    managementCheckLists.Add(checkList);

                    counter += 1;
                   
                }

            }

            oasisconnectionString.Close();
            return managementCheckLists;
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


        public void Delete(int checkID)
        {
            SqlCommand Deletecommand = GetSqlCommand(checkID);
            oasisconnectionString.Open();
            // int counter = 0;

            Deletecommand.CommandType = CommandType.Text;

            Deletecommand.ExecuteNonQuery();
            oasisconnectionString.Close();

        }
    }
}
