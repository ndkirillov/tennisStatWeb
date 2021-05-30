using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PraktikaWeb.Models
{
    public class ShotPlayerMatch
    {
        public TennisPlayers players { get; set; }
        public Shot shot { get; set; }
        public Match_Progress matchProgress { get; set; }
    }
}