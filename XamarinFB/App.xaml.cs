using System;
using System.Linq;
using Xamarin.Forms;

namespace XamarinFB
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new XamarinFBPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
            var link = uri.AbsoluteUri.Split(new string[] { "https://vinod.com.au/?" }, StringSplitOptions.None).LastOrDefault();
            if (!string.IsNullOrWhiteSpace(link))
            {
                var provider = link.Split(new char[] { '&' }).FirstOrDefault();
                //MessagingCenter.Send(Current, "Celebrate", provider);
            }
        } 
    }
}
