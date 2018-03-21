using System;
using System.Threading.Tasks;

namespace XamarinFB.Interface
{
    public interface IAuthService
    {
        string VerificationId { get; set; }

        string VerificationError { get; set; }

         Task SendVerificationCode(string phoneNumber);

         Task VerifyPhoneNumber(string verificationCode);

    }
}
