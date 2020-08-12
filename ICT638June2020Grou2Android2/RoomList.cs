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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ICT638June2020Grou2Android2
{
    public class RoomPhoto
    {
        public int mPhotoID { get; set; }
        public Room roomDetails = new Room();
    }
    public class RoomList
    {
        static int[] pictures = { Resource.Drawable.Room1, Resource.Drawable.Room2, Resource.Drawable.Room3, Resource.Drawable.Room4, Resource.Drawable.Room5 };
        private List<RoomPhoto> rooms = new List<RoomPhoto>();
        public RoomList()
        {
            getRooms();
        }

        public void getRooms()
        {
            string url = "http://10.0.2.2:5000/api/RoomInfoes";
            var httpWebRequest = new HttpWebRequest(new Uri(url));
            httpWebRequest.ServerCertificateValidationCallback = delegate { return true; };
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";



            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                var list = JsonConvert.DeserializeObject<List<Room>>(result);
                int i = 0;
                foreach (Object l in list)
                {
                    rooms.Add(new RoomPhoto() { mPhotoID = pictures[i % 5], roomDetails = (Room)l });
                    i++;
                }
            }
        }
        public int numPhoto
        {
            get
            {
                return rooms.Count;
            }
        }
        public RoomPhoto this[int i]
        {
            get { return rooms[i]; }
        }
    }
    public class PhotoViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; set; }
        public TextView Caption { get; set; }

        [Obsolete]
        public PhotoViewHolder(View itemview, Action<int> listener) : base(itemview)
        {
            Image = itemview.FindViewById<ImageView>(Resource.Id.imageView);
            Caption = itemview.FindViewById<TextView>(Resource.Id.textView);
            itemview.Click += (sender, e) => listener(Position);
        }
    }
}