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
    [Activity(Label = "RecyclerView")]
    public class RecyclerView : Activity
    {
        RecyclerView.LayoutManager mLayoutManager;
        RoomList mRoomlist;
        RoomPhotoAdapter mAdapter;
        RecyclerView recyclerView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.activity_main);
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            //recycler view
            mRoomlist = new RoomList();
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recycler1);
            mLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(mLayoutManager);
            mAdapter = new RoomPhotoAdapter(mRoomlist);
            mAdapter.ItemClick += MAdapter_ItemClick;

            recyclerView.SetAdapter(mAdapter);


        }
        private void MAdapter_ItemClick(object sender, int e)
        {
            Intent startA = new Intent(this, typeof(HouseActivity));

            startA.PutExtra("id", mRoomlist[e].roomDetails.Id);
            StartActivity(startA);
        }

        public string getQuotedString(string str)
        {
            return "\"" + str + "\"";
        }
        public async void Roomrent(Room room)
        {
            string url = "http://localhost:5000/api/RoomInfoes";
            var httpWebRequest = new HttpWebRequest(new Uri(url));
            httpWebRequest.ServerCertificateValidationCallback = delegate { return true; };
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{" +
                    getQuotedString("roomrent") + ":" + getQuotedString(room.roomrent) + "," +
                    getQuotedString("NumOfBedroom") + ":" + getQuotedString(room.NumOfBedroom.ToString()) + "," +
                    getQuotedString("NumOfBathroom") + ":" + getQuotedString(room.NumOfBathroom.ToString()) + "," +
                    getQuotedString("Agentid") + ":" + getQuotedString(room.Agentid.ToString()) +
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