using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PraktikaWeb.Models
{
    public class TournamentInfoModel
    {
        public Tournament tournament { get; set; }
        public Country country { get; set; }
        public Start_Table start { get; set; }
    }
}