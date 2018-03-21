using System;
using System.Threading.Tasks;
using Firebase.Auth;
using XamarinFB.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(AuthService))]
namespace XamarinFB.iOS.Services
{
    public class AuthService : XamarinFB.Services.BaseAuthService
    {
        
        public override async Task SendVerificationCode(string phoneNumber)
        {
            await Task.Run(() =>
            {
                PhoneAuthProvider.DefaultInstance.VerifyPhoneNumber(phoneNumber, null, (verificationId, error) =>
                {
                    if (error != null)
                    {
                        Console.WriteLine(error.LocalizedDescription);
                        VerificationError = error.LocalizedDescription;
                        VerificationCompleted((false, VerificationError));
                        return;
                    }
                    VerificationId = verificationId;
                });
            });
        }

        public override async Task VerifyPhoneNumber(string verificationCode)
        {
            VerificationError = string.Empty;
            await Task.Run(() =>
            {
                var credential = PhoneAuthProvider.DefaultInstance.GetCredential(this.VerificationId, verificationCode);
                {
                    Auth.DefaultInstance.SignIn(credential, (user, error) =>
                    {
                        if (error != null)
                        {                    
                            VerificationError = error.LocalizedDescription;
                            return;
                        }
                        GetIdToken(user);
                    });
                }
            });
        }

        private void GetIdToken(Firebase.Auth.User firUser)
        {
            if (firUser == null)
                return;

            firUser.GetIdToken((string token, Foundation.NSError idError) =>
            {               
                if (idError != null)
                {               
                    VerificationError = idError.LocalizedDescription;
                    VerificationCompleted((false, VerificationError));
                    return;
                }
                //Identity.IdToken = token;
                //SUccess
                VerificationCompleted((true, token));
            });
        }
    }
}
