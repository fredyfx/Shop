namespace Shop.UIClassic.iOS
{
    using System;
    using Common.Models;
    using Common.Services;
    using UIKit;

    public partial class ViewController : UIViewController
    {
        private ApiService apiService;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.apiService = new ApiService();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void LoginButton_TouchUpInside(UIButton sender)
        {
            if (string.IsNullOrEmpty(this.EmailText.Text))
            {
                var alert = UIAlertController.Create("Error", "You must enter an email.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
                return;
            }

            if (string.IsNullOrEmpty(this.PasswordText.Text))
            {
                var alert = UIAlertController.Create("Error", "You must enter a password.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
                return;
            }

            this.DoLogin();
        }

        private async void DoLogin()
        {
            this.ActivityIndicator.StartAnimating();
            var request = new TokenRequest
            {
                Username = this.EmailText.Text,
                Password = this.PasswordText.Text
            };

            var response = await this.apiService.GetTokenAsync(
                "https://shopzulu.azurewebsites.net",
                "/Account",
                "/CreateToken",
                request);

            if (!response.IsSuccess)
            {
                this.ActivityIndicator.StopAnimating();
                var alert = UIAlertController.Create("Error", "User or password incorrect.", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
                this.PresentViewController(alert, true, null);
                return;
            }

            var token = (TokenResponse)response.Result;
            this.ActivityIndicator.StopAnimating();
            var ok = UIAlertController.Create("Ok", "Fuck yeah!", UIAlertControllerStyle.Alert);
            ok.AddAction(UIAlertAction.Create("Accept", UIAlertActionStyle.Default, null));
            this.PresentViewController(ok, true, null);
        }
    }
}