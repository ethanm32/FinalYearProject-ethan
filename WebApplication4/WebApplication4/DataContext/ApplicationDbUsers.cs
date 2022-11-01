using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication4.Models;

namespace WebApplication4.DataContext
{
    public class ApplicationDbUsers : DbContext
    {
        public ApplicationDbUsers() : base(nameOrConnectionString: "newConnection")
        {


        }


        public virtual DbSet<UserModel> UserObj { get; set; }

    }
}