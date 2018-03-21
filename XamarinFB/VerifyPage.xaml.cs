
using Xamarin.Forms;

namespace XamarinFB
{
    public partial class VerifyPage : ContentPage
    {
        public VerifyPage()
        {
            InitializeComponent();
            this.BindingContext = new VerifyPageViewModel();
        }
    }
}
