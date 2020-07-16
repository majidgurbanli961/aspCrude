using CustomCrude.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace CustomCrude.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<TableData> tableDatas = getTableData();

            return View(tableDatas);
        }
        private List<TableData> getTableData()
        {

            List<TableData> tableDatas = new List<TableData>();
            string connString = "Server=DESKTOP-SAVE5C2\\SQLEXPRESS;Database=CustomCrude;Trusted_Connection=True;";
            SqlConnection myconnection = new SqlConnection(connString);
            
                myconnection.Open();
                string queryString = "select * from dbo.Frameworks";
                SqlCommand command = new SqlCommand(queryString, myconnection);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                    TableData customData = new TableData() { Id = Convert.ToInt32(reader[0]),
                        Name = reader[1].ToString(),
                            Description = reader[2].ToString(), 
                        Price=reader[3].ToString()};
                        tableDatas.Add(customData);

                    }
                
                }catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            finally
            {
                myconnection.Close();

            }
            // Do work here.  


            return tableDatas;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Edit()
        {

            return RedirectToAction("Contact");
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string name, string description, int price)
        {
            customAdd(name,description,price);




            return RedirectToAction("Index");
        }
        private void customAdd(string name, string description, int price)
        {

            string connString = "Server=DESKTOP-SAVE5C2\\SQLEXPRESS;Database=CustomCrude;Trusted_Connection=True;";
            SqlConnection myconnection = new SqlConnection(connString);
            int nextId=0;
            myconnection.Open();
            string queryString = "SELECT TOP 1 * FROM dbo.Frameworks ORDER BY ID DESC";
            SqlCommand command = new SqlCommand(queryString, myconnection);
          
                 var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    nextId = Convert.ToInt32(reader[0]) + 1;
                }
            }
            else
            {
                nextId = 1;
            }
            reader.Close();
                string querySecondString = "insert into dbo.Frameworks values (@id,@name,	@description,@price )";
                SqlCommand secondcommand = new SqlCommand(querySecondString, myconnection);
                secondcommand.Parameters.Add(new SqlParameter("id", nextId));
                secondcommand.Parameters.Add(new SqlParameter("name", name));
                secondcommand.Parameters.Add(new SqlParameter("description", description));
                secondcommand.Parameters.Add(new SqlParameter("price" ,price));



                secondcommand.ExecuteNonQuery();

           
            myconnection.Close();






        }
        public ActionResult Delete(int id)
        {
            DeleteFunc(id);
            return RedirectToAction("Index");
        }
        public ActionResult Update(int id, string name, string description, int price)
        {
            TableData mydata = new TableData() { Id = id, Name = name, Description = description, Price = price.ToString() };


            return View(mydata);

        }
        [HttpPost]
        public ActionResult Update(string myid, string name, string description, string myprice, string empty)
        {
            int id = Convert.ToInt32(myid);
            int price = Convert.ToInt32(myprice);

            string connString = "Server=DESKTOP-SAVE5C2\\SQLEXPRESS;Database=CustomCrude;Trusted_Connection=True;";
            SqlConnection myconnection = new SqlConnection(connString);
            myconnection.Open();
            string querySecondString = " Update dbo.Frameworks SET Name=@name, Description=@description, Price= @price where Id=@id ";
            SqlCommand secondcommand = new SqlCommand(querySecondString, myconnection);
            secondcommand.Parameters.Add(new SqlParameter("name", name));
            secondcommand.Parameters.Add(new SqlParameter("description", description));
            secondcommand.Parameters.Add(new SqlParameter("price", price));
            secondcommand.Parameters.Add(new SqlParameter("id", id));




            secondcommand.ExecuteNonQuery();


            myconnection.Close();
            return RedirectToAction("Index");


        }

        private void DeleteFunc(int id)
        {
            string connString = "Server=DESKTOP-SAVE5C2\\SQLEXPRESS;Database=CustomCrude;Trusted_Connection=True;";
            SqlConnection myconnection = new SqlConnection(connString);

            myconnection.Open();
            string queryString = "DELETE FROM dbo.Frameworks WHERE Id= @id";
            SqlCommand command = new SqlCommand(queryString, myconnection);
            command.Parameters.Add(new SqlParameter("id", id));
            command.ExecuteNonQuery();
            myconnection.Close();



        }
        public ActionResult GetDetailedInformation(string name, string description,string price)
        {
            TableData newData = new TableData() { Id = 1, Name = name, Description = description, Price = price };


            return View(newData);
        }
    }
}