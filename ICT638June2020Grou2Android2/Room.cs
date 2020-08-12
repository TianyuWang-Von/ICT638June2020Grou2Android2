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
    public class Room
    {
        public int Id { get; set; }
        public string roomrent { get; set; }
        public int NumOfBedroom { get; set; }
        public int NumOfBathroom { get; set; }
        public int Agentid { get; set; }


    }
}