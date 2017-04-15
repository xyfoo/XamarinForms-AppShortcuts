using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AppShortcut
{
    public partial class App : Application
    {
        public App(string androidIntentDataLastPathSegment = null)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            if (!string.IsNullOrWhiteSpace(androidIntentDataLastPathSegment))
            {
                MessagingCenter.Send(string.Empty, "DroidAppShortcutInvoked", androidIntentDataLastPathSegment);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

