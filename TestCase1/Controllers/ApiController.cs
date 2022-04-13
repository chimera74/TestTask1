using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace TestCase1.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        
        private readonly ILogger<ApiController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ApiController(ILogger<ApiController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        }

        [HttpGet]
        public JsonResult Get(int? amount, int? startIndex)
        {
            
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            createTableIfNotExist(connection);
                
            using SqlCommand command = formSqlCommand(connection, amount, startIndex);
            SqlDataReader reader = command.ExecuteReader();
            
            var res = new List<Item>();
            while (reader.Read())
            {
                res.Add(new Item()
                {
                    Id = Convert.ToInt32(reader.GetValue(0)),
                    Code = Convert.ToInt32(reader.GetValue(1)),
                    Value = reader.GetValue(2).ToString()
                });
            }
            
            reader.Close();
            connection.Close();
            
            return new JsonResult(res);
        }

        private SqlCommand formSqlCommand(SqlConnection connection, int? amount, int? startIndex)
        {
            StringBuilder query = new StringBuilder(@"
                SELECT * FROM dbo.Items
                ORDER BY Id");
            if (amount is > 0)
                query.Append(" OFFSET (@skip) ROWS");
            if (startIndex is > 0)
                query.Append(" FETCH NEXT (@take) ROWS ONLY");
            SqlCommand command = new SqlCommand(query.ToString(), connection);
            if (amount is > 0) 
                command.Parameters.AddWithValue("@skip", startIndex);
            if (startIndex is > 0)
                command.Parameters.AddWithValue("@take", amount);
            return command;
        }
        
        [HttpPost]
        public ActionResult Post(List<Item> items)
        {
            if (items == null)
                return BadRequest();

            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            
            createTableIfNotExist(connection);
            
            using SqlTransaction transaction = connection.BeginTransaction();
            
            string deleteQuery = @"DELETE FROM dbo.Items
                                   DBCC CHECKIDENT ('dbo.Items', RESEED, 0)";
            using SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection, transaction);
            deleteCommand.ExecuteNonQuery();
            
            string query = "INSERT INTO dbo.Items(code, val) VALUES(@code,@val)";
            using SqlCommand insertCommand = new SqlCommand(query, connection, transaction);
            insertCommand.Parameters.Add(new SqlParameter("@code", SqlDbType.Int));
            insertCommand.Parameters.Add(new SqlParameter("@val", SqlDbType.NChar));
            
            foreach (var item in items.OrderBy(i=>i.Code))
            {
                insertCommand.Parameters[0].Value = item.Code;
                insertCommand.Parameters[1].Value = item.Value;
                insertCommand.ExecuteNonQuery();
            }
            
            transaction.Commit();
            return StatusCode(StatusCodes.Status201Created);

        }

        private void createTableIfNotExist(SqlConnection connection)
        {
            string createQuery = @"IF NOT EXISTS (SELECT 'X'
                                    FROM INFORMATION_SCHEMA.TABLES
                                    WHERE TABLE_NAME = 'Items')                                    
                                    CREATE TABLE [dbo].[Items](
	                                    ID int IDENTITY(1,1) NOT NULL,
                                    	code int NOT NULL,
	                                    val nvarchar(255))";
            using SqlCommand command = new SqlCommand(createQuery, connection);
            command.ExecuteNonQuery();
        }
    }
    
    
}