using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    [Table("playlists", Schema = "public")]
    public class PlaylistModel
    {
        public string username { get; set; }

        public string trackname { get; set; }

        public string playlistname { get; set; }
        public string genre { get; set; }

        public string artist { get; set; }

    }

    
}