using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PraktikaWeb.Models
{
    public class ExportModel
    {
       public Tournament tour { get; set; }
       public string category { get; set; }
      public Country country { get; set; }
       public Shot shot { get; set; }
        public Match_Progress prog { get; set; }
        public TennisPlayers players { get; set; }


        [Required]
      public int  startSpeed { get; set; }
        [Required]
        public int stopSpeed { get; set; }
        [Required]
        public string startDate { get; set; }
        [Required]
        public string stopDate { get; set; }

    }
}