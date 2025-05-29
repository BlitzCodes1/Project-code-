using System.Data.SQLite;
using System.Diagnostics;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        string name1; 

        public List<User> AllUsers { get; set; } = new List<User>();

        public class User
        {
            public string Name { get; set; } = "";
            public string Email { get; set; } = "";
        }

        private void InsertNewUser(string name, string email)
        {
            string sqlQuery = "INSERT INTO Resturant (name, email) VALUES (@name, @email)";
            SQLiteConnection connection = new SQLiteConnection("Data Source=Resturant.db");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@email", email);
            command.ExecuteNonQuery();
            connection.Close();
        }
        private bool VerifyUser(string name, string email)
        {
            
            string sqlQuery = "SELECT name, email FROM Resturant WHERE name = @name AND email = @email";

           
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=Resturant.db"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@email", email);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                       
                        if (reader.Read())
                        {
                            
                            name1 = "user already exits";
                            return false; 
                            
                        }
                    }
                }
            } 

            
            return true; 
        }
        private List<User> GetAllUsers()
        {
            var users = new List<User>();
            string sqlQuery = "SELECT name, email FROM Resturant";

            SQLiteConnection connection = new SQLiteConnection("Data Source=Resturant.db");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if(VerifyUser(reader["name"].ToString(), reader["name"].ToString())) { 
                        users.Add(new User
                        {
                       
                            Name = reader["name"].ToString() ?? "",
                            Email = reader["email"].ToString() ?? ""
                        });
                    }
                }
            }

            connection.Close();
            return users;
        }

        public void OnGet()
        {
            AllUsers = GetAllUsers();
        }

        
        public  bool condition;
        public void OnPost()
        {
            string name = Request.Form["name"];
            string email = Request.Form["email"];

            condition = VerifyUser(name, email); 

            if (condition)
            {
                
                InsertNewUser(name, email);
               
            }
            else
            {
                
               
            }

           
            AllUsers = GetAllUsers();
        }

    }
}