using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ICT638June2020Grou2Android2
{
    public class User
    {
        public int id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string phonenumber { get; set; }
        public string address { get; set; }
        public string country { get; set; }
        public string uniquename { get; set; }
        public string password { get; set; }
    }
}