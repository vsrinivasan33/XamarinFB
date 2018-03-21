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
                Firebase.Core.Options.DefaultInstance.DeepLinkUrlScheme = Constants.FirebaseDeepLinkName;
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
            Console.WriteLine($"OpenUrl Link {url.AbsoluteString}");

            var dynamicLink = DynamicLinks.SharedInstance?.FromCustomSchemeUrl(url);

            if (dynamicLink == null)
                return false;
            var link = dynamicLink.Url?.AbsoluteString.Split(new string[] { Constants.FirebaseDeepLinkUrl }, StringSplitOptions.None).LastOrDefault();
            if (!string.IsNullOrWhiteSpace(link))
            {
                var welcomeString = link.Split(new char[] { '&' }).FirstOrDefault();
                Xamarin.Forms.MessagingCenter.Send(Xamarin.Forms.Application.Current, Constants.Welcome, welcomeString);
            }           
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

                var link = dynamicLink.Url.AbsoluteString.Split(new string[] { Constants.FirebaseDeepLinkUrl }, StringSplitOptions.None).LastOrDefault();
                var provider = link.Split(new char[] { '&' }).FirstOrDefault();

                Xamarin.Forms.MessagingCenter.Send(Xamarin.Forms.Application.Current, Constants.Welcome, provider);
                Console.WriteLine("{0}]", string.Join(", ", link));               
            });
        } 
    }
}
