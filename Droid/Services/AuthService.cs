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
        public AuthService()
        {
            verificationStateChangedCallback.OnVerificationCompletedEvent += (bool status, string token) =>
            {
                VerificationCompleted((status, token));
            };
        }
        public override async Task SendVerificationCode(string phoneNumber)
        {
            await Task.Run(() =>
            {
                PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, 60, Java.Util.Concurrent.TimeUnit.Seconds, CrossCurrentActivity.Current.Activity, verificationStateChangedCallback);
            });
        }

        public override async Task VerifyPhoneNumber(string verificationCode)
        {
            //NOT required for latest Android as the Verification will be handled automatically
        }

    }
    internal class VerificationCallBack : OnVerificationStateChangedCallbacks
    {
        public delegate void VerificationCompletedHandler(bool status, string token);

        public event VerificationCompletedHandler OnVerificationCompletedEvent;

        string _verificationId = string.Empty;
        public override void OnCodeSent(string verificationId, ForceResendingToken forceResendingToken)
        {
            base.OnCodeSent(verificationId, forceResendingToken);
            _verificationId = verificationId;
        }

        public override void OnCodeAutoRetrievalTimeOut(string verificationId)
        {
            base.OnCodeAutoRetrievalTimeOut(verificationId);

        }

        public async override void OnVerificationCompleted(PhoneAuthCredential credential)
        {
            var authResult = await FirebaseAuth.Instance.SignInWithCredentialAsync(credential);
            if (authResult != null)
            {
                var tokenResult = await authResult.User.GetIdTokenAsync(true);
                Debug.WriteLine(tokenResult.Token);
                if(OnVerificationCompletedEvent != null)
                    OnVerificationCompletedEvent(true, tokenResult.Token);
            }
        }

        public override void OnVerificationFailed(FirebaseException exception)
        {
            Debug.WriteLine(exception);
            if (OnVerificationCompletedEvent != null)
                OnVerificationCompletedEvent(false, exception.Message);
        }
    }
}