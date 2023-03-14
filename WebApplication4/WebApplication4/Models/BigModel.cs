using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebApplication4.Data;

namespace WebApplication4.Models
{
    public class BigModel
    {
        public UserModel UserModel { get; set; }
        public PlaylistModel PlaylistModel { get; set; }
        public List<ReviewModel>  ReviewModel{ get; set; }
        
        public RatingModel RatingModel { get; set; }

    }
}