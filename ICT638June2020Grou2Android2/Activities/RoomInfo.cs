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

namespace ICT638June2020Grou2Android2.Activities
{
    [Activity(Label = "RoomInfo")]
    public class RoomInfo : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RoomDetail);
            // Create your application here
            Button btnRoomDetail = FindViewById<Button>(Resource.Id.btnRoom);
            btnRoomDetail.Click += BtnRoomDetail_Click;
        }

        private void BtnRoomDetail_Click(object sender, EventArgs e)
        {
            Intent startA = new Intent(this, typeof(MainActivity));
            StartActivity(startA);
        }
    }
}