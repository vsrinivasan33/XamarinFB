
using System;
using System.Linq;
using Firebase.DynamicLinks;
using Foundation;
using Lottie.Forms.iOS.Renderers;
using UIKit;

namespace XamarinFB.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            AnimationViewRenderer.Init();
            if (Firebase.Core.Options.DefaultInstance != null)
            {
                Firebase.Core.Options.DefaultInstance.DeepLinkUrlScheme = "xamarinfb";
                Firebase.Core.App.Configure();
            } 
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            return base.FinishedLaunching(app, options);
        }

        // Handle Custom Url Schemes for iOS 9 or newer
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return OpenUrl(app, url, null, null);
        }

        // Handle Custom Url Schemes for iOS 8 or older
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            Console.WriteLine("I'm handling a link through the OpenUrl method.");

            var dynamicLink = DynamicLinks.SharedInstance?.FromCustomSchemeUrl(url);

            if (dynamicLink == null)
                return false;
            var link = dynamicLink.Url?.AbsoluteString.Split(new string[] { "https://vinod.com.au?welcome=" }, StringSplitOptions.None).LastOrDefault();
            if (!string.IsNullOrWhiteSpace(link))
            {
                var welcomeString = link.Split(new char[] { '&' }).FirstOrDefault();
                Xamarin.Forms.MessagingCenter.Send(Xamarin.Forms.Application.Current, "Celebrate", welcomeString);
            }
            //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("New App - Dynamic Link", provider, "OK");
            // Handle the deep link. For example, show the deep-linked content or
            // apply a promotional offer to the user's account.
            return true;
        } 

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            return DynamicLinks.SharedInstance.HandleUniversalLink(userActivity.WebPageUrl, (dynamicLink, error) => {
                if (error != null)
                {
                    System.Console.WriteLine(error.LocalizedDescription);
                    return;
                }

                var link = dynamicLink.Url.AbsoluteString.Split(new string[] { "https://healthnow.io?" }, StringSplitOptions.None).LastOrDefault();
                var provider = link.Split(new char[] { '&' }).FirstOrDefault();

                Xamarin.Forms.MessagingCenter.Send(Xamarin.Forms.Application.Current, "Celebrate", provider);
                //Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Dynamic Link Received", provider, "OK");

                Console.WriteLine("{0}]", string.Join(", ", link));
                // Handle Universal Link
            });
        } 
    }
}
