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
using Xamarin.Essentials;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using System.Net;
using System.IO;

namespace ICT638June2020Grou2Android2.Activities
{
    [Activity(Label = "View")]
    public class View : Activity,IOnMapReadyCallback
    {
        private EditText edName, edPassword, edEmail, edphon;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetContentView(Resource.Layout.user_layout);
            base.OnCreate(savedInstanceState);

            //Get Api
            edName = FindViewById<EditText>(Resource.Id.edUsername);
            edPassword = FindViewById<EditText>(Resource.Id.edUserpassword);
            edEmail = FindViewById<EditText>(Resource.Id.edUseremail);
            edphon = FindViewById<EditText>(Resource.Id.edUserphon);


            string url = "https://10.0.2.2:5001/api/Registers/1";
            string login7 = "";
            var httpWebRequest = new HttpWebRequest(new Uri(url));
            httpWebRequest.ServerCertificateValidationCallback = delegate { return true; };
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";



            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                login7 = reader.ReadToEnd();
            }


            User user = new User();
            user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(login7);

            edName.Text = user.fname;
            edPassword.Text = user.password;
            edEmail.Text = "email";
            edphon.Text = user.phonenumber;

            //Map
            var mapFragment = MapFragment.NewInstance();
            this.FragmentManager.BeginTransaction()
            .Add(Resource.Id.map, mapFragment, "map_fragment")
            .Commit();
            mapFragment.GetMapAsync(this);

            //Share and Send
            Button btnshare = FindViewById<Button>(Resource.Id.btnblue);
            btnshare.Click += Btnshare_Click;

            Button btnsend = FindViewById<Button>(Resource.Id.btnsend);
            btnsend.Click += Btnsend_Click;

            //Link the Roomlist
            Button btnRPhoto = FindViewById<Button>(Resource.Id.btnRoomPhoto1);
            btnRPhoto.Click += BtnRPhoto_Click;
        }

        private void BtnRPhoto_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Recycler_View));
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            googleMap.MapType = GoogleMap.MapTypeNormal;

            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;

            getCurrentLoc(googleMap);

        }
        public async void getLastLocation(GoogleMap googleMap)
        {
            Console.WriteLine("Test - LastLoc");
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    Console.WriteLine($"Last Loc - Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    MarkerOptions curLoc = new MarkerOptions();
                    curLoc.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    curLoc.SetTitle("You were here");
                    curLoc.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure));

                    googleMap.AddMarker(curLoc);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Toast.MakeText(this, "Feature Not Supported", ToastLength.Short);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Toast.MakeText(this, "Feature Not Enabled", ToastLength.Short);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Toast.MakeText(this, "Needs more permission", ToastLength.Short);
            }
            catch (Exception ex)
            {
                // Unable to get location
                Toast.MakeText(this, "Unable to get location", ToastLength.Short);
            }
        }

        public async void getCurrentLoc(GoogleMap googleMap)
        {
            Console.WriteLine("Test - CurrentLoc");
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"current Loc - Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    MarkerOptions curLoc = new MarkerOptions();
                    curLoc.SetPosition(new LatLng(location.Latitude, location.Longitude));
                    curLoc.SetTitle("You are here");
                    curLoc.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueAzure));

                    googleMap.AddMarker(curLoc);
                    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                    builder.Target(new LatLng(location.Latitude, location.Longitude));
                    builder.Zoom(18);
                    builder.Bearing(155);
                    builder.Tilt(65);

                    CameraPosition cameraPosition = builder.Build();

                    CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

                    googleMap.MoveCamera(cameraUpdate);
                }
                else
                {
                    getLastLocation(googleMap);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Toast.MakeText(this, "Feature Not Supported", ToastLength.Short);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Toast.MakeText(this, "Feature Not Enabled", ToastLength.Short);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Toast.MakeText(this, "Needs more permission", ToastLength.Short);
            }
            catch (Exception ex)
            {
                getLastLocation(googleMap);
            }
        }




        private async void Btnsend_Click(object sender, EventArgs e)
        {
            string messageText = "";
            string recipient = edphon.Text;
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

        private async void Btnshare_Click(object sender, EventArgs e)
        {
            string text = "Name: " + edName.Text + "\r\n" +
                edPassword.Text + edEmail.Text;
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Text"
            });
        }
    }
}