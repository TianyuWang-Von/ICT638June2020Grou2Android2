using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace ICT638June2020Grou2Android2.Activities
{
    [Activity(Label = "AgentActivity")]
    public class AgentActivity : Activity,IOnMapReadyCallback
    {
        private EditText edName, edMail, edPhone;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.agnet_layout);
            base.OnCreate(savedInstanceState);

            //Get Data and Show on Agent Page
            edName = FindViewById<EditText>(Resource.Id.edAgName);
            edMail = FindViewById<EditText>(Resource.Id.edAgMail);
            edPhone = FindViewById<EditText>(Resource.Id.edAgPhone);

            string url = "https://10.0.2.2:5001/api/Agents/1";
            string result123 = "";
            var httpWebRequest = new HttpWebRequest(new Uri(url));
            httpWebRequest.ServerCertificateValidationCallback = delegate { return true; };
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";

            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result123 = reader.ReadToEnd();
            }

            Agent agent = new Agent();
            agent = Newtonsoft.Json.JsonConvert.DeserializeObject<Agent>(result123);

            edName.Text = agent.Name;
            edMail.Text = agent.Email;
            edPhone.Text = agent.Phonenumber;

            //Map Feature Start
            var mapFrag = MapFragment.NewInstance();
            this.FragmentManager.BeginTransaction()
                                    .Add(Resource.Id.mapContainer, mapFrag, "map_fragment")
                                    .Commit();
            mapFrag.GetMapAsync(this);

            //Agent Page Share and Message Button 
            Button btn_Share = FindViewById<Button>(Resource.Id.btnAgShare);
            btn_Share.Click += Btn_Share_Click;

            Button btn_Message = FindViewById<Button>(Resource.Id.btnAgMail);
            btn_Message.Click += Btn_Message_Click;
        }

        //Map Feature
        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;



            getCurrentLoc(googleMap);
        }

        public async void getCurrentLoc(GoogleMap gMap)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);



                if (location != null)
                {
                    MarkerOptions curLocMarker = new MarkerOptions();
                    curLocMarker.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    curLocMarker.SetTitle("I am here");
                    curLocMarker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueGreen));
                    gMap.AddMarker(curLocMarker);



                    //Code to Zoom at Location
                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(new LatLng(location.Latitude, location.Longitude));
                    builder.Zoom(18);
                    builder.Bearing(155);
                    builder.Tilt(65);



                    var address = await Geocoding.GetPlacemarksAsync(location);
                    var placemark = address?.FirstOrDefault();
                    var geocodeAddress = "";
                    if (placemark != null)
                    {
                        geocodeAddress =
                            $"AdminArea:       {placemark.AdminArea}\n" +
                            $"CountryCode:     {placemark.CountryCode}\n" +
                            $"CountryName:     {placemark.CountryName}\n" +
                            $"FeatureName:     {placemark.FeatureName}\n" +
                            $"Locality:        {placemark.Locality}\n" +
                            $"PostalCode:      {placemark.PostalCode}\n" +
                            $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                            $"SubLocality:     {placemark.SubLocality}\n" +
                            $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                            $"Thoroughfare:    {placemark.Thoroughfare}\n";
                    }



                    Toast.MakeText(this, geocodeAddress, ToastLength.Long);
                }
                else
                {
                    GetLastLocation(gMap);
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "Exception encountered", ToastLength.Long);
                GetLastLocation(gMap);
            }
        }

        public async void GetLastLocation(GoogleMap gMap)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();



                if (location != null)
                {
                    MarkerOptions curLocMarker = new MarkerOptions();
                    curLocMarker.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    curLocMarker.SetTitle("Last Known Location");
                    curLocMarker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueMagenta));
                    gMap.AddMarker(curLocMarker);
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "Exception encountered", ToastLength.Long);
            }
        }

        //Message and Share Feature
        private async void Btn_Message_Click(object sender, EventArgs e)
        {
            //Message Feature
            string messageText = "";
            string recipient = edPhone.Text;
            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private async void Btn_Share_Click(object sender, EventArgs e)
        {
            //Share Feature
            string text = "Name: " + edName.Text + "\r\n" +
                "Email: " + edMail.Text + "\r\n" +
                "Phone Number: " + edPhone.Text;

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Text"
            });
        }
    }
}