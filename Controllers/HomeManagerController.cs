using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PraktikaWeb.Models;
using ClosedXML.Excel;
using Application = Microsoft.Office.Interop.Word.Application;
using Word = Microsoft.Office.Interop.Word;

namespace PraktikaWeb.Controllers
{
    public class HomeManagerController : Controller
    {
        // GET: HomeManager
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
                                     Match_Stage = p.Match_Stage,
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
        public ActionResult DetailsStage(string id, int IDTour)
        {
            string idd = "";
            if (id != "Final")
            {
                idd = id[0] + "/" + id[1];
                id = idd;
            }
            DBTennisContext db = new DBTennisContext();
            List<Match> match = db.Match.ToList();
            List<Shot> shot = db.Shot.ToList();
            List<Match_Progress> matchProgress = db.Match_Progress.ToList();
            List<TennisPlayers> players = db.TennisPlayers.ToList();
            List<Match_Tennis_Player> matchPlayers = db.Match_Tennis_Player.ToList();
            using (db = new DBTennisContext())
            {
                int IDMatch = 0;
                var queryMatch = from p in match
                                 where p.ID_Tournament == IDTour && p.Match_Stage ==id
                                 select new 
                                 {
                                     
                                     idMatch = p.ID_Match,
                                 };
                foreach ( var s in queryMatch)
                {
                    IDMatch = s.idMatch;
                }

                var queryShot = from p in shot
                                join s in matchProgress
                                on p.ID_Shot equals s.Shot_Id
                                join r in players 
                                on s.ID_Player equals r.ID_TennisPlayers
                                where s.ID_Match == IDMatch
                                select new ShotPlayerMatch
                                {
                                    matchProgress  = s,
                                    shot = p,
                                    players =r                                   
                                };

                return View(queryShot);
            }
           
            
        }
        public ActionResult Clients()
        {
            
            DBTennisContext db = new DBTennisContext();
            List<Person_Profile> pp = db.Person_Profile.ToList();
            using (db = new DBTennisContext())
            {
                var queryClients = from p in pp
                                
                                 select new  Person_Profile
                                 {
                                     ID_Person =p.ID_Person,
                                     Surname_Profile = p.Surname_Profile,
                                     Name_Profile = p.Name_Profile,
                                     Email_Profile = p.Email_Profile
                                   
                                 };
                return View(queryClients);
            }
        }
        public ActionResult DeleteClient(int id)
        {
            DBTennisContext db = new DBTennisContext();
            List<Person_Profile> pp = db.Person_Profile.ToList();
            List<Passwords> pas = db.Passwords.ToList();
            using (db = new DBTennisContext())
            {
                string email = "";
                var queryClients = from p in pp
                                   where p.ID_Person == id
                                   select new Person_Profile
                                   {
                                       ID_Person = p.ID_Person,
                                       Surname_Profile = p.Surname_Profile,
                                       Name_Profile = p.Name_Profile,
                                       Email_Profile = p.Email_Profile

                                   };
                foreach (var s in queryClients)
                {
                    email = s.Email_Profile;
                    Person_Profile p = new Person_Profile
                    {
                        ID_Person = s.ID_Person,
                        Surname_Profile = s.Surname_Profile,
                        Name_Profile = s.Name_Profile,
                        Email_Profile = s.Email_Profile
                    };
                    db.Entry(p).State = EntityState.Deleted;
                }

                var queryPassword = from p in pas
                                   where p.Email == email
                                   select new 
                                   {
                                       em = p.Email,
                                       pasword = p.Password

                                   };
                foreach (var s in queryPassword)
                {
                   
                    Passwords p = new Passwords
                    {
                       Email =s.em,
                       Password = s.pasword
                    };
                    db.Entry(p).State = EntityState.Deleted;
                }
                db.SaveChanges();
                return RedirectToAction("Clients", "HomeManager");

            }
        }
        public ActionResult Players()
        {

            DBTennisContext db = new DBTennisContext();
            List<TennisPlayers> tp = db.TennisPlayers.ToList();
            using (db = new DBTennisContext())
            {
                var queryPlayers = from p in tp

                                   select new TennisPlayers
                                   {
                                       ID_TennisPlayers = p.ID_TennisPlayers,
                                      Surname = p.Surname,
                                      Age  = p.Age,
                                      Hand = p.Hand,
                                      Rating = p.Rating,
                                      Country = p.Country
                                      
                                   };
                return View(queryPlayers);
            }
        }
       public ActionResult CreatePlayer()
        {
            DBTennisContext db = new DBTennisContext();
            ViewBag.Country = new SelectList(db.Country, "Country_Name", "Country_Name");
            ViewBag.Hand = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Левая", Value = "Left".ToString(), Selected=true},
new SelectListItem { Text = "Правая", Value = "Right".ToString(),Selected=false}
}, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlayer(TennisPlayers model)
        {
            DBTennisContext dbb = new DBTennisContext();
            if (ModelState.IsValid)
            {
                DBTennisContext db = new DBTennisContext();
                // поиск пользователя в бд
                TennisPlayers user = null;
                using ( db = new DBTennisContext())
                {
                    user = db.TennisPlayers.FirstOrDefault(u => u.Surname == model.Surname && u.Age == model.Age);

                }
                if (user != null)
                {
                    
                    ViewBag.Country = new SelectList(db.Country, "Country_Name", "Country_Name");
                    ViewBag.Hand = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Левая", Value = "Left".ToString(), Selected=true},
new SelectListItem { Text = "Правая", Value = "Right".ToString(),Selected=false}
}, "Value", "Text");
                    ModelState.AddModelError("", "Такой теннинсист уже есть");
                    return View(model);
                }
                else
                {
                    using (db = new DBTennisContext())
                    {
                        TennisPlayers p = new TennisPlayers
                        {
                            Surname = model.Surname,
                            Age = model.Age,
                            Country = model.Country,
                            Hand = model.Hand,
                            Rating = model.Rating
                        };
                        db.Entry(p).State = EntityState.Added;
                        db.SaveChanges();
                    }                  
                    return RedirectToAction("Players", "HomeManager");

                }
            }
            
            ViewBag.Country = new SelectList(dbb.Country, "Country_Name", "Country_Name");
            ViewBag.Hand = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Левая", Value = "Left".ToString(), Selected=true},
new SelectListItem { Text = "Правая", Value = "Right".ToString(),Selected=false}
}, "Value", "Text");
            ModelState.AddModelError("", "Данные введены некорректно");
            return View(model);
        }
        public ActionResult ChangePlayer(int id)
        {
            DBTennisContext db = new DBTennisContext();
            List<TennisPlayers> tp = db.TennisPlayers.ToList();
            ViewBag.Country = new SelectList(db.Country, "Country_Name", "Country_Name");
            ViewBag.Hand = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Левая", Value = "Left".ToString(), Selected=true},
new SelectListItem { Text = "Правая", Value = "Right".ToString(),Selected=false}
}, "Value", "Text");

            var queryFindPlayer = from p in tp
                                  where p.ID_TennisPlayers == id
                                  select new TennisPlayers
                                  {
                                      Surname = p.Surname,
                                      Age = p.Age,
                                      ID_TennisPlayers = p.ID_TennisPlayers,
                                      Hand = p.Hand,
                                      Rating = p.Rating,
                                      Country = p.Country
                                  };
            foreach(var s in queryFindPlayer)
            {
                TennisPlayers model = new TennisPlayers
                {
                    Country = s.Country,
                    Age = s.Age,
                    ID_TennisPlayers = s.ID_TennisPlayers,
                    Hand = s.Hand,
                    Surname = s.Surname,
                    Rating = s.Rating
                };
                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePlayer(int id,TennisPlayers model)
        {
            DBTennisContext db = new DBTennisContext();
            if (ModelState.IsValid)
            {

                    TennisPlayers p = new TennisPlayers
                    {
                        ID_TennisPlayers = model.ID_TennisPlayers,
                        Surname = model.Surname,
                        Age = model.Age,
                        Country = model.Country,
                        Hand = model.Hand,
                        Rating = model.Rating
                    };
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Players", "HomeManager");

                
            }
            
            ViewBag.Country = new SelectList(db.Country, "Country_Name", "Country_Name");
            ViewBag.Hand = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Левая", Value = "Left".ToString(), Selected=true},
new SelectListItem { Text = "Правая", Value = "Right".ToString(),Selected=false}
}, "Value", "Text");
            ModelState.AddModelError("", "Данные введены некорректно");
            return View(model);
        }

        public ActionResult Reports()
        {
            ViewBag.TournamentOrShot = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Турниры", Value = "Турниры".ToString(), Selected=true},
new SelectListItem { Text = "Удары", Value = "Удары".ToString(),Selected=false}
}, "Value", "Text");

            ExportModel v = new ExportModel
            {
             
                prog = null,
                players = null,
                shot = null,
                tour = null,
                category = "",
                startSpeed = 0,
                stopSpeed =0,
                startDate = DateTime.Now.ToShortDateString(),
                stopDate = DateTime.Now.ToShortDateString()
            };
            return View(v);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reports(ExportModel ex)
        {
            if (ex.category != "Удары" && ex.category != "Турниры")
            {
                ViewBag.TournamentOrShot = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Турниры", Value = "Турниры".ToString(), Selected=true},
new SelectListItem { Text = "Удары", Value = "Удары".ToString(),Selected=false}
}, "Value", "Text");

                ExportModel v = new ExportModel
                {

                    prog = null,
                    players = null,
                    shot = null,
                    tour = null,
                    category = "",
                    startSpeed = 0,
                    stopSpeed = 0,
                    startDate = DateTime.Now.ToShortDateString(),
                    stopDate = DateTime.Now.ToShortDateString()
                };
                ModelState.AddModelError("", "Вы не выбрали сущность для отчета");
                return View(v);
            }
            else return View(ex);            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowReport(ExportModel ex)
        {
            DBTennisContext db = new DBTennisContext();


            if (ex.category == "Удары")
            {
                if (ex.startSpeed > ex.stopSpeed)
                {
                    ViewBag.TournamentOrShot = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Турниры", Value = "Турниры".ToString(), Selected=true},
new SelectListItem { Text = "Удары", Value = "Удары".ToString(),Selected=false}
}, "Value", "Text");
                    ModelState.AddModelError("", "Данные введены некорректно, проверьте правильность");
                    return View("Reports", ex);
                }
                else
                {
                    List<Shot> shot = db.Shot.ToList();
                    List<Match_Progress> progress = db.Match_Progress.ToList();
                    List<TennisPlayers> players = db.TennisPlayers.ToList();

                    var queryShotReport = from s in shot
                                          join p in progress
                                          on s.ID_Shot equals p.Shot_Id
                                          join t in players
                                          on p.ID_Player equals t.ID_TennisPlayers
                                          where s.Speed >= ex.startSpeed && s.Speed <= ex.stopSpeed
                                          select new ExportModel
                                          {
                                              shot = s,
                                              players =t,
                                              prog =p,
                                              startSpeed = ex.startSpeed,
                                              stopSpeed = ex.stopSpeed,
                                              startDate = DateTime.Now.ToShortDateString(),
                                              stopDate = DateTime.Now.ToShortDateString(),
                                              category = "Удары"                                           
                                          };               


                    return View(queryShotReport);


                }
            }
            else if (ex.category == "Турниры")
            {
                if (Convert.ToDateTime(ex.startDate) <= Convert.ToDateTime(ex.stopDate) && ex.startDate != null && ex.stopDate != null)
                {
                    DateTime startD = Convert.ToDateTime(ex.startDate);
                    DateTime stoptD = Convert.ToDateTime(ex.stopDate);


                    List<Tournament> tour = db.Tournament.ToList();
                    List<Country> country = db.Country.ToList();

                    var queryTourReport = from c in country
                                          join t in tour
                                          on c.ID_Country equals t.Country_info
                                          where t.Date_Start >= startD && t.Date_Finish <= stoptD
                                          select new ExportModel
                                          {
                                              country = c,
                                              tour = t,
                                              startSpeed = ex.startSpeed,
                                              stopSpeed = ex.stopSpeed,
                                              startDate = ex.startDate,
                                              stopDate = ex.stopDate,
                                              category = "Турниры"
                                          };

                    return View(queryTourReport);
                }
                else
                {
                    ViewBag.TrenerOrClient = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Клиенты", Value = "Клиенты".ToString(), Selected=true},
new SelectListItem { Text = "Тренировки", Value = "Тренировки".ToString(),Selected=false}
}, "Value", "Text");
                    ModelState.AddModelError("", "Данные введены некорректно, проверьте правильность");
                    return View("Reports", ex);
                }
            }            
            else
            {
                ViewBag.TrenerOrClient = new SelectList(new List<SelectListItem> {

new SelectListItem { Text = "Клиенты", Value = "Клиенты".ToString(), Selected=true},
new SelectListItem { Text = "Тренировки", Value = "Тренировки".ToString(),Selected=false}
}, "Value", "Text");
                ModelState.AddModelError("", "Данные введены некорректно, проверьте правильность");
                return View("Reports", ex);
            }

        }


        public ActionResult ExportExcel(string IdCategory , string IdStartDate , string IdStopDate , int idStartS , int idStopS)
        {
            DBTennisContext db = new DBTennisContext();
            if (IdCategory == "Удары")
            {

                List<Shot> shot = db.Shot.ToList();
                List<Match_Progress> progress = db.Match_Progress.ToList();
                List<TennisPlayers> players = db.TennisPlayers.ToList();

                var queryShotReport = from s in shot
                                      join p in progress
                                      on s.ID_Shot equals p.Shot_Id
                                      join t in players
                                      on p.ID_Player equals t.ID_TennisPlayers
                                      where s.Speed >= idStartS && s.Speed <= idStopS
                                      select new ExportModel
                                      {
                                          shot = s,
                                          players = t,
                                          prog = p,
                                          startSpeed = idStartS,
                                          stopSpeed =idStopS,
                                          startDate = DateTime.Now.ToShortDateString(),
                                          stopDate = DateTime.Now.ToShortDateString(),
                                          category = "Удары"
                                      };

                using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
                {
                    var worksheet = workbook.Worksheets.Add("Brands");

                    worksheet.Cell("A1").Value = "Cкорость удара";
                    worksheet.Cell("B1").Value = "Игрок";
                    worksheet.Row(1).Style.Font.Bold = true;

                    //нумерация строк/столбцов начинается с индекса 1 (не 0)
                    int i = 0;
                    foreach (var s in queryShotReport)
                    {

                        worksheet.Cell(i + 2, 1).Value = s.shot.Speed;
                        worksheet.Cell(i + 2, 2).Value = s.players.Surname;
                        i++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Flush();

                        return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"Отчет {IdCategory + " c " + idStartS.ToString() + " по " +idStopS.ToString()}.xlsx"
                        };
                    }
                }
            }
            else if (IdCategory== "Турниры")
            {
                DateTime startD = Convert.ToDateTime(IdStartDate);
                DateTime stoptD = Convert.ToDateTime(IdStopDate);
                
               
                List<Tournament> tour= db.Tournament.ToList();
                List<Country> country = db.Country.ToList();

                var queryTourReport = from c in country
                                      join t in tour
                                      on c.ID_Country equals t.Country_info                                 
                                      where t.Date_Start >= startD && t.Date_Finish<=stoptD
                                      select new ExportModel
                                      {
                                         country =c,
                                         tour=t,
                                          startSpeed = idStartS,
                                          stopSpeed = idStopS,
                                          startDate = DateTime.Now.ToShortDateString(),
                                          stopDate = DateTime.Now.ToShortDateString(),
                                          category = "Турниры"
                                      };
                using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
                {
                    var worksheet = workbook.Worksheets.Add("Brands");

                    worksheet.Cell("A1").Value = "Название турнира";
                    worksheet.Cell("B1").Value = "Страна";
                    worksheet.Cell("C1").Value = "Даты проведения";
                    worksheet.Row(1).Style.Font.Bold = true;

                    //нумерация строк/столбцов начинается с индекса 1 (не 0)
                    int i = 0;
                    foreach (var s in queryTourReport)
                    {

                        worksheet.Cell(i + 2, 1).Value = s.tour.Name_Tournament;
                        worksheet.Cell(i + 2, 2).Value = s.country.Country_Name;
                        worksheet.Cell(i + 2, 3).Value = s.tour.Date_Start.ToShortDateString()+" - "+s.tour.Date_Finish.ToShortDateString();
                        i++;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Flush();

                        return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        {
                            FileDownloadName = $"Отчет {IdCategory + " c " + IdStartDate + " по " + IdStopDate}.xlsx"
                        };
                    }
                }
            }
            return

            RedirectToAction("Reports");
        }

        
    }
}
