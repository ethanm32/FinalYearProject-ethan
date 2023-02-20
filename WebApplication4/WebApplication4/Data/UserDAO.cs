using Npgsql;
using System.Collections.Generic;
using System.Configuration;
using WebApplication4.Models;

namespace WebApplication4.Data
{ 
    internal class UserDAO
    {

        private string conn = "Host=localhost;Port=5432;Database=users;User Id=admin;password=secret";

        public List<UserModel> Fetch()
        {
            List<UserModel> returnList = new List<UserModel>();
            var newConn = new NpgsqlConnection(conn);
            

            string sql = "select * from public.users where username='ethanm324'";
            var cmd = new NpgsqlCommand(sql, newConn);
            newConn.Open();
            NpgsqlDataReader reader = cmd.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read()) {
                    
                    UserModel user = new UserModel();

                    user.id = reader.GetInt32(0);
                    user.email= reader.GetString(1);
                    user.password= reader.GetString(2);
                    user.name= reader.GetString(3);
                    user.username= reader.GetString(4);
                        
                    returnList.Add(user);
                }

            }


            return returnList;
        }
    }
}