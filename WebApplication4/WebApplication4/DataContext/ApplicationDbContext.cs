using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebApplication4.Models;

namespace WebApplication4.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base(nameOrConnectionString: "Myconnection")
        {
        

        }

  
        public virtual DbSet<ReviewModel> RatingObj { get; set; }

        
    }
}