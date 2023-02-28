﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    [Table("users", Schema = "public")]
    public class UserModel
    {
        [Key]
        
        public string email { get; set; }
        public string password { get; set; }

        public string name { get; set; }
        
        public string username { get; set; }

    }
}