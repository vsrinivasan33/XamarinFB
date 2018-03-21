using System;
using System.Linq;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase.DynamicLinks;
using Xamarin.Forms;

namespace XamarinFB.Droid
{
    [Activity(Label = "XamarinFB.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();
            FirebaseDynamicLinks.Instance.GetDynamicLink(Intent)
                                    .AddOnSuccessListener(this, new AndroidActionSuccessListener())
                                  .AddOnFailureListener(this, new AndroidActionFailureListener());
        }


        internal class AndroidActionSuccessListener : Java.Lang.Object, Android.Gms.Tasks.IOnSuccessListener
        {
            public void OnSuccess(Java.Lang.Object result)
            {
                Console.WriteLine(result);

                if (result is PendingDynamicLinkData)
                {
                    var dynamicLink = result as PendingDynamicLinkData;

                    var link = dynamicLink.Link.ToString().Split(new string[] { "https://vinod.com.au?welcome=" }, StringSplitOptions.None).LastOrDefault();
                    var welcomeMsg = link.Split(new char[] { '&' }).FirstOrDefault();
                    MessagingCenter.Send(Xamarin.Forms.Application.Current, "Celebrate", welcomeMsg);
                }
            }
        }

        internal class AndroidActionFailureListener : Java.Lang.Object, Android.Gms.Tasks.IOnFailureListener
        {
            public void OnFailure(Java.Lang.Exception e)
            {
                Console.WriteLine(this.Class.Name, e, $" [{DateTime.Now}] - [AndroidAppLinks Failure] - {e.Message}");
                throw e;
            }
        }
    }
}
