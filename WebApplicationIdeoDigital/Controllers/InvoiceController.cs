using WebApplicationIdeoDigital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace WebApplicationIdeoDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InvoiceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            List<Invoice> invoices = new List<Invoice>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Invoices", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    /** InvoiceID Date CustomerID Status Amount **/
                    Invoice invoice = new Invoice
                    {
                        InvoiceID = Convert.ToInt32(reader["InvoiceID"]),
                        Date = DateTime.Parse(reader["Date"].ToString()),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        Status = reader["Status"].ToString(),
                        Amount = Convert.ToInt32(reader["Amount"]),
                         
                    };

                    invoices.Add(invoice);
                }
            }
            return Ok(invoices);
        }



        [HttpPost]
        public IActionResult Post([FromBody] Invoice invoice)
        {
            if (invoice == null)
                return BadRequest();
            invoice.Date = invoice.Date.AddHours(3);
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                /** InvoiceID Date CustomerID Status Amount **/
                SqlCommand command = new SqlCommand("INSERT INTO Invoices (Date, CustomerID, Status, Amount) VALUES (@Date, @CustomerID, @Status, @Amount); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@Date", invoice.Date);
                command.Parameters.AddWithValue("@CustomerID", invoice.CustomerID);
                command.Parameters.AddWithValue("@Status", invoice.Status);
                command.Parameters.AddWithValue("@Amount", invoice.Amount);
                int insertedId = Convert.ToInt32(command.ExecuteScalar());

                invoice.InvoiceID = insertedId;
            }

            return Ok(invoice);
        }



        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Invoice invoice)
        {
            if (invoice == null || invoice.InvoiceID != id)
                return BadRequest();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("UPDATE Invoices SET Date = @Date, CustomerID = @CustomerID, Status = @Status, Amount = @Amount WHERE InvoiceID = @InvoiceID", connection);
                command.Parameters.AddWithValue("@InvoiceID", invoice.InvoiceID);
                command.Parameters.AddWithValue("@Date", invoice.Date);
                command.Parameters.AddWithValue("@CustomerID", invoice.CustomerID);
                command.Parameters.AddWithValue("@Status", invoice.Status);
                command.Parameters.AddWithValue("@Amount", invoice.Amount);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound();
            }

            return Ok(invoice);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Invoice invoice = null;

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Invoices WHERE InvoiceID = @InvoiceID", connection);
                command.Parameters.AddWithValue("@InvoiceID", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    invoice = new Invoice
                    {
                        InvoiceID = Convert.ToInt32(reader["InvoiceID"]),
                        Date = DateTime.Parse(reader["Date"].ToString()),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        Status = reader["Status"].ToString(),
                        Amount = Convert.ToInt32(reader["Amount"]),
                    };
                }
            }

            if (invoice == null)
                return NotFound();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Invoices WHERE InvoiceID = @InvoiceID", connection);
                command.Parameters.AddWithValue("@InvoiceID", id);
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                    return NotFound();
            }

            return Ok(invoice);
        }

    }
}
