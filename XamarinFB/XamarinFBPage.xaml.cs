using Xamarin.Forms;
using XamarinFB.Interface;

namespace XamarinFB
{
    public partial class XamarinFBPage : ContentPage
    {
        public XamarinFBPage()
        {
            InitializeComponent();

            this.BindingContext = new MainPageViewModel();

            MessagingCenter.Subscribe<Application, string>(this, "Celebrate", (sender, payload) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    gridEmpty.IsVisible = false;
                    emptyLabel.IsVisible = false;


                    gridSuccess.IsVisible = true;
                    successLabel.Text = $"Hola! {payload}";
                    successLabel.IsVisible = true;


                    labelProviderText.Text = payload;

                });
            });

            buttonContinue.Clicked += async (sender, args) =>
            {
                await Navigation.PushAsync(new VerifyPage());
            };
        }
    }
}
