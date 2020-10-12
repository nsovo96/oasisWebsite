
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
using System.Windows.Forms;
using DataAccessLayer.Models;
using BusinessLogic;

namespace OasisCommunicationManagement.Controllers
{
    public class UserAccountsController : Controller
    {
        // GET: Client
        private string OasisConnectionManager;
        private SqlConnection OasisConnection;
        public ActionResult Index()
        {
           

           return View();
        }

        public ActionResult logout()
        {
            Session.Abandon();

            Session.Remove("id");
            Session.Remove("TaskID");
            Session.Remove("Fk_EmployeeID");
            Session.Remove("SenderId");
            Session.Remove("Username");
            Session.Remove("Fk_Reciever");
            Session.Remove("notificationID");


            return RedirectToAction("Home", "index");
        }
        public ActionResult Login()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Login(String Email,String Password)
        {
            List<Users> userManager = new List<Users>();


            UserAccounts AccountList = new UserAccounts("Select");

            userManager = AccountList.Login();



            foreach(var U in userManager)
            {

                if(U.Email== Email && U.Password== Password)
                {
                    Session["username"] ="Welcome" +  U.FullNames;
                    Session["userId"] = U.id;

                    Session["UserRole"] = U.UserRole;
                    switch (U.UserRole)
                    {
                        case "ProccessAreaEmployee":
                            Session["addtoLayout"] = "_HomePageLayout.cshtml";

                            return RedirectToAction("index", "Home");
                        case "StorageAreaEmployee":
                            Session["addtoLayout"] = "_HomePageLayout.cshtml";

                            return RedirectToAction("Index", "Home");
                        case "FrontEndEmployee":
                            Session["addtoLayout"] = "_HomePageLayout.cshtml";

                            return RedirectToAction("Index", "Home");

                        case "Manager":
                            Session["addtoLayout"] = "_IndexPageLayout.cshtml";
                            return RedirectToAction("Index", "Home");

                        case "ProccessMaintananceEmployee":
                            Session["addtoLayout"] = "_HomePageLayout.cshtml";
                            return RedirectToAction("Index", "Home");

                    }
                }
               
            }
            return View();

        }
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
            public ActionResult Register(Users value)
        {


            List<Users> products = new List<Users>();

            products.Add(value);

            UserAccounts genericProduct = new UserAccounts(value, "Insert");

            genericProduct.Register();

            return RedirectToAction("Index", "Home");
        }

    }

    

   
}