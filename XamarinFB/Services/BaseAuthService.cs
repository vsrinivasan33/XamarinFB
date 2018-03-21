using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinFB.Interface;

namespace XamarinFB.Services
{
    public abstract class BaseAuthService : IAuthService
    {       
        protected SemaphoreSlim _accessTokenSemaphore = new SemaphoreSlim(1, 1);

        public string VerificationId { get; set; }

        public string VerificationError { get; set; }

        public abstract Task SendVerificationCode(string phoneNumber);

        public abstract Task VerifyPhoneNumber(string verificationCode);

        protected void VerificationCompleted((bool, string) result)
        {
            MessagingCenter.Send<IAuthService, (bool, string)>(this, "VerificationComplete", result);
        }
    }
}