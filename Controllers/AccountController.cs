using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PraktikaWeb.Models;

namespace PraktikaWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(PasswordsProfileModel model)
        {
            model.profile.Email_Profile = model.password.Email;
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Passwords user = null;
                using (DBTennisContext db = new DBTennisContext())
                {
                    user = db.Passwords.FirstOrDefault(u => u.Email == model.password.Email && u.Password == model.password.Password);

                }
                if (user != null)
                {
                    ModelState.AddModelError("", "Пользователь с таким логином и паролем уже есть");
                    return View(model);
                }
                else
                {
                    DBTennisContext db = new DBTennisContext();
                    using (db = new DBTennisContext())
                    {
                        Passwords p = new Passwords
                        {
                            Email = model.password.Email,
                            Password = model.password.Password
                        };
                        db.Entry(p).State = EntityState.Added;

                        Person_Profile pp = new Person_Profile
                        {
                            Name_Profile = model.profile.Name_Profile,
                            Surname_Profile = model.profile.Surname_Profile,
                            Email_Profile = model.password.Email
                        };
                        db.Entry(pp).State = EntityState.Added;
                        db.SaveChanges();
                    }
                    FormsAuthentication.SetAuthCookie(model.profile.Name_Profile, true);
                    return RedirectToAction("Index", "HomeAnalitic");

                }
            }
            ModelState.AddModelError("", "Данные введены некорректно");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Passwords model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Passwords user = null;
                using (DBTennisContext db = new DBTennisContext())
                {
                    user = db.Passwords.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                }
                if (user != null)
                {
                    Person_Profile client = null;
                    using (DBTennisContext db = new DBTennisContext())
                    {
                        client = db.Person_Profile.FirstOrDefault(u => u.Email_Profile == model.Email);

                    }
                    if (client != null)
                    {
                        string clientName = client.Name_Profile;
                        FormsAuthentication.SetAuthCookie(clientName, true);
                        return RedirectToAction("Index", "HomeAnalitic");
                    }
                    else
                    {                       
                            FormsAuthentication.SetAuthCookie(model.Email, true);
                            return RedirectToAction("Index", "HomeManager");                      
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            ModelState.AddModelError("", "Данные введены некорректно");
            return View(model);
        }
    }
}