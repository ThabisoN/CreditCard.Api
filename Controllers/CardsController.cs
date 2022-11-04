using CreditCard.Api.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CreditCard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public CardsController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string getQuery = @"
                         select Id, Name, Number, Expiry, CVC from dbo.CreditCard";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CreditCardValidation_DB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCom = new SqlCommand(getQuery, myCon))
                {
                    myReader = myCom.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Card cd)
        {
            int recordsInserted = 0;

            string sqlDataSource = _configuration.GetConnectionString("CreditCardValidation_DB");
           
            string addQuery = "IF NOT EXISTS(SELECT Id FROM dbo.CreditCard  WHERE Number = @Number) ";
            addQuery += "BEGIN ";
            addQuery += "INSERT INTO dbo.CreditCard  (Name, Number, Expiry, CVC) VALUES(@Name, @Number, @Expiry, @CVC) ";
            addQuery += "END";

            DataTable table = new DataTable();
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using SqlCommand myCommand = new SqlCommand(addQuery, myCon);
                myCommand.Parameters.AddWithValue("@Name", cd.Name);
                myCommand.Parameters.AddWithValue("@Number", cd.Number);
                myCommand.Parameters.AddWithValue("@Expiry", cd.Expiry);
                myCommand.Parameters.AddWithValue("@CVC", cd.CVC);
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);
                myReader.Close();
                myCon.Close();
            }

            if (recordsInserted == -1)
            {
                return new JsonResult("Duplicate Card Number");
            }
            else
            {
                return new JsonResult("Added Successfully");
            }
            


        }

        [HttpPut]
        public JsonResult Put(Card cd)
        {
            string query = @"
                             update dbo.CreditCard 
                              set Name = @Name, Number =@Number, Expiry =@Expiry, CVC = @CVC
                             where Id = @Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CreditCardValidation_DB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", cd.Id);
                    myCommand.Parameters.AddWithValue("@Name", cd.Name);
                    myCommand.Parameters.AddWithValue("@Expiry", cd.Expiry);
                    myCommand.Parameters.AddWithValue("@CVC", cd.CVC);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                             delete from dbo.CreditCard
                              where Id = @Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CreditCardValidation_DB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

    }
}
