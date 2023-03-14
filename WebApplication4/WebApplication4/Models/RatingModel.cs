﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    [Table("rating", Schema = "public")]
    public class RatingModel
    {

        public string username { get; set; }
        public string trackid { get; set; }
        public float rating { get; set; }

        public float total { get; set; }

    }
}