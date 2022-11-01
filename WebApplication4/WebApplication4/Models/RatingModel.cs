using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    [Table("review", Schema = "public")]
    public class RatingModel
    {
        [Key]
        public int songid { get; set; }
        public string songname { get; set; }
        public float rating { get; set; }
        public string review { get; set; }

    }
}