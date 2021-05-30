using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PraktikaWeb.Models;

namespace PraktikaWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DBTennisContext db = new DBTennisContext();
            List<Tournament> tournament = db.Tournament.ToList();
            List<Start_Table> startTable = db.Start_Table.ToList();
            List<Country> country = db.Country.ToList();
            using (db = new DBTennisContext())
            {
                var queryTournament = from p in tournament
                                      join t in startTable
                                      on p.ID_Tournament equals t.Tournament_info
                                      join r in country
                                      on p.Country_info equals r.ID_Country
                                      select new TournamentInfoModel
                                      {
                                          tournament = p,
                                          country = r,
                                          start = t
                                      };
                return View(queryTournament);
            }
           
        }
        public ActionResult DetailsTournament(int id)
        {
            
            DBTennisContext db = new DBTennisContext();
            List<Match> match = db.Match.ToList();
            List<TennisPlayers> players = db.TennisPlayers.ToList();
            List<Match_Tennis_Player> matchPlayers = db.Match_Tennis_Player.ToList();
            List<string[]> date = new List<string[]>();
            using (db = new DBTennisContext())
            {
                var queryMatch = from p in match
                                      where p.ID_Tournament == id
                                      select new Match
                                      {
                                          Match_Stage =p.Match_Stage,
                                          Match_Score = p.Match_Score,
                                          ID_Tournament = id,
                                          ID_Match = p.ID_Match,
                                      };
                foreach (var s in queryMatch)
                {
                   
                    var queryPlayers = from p in players
                                       join e in matchPlayers
                                       on p.ID_TennisPlayers equals e.ID_Player
                                     where e.ID_Match == s.ID_Match
                                     select new 
                                     {
                                        playerName = p.Surname
                                     };
                    string tennisPlayers1 = "";
                    string tennisPlayers2 = "";
                    int i = 0;
                    foreach (var w in queryPlayers)
                    {
                        if (i == 0)
                            tennisPlayers1 = w.playerName;
                        else tennisPlayers2 = w.playerName;
                        i++;
                    }
                    date.Add(new string[3]);
                    date[date.Count - 1][0] = s.Match_Stage.ToString();
                    date[date.Count - 1][1] = s.Match_Score.ToString();
                    date[date.Count - 1][2] = tennisPlayers1 + " - " + tennisPlayers2;
                }
                Object[] ForReturn = new Object[2];
                ForReturn[0] = id;
                ForReturn[1] = date;
                return View(ForReturn);
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}