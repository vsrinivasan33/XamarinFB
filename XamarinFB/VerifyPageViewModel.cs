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

            MessagingCenter.Subscribe<IAuthService, (bool Status, string Result)>(this, Constants.VerificationComplete, (sender, payload) =>
            {               
                Result = payload.Result;
                MessagingCenter.Unsubscribe<IAuthService, (bool, string)>(this, Constants.VerificationComplete);
            });

           PhoneNumber = Device.RuntimePlatform == Device.iOS ? "+61433286080" : "+61459862793";

        }

        private string _result;
        public string Result
        {
            get => _result;
            set {
                _result = value;
                OnPropertyChanged();
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
            get => _verificationCode;
            set
            {
                _verificationCode = value;
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
