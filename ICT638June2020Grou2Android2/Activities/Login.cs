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
    [Activity(Label = "Login")]
    public class Login : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.login_layout);
            base.OnCreate(savedInstanceState);
            Button btnLogin = FindViewById<Button>(Resource.Id.btnLoginUser);
            btnLogin.Click += BtnLogin_Click;

            Button btnregister = FindViewById<Button>(Resource.Id.btnGoRegister);
            btnregister.Click += Btnregister_Click;
        }

        private void Btnregister_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Register));
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            EditText eduseraccount = FindViewById<EditText>(Resource.Id.edUserAccount);
            EditText eduserpassword = FindViewById<EditText>(Resource.Id.edUserPassword);

            string url = "https://10.0.2.2:5001/api/Registers/1";
            string login77 = "";
            var httpWebRequest = new HttpWebRequest(new Uri(url));
            httpWebRequest.ServerCertificateValidationCallback = delegate { return true; };
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";



            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                login77 = reader.ReadToEnd();
            }


            User user = new User();
            user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(login77);

            if (eduseraccount.Text == user.uniquename && eduserpassword.Text == user.password)
            {
                StartActivity(typeof(View));
            }
        }
    }
}