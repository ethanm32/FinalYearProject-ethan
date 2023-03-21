using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    [Table("review", Schema = "public")]
    public class YourReviewModel
    {

        public string username { get; set; }
        public string trackid { get; set; }
        public string review_desc { get; set; }

    }
}