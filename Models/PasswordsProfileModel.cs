using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PraktikaWeb.Models
{
    public class PasswordsProfileModel
    {
        public Passwords password { get; set; }
        public Person_Profile profile { get; set; }
    }
}