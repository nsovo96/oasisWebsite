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
using DataAccessLayer;
using BusinessLogic;
using DataAccessLayer.Models;

namespace OasisCommunicationManagement.Controllers
{
   

    public class HomeController : Controller
    {
        int id=0;

             // GET: ProductionDepartment


        public ActionResult Index()
        {
            NotificationsDisplay notifications = new NotificationsDisplay("Select");

            List<Notifications> pNotiList = new List<Notifications>();


            pNotiList = notifications.dispalayNotifications();

            List<Notifications> Mynotifications = new List<Notifications>();

            foreach (var n in pNotiList)
            {
                if(n.RecieverRole == Session["UserRole"].ToString())
                {
                    Notifications notifications1 = new Notifications();
                    notifications1 = n;
                    Mynotifications.Add(notifications1);

                }

               

            }

            return View(Mynotifications);

        }


        public ActionResult ViewNotification(int id)
        {
            NotificationsDisplay notifications = new NotificationsDisplay("Select");

            Session["notificationId"] = id;
            Notifications mynotificatin= notifications.GetNotificationsID(id);

            return View(mynotificatin);
        }



        public ActionResult DeleteNotification()
        {
            NotificationsDisplay notifications = new NotificationsDisplay("Delete");


            notifications.Delete(Convert.ToInt32( Session["notificationId"]));

            return RedirectToAction("Index");
        }



































        // public string UpdateQuery(string tableName, List<ModelFunctions> Update, string updatekey, int id)

        public ActionResult Check()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Check(Product modelClasses)
        {
           List<Product> products = new List<Product>();

            products.Add(modelClasses);

            genericProduct genericProduct = new genericProduct(modelClasses, "Insert");

            genericProduct.Insert();

            return RedirectToAction("DisplayProduct");
        }
      


        public ActionResult DisplayProduct()
        {
            genericProduct genericProduct = new genericProduct("Select");

            List<Product> productList = new List<Product>();


            productList = genericProduct.SelectAll();

            return View(productList);

        }

        public ActionResult Delete(int id)
        {
            genericProduct genericProduct = new genericProduct("Delete");


           genericProduct.Delete(id);

            return RedirectToAction("DisplayProduct");
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            List<Product> products = new List<Product>();

            products.Add(product);

            genericProduct genericProduct = new genericProduct(product, "Update");

            genericProduct.edit(productID);


            return View();
        }

        static int productID;
       public ActionResult Edit(int id)
        {
            productID = id;

            return View();
        }
    }
}