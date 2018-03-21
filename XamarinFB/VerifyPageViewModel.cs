using System;
using Xamarin.Forms;
using XamarinFB.Interface;

namespace XamarinFB
{
    public class VerifyPageViewModel : BaseViewModel
    {
        IAuthService _authService;

        public VerifyPageViewModel()
        {
            var authService = DependencyService.Get<IAuthService>();
            if (authService != null)
            {
                _authService = authService;
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private string _verificationCode;
        public string VerificationCode
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private Command _sendCode;
        public Command SendCodeCommand
        {
            get => _sendCode ?? (_sendCode = new Command(() =>
            {
                _authService.SendVerificationCode(PhoneNumber);
            }));
        }


        private Command verify;
        public Command VerifyCommand
        {
            get => verify ?? (verify = new Command(() =>  _authService.VerifyPhoneNumber(VerificationCode)));
        }
    }
}
