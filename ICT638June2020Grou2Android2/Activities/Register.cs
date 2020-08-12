using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ICT638June2020Grou2Android2.Activities
{
    [Activity(Label = "Register")]
    public class Register : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.register_layout);
            base.OnCreate(savedInstanceState);
            Button btnRegister = FindViewById<Button>(Resource.Id.btnRegisterUser);
            btnRegister.Click += BtnRegister_Click;

            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += BtnLogin_Click;

            // Create your application here
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Login));
        }

        public string getQuotedString(string str)
        {
            return "\"" + str + "\"";
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            User user = new User();
            user.fname = FindViewById<EditText>(Resource.Id.edUserFName).Text;
            user.lname = FindViewById<EditText>(Resource.Id.edUserLName).Text;
            user.phonenumber = FindViewById<EditText>(Resource.Id.edUserPnumber).Text;
            user.address = FindViewById<EditText>(Resource.Id.edUserAddress).Text;
            user.country = FindViewById<EditText>(Resource.Id.edUsercoun).Text;
            user.uniquename = FindViewById<EditText>(Resource.Id.edUseruniq).Text;
            user.password = FindViewById<EditText>(Resource.Id.edUserpass).Text;



            string url = "https://10.0.2.2:5001/api/Registers";
            var httpWebRequest = new HttpWebRequest(new Uri(url));
            //var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ServerCertificateValidationCallback = delegate { return true; };
            //httpWebRequest.ServerCertificateCustomValidationCallback = delegate { return true; }
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{" +
                    getQuotedString("fname") + ":" + getQuotedString(user.fname) + "," +
                    getQuotedString("lname") + ":" + getQuotedString(user.lname) + "," +
                    getQuotedString("phonenumber") + ":" + getQuotedString(user.phonenumber) + "," +
                    getQuotedString("address") + ":" + getQuotedString(user.address) + "," +
                    getQuotedString("country") + ":" + getQuotedString(user.country) + "," +
                    getQuotedString("uniquename") + ":" + getQuotedString(user.uniquename) + "," +
                    getQuotedString("password") + ":" + getQuotedString(user.password) +
                               "}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}