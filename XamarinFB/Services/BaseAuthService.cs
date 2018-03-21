using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinFB.Interface;

namespace XamarinFB.Services
{
    public abstract class BaseAuthService : IAuthService
    {   
        public string VerificationId { get; set; }

        public string VerificationError { get; set; }

        public abstract Task SendVerificationCode(string phoneNumber);

        public abstract Task VerifyPhoneNumber(string verificationCode);

        protected void VerificationCompleted((bool, string) result)
        {
            //Pass the Message to anyone who is interested
            MessagingCenter.Send<IAuthService, (bool, string)>(this, Constants.VerificationComplete, result);
        }
    }
}