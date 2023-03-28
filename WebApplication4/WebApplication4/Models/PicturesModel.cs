using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Models
{
    [Table("pictures", Schema = "public")]
    public class PicturesModel
    {
        public byte[] logo { get; set; }

    }


}