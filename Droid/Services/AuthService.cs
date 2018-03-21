using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Plugin.CurrentActivity;
using XamarinFB.Droid.Services;
using static Firebase.Auth.PhoneAuthProvider;

[assembly: Xamarin.Forms.Dependency(typeof(AuthService))]
namespace XamarinFB.Droid.Services
{
    public class AuthService : XamarinFB.Services.BaseAuthService
    {
        VerificationCallBack verificationStateChangedCallback = new VerificationCallBack();

        public override async Task SendVerificationCode(string phoneNumber)
        {
            await Task.Run(() =>
            {
                PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, 60, Java.Util.Concurrent.TimeUnit.Seconds, CrossCurrentActivity.Current.Activity, verificationStateChangedCallback);
            });
        }

        public override async Task VerifyPhoneNumber(string verificationId, string verificationCode)
        {
            //Credential credential = phoneAuthProvider.GetCredential(verificationId, verificationCode);

        }
    }

    internal class VerificationCallBack : OnVerificationStateChangedCallbacks
    {
		public override void OnCodeSent(string verificationId, ForceResendingToken forceResendingToken)
		{
            base.OnCodeSent(verificationId, forceResendingToken);
		}

		public override void OnCodeAutoRetrievalTimeOut(string verificationId)
		{
            base.OnCodeAutoRetrievalTimeOut(verificationId);
		}

		public async override void OnVerificationCompleted(PhoneAuthCredential credential)
        {
            //Convert anonymous user to new user
            if (String.IsNullOrEmpty(FirebaseAuth.Instance.CurrentUser.PhoneNumber))
            {
                //await FirebaseAuth.Instance.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task =>
                //{
                //    if (task.IsCanceled)
                //    {
                //        Debug.WriteLine("LinkWithCredentialAsync was canceled.");
                //        return;
                //    }
                //    if (task.IsFaulted)
                //    {
                //        Debug.WriteLine("LinkWithCredentialAsync encountered an error: " + task.Exception);
                //        return;
                //    }

                //    Firebase.Auth.FirebaseUser newUser = (Firebase.Auth.FirebaseUser)task.Result;
                //    Debug.WriteLine("Credentials successfully linked to Firebase user: {0} ({1})",
                //                    newUser.DisplayName, newUser.Uid);
                //});

            }
            else
            {
                var authResult = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);
                if (authResult != null)
                {
                    var tokenResult = await authResult.User.GetIdTokenAsync(true);
                    Debug.WriteLine(tokenResult.Token);
                }
            }
        }

        public override void OnVerificationFailed(FirebaseException exception)
        {
            Debug.WriteLine(exception);
        }    
    }
}
