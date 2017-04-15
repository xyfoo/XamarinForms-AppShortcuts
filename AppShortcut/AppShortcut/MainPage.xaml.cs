using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppShortcut
{
    public partial class MainPage : TabbedPage
    {
        /// <summary>
        /// Hold the flight number to display when the app appears
        /// </summary>
        string pendingFlightStatusToDisplay_FlightNumber = string.Empty;

        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string, string>(string.Empty, "DroidAppShortcutInvoked", DroidAppShortcutInvokedHandler);
        }

        /// <summary>
        /// Handle the invocation from the App's shortcut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="intentDataLastPathSegment"></param>
        private void DroidAppShortcutInvokedHandler(string sender, string intentDataLastPathSegment)
        {
            if (intentDataLastPathSegment == "bookings")
            {
                // Switch to bookings tab
                CurrentPage = Children[1];
            }
            else if (intentDataLastPathSegment == "customerservice")
            {
                // Switch to customer service tab
                CurrentPage = Children[2];
            }
            else if (intentDataLastPathSegment.StartsWith("flight-"))
            {
                // Show a pop up of the flight status

                // Extract the "flight-" prefix from "flight-abc123";
                var flightNumber = intentDataLastPathSegment.Remove(0, 7);
                pendingFlightStatusToDisplay_FlightNumber = flightNumber; 
            }
        }

        private async void AddRandomFlightShortcut_Clicked(object sender, EventArgs e)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            string randomFlightNumber = "SA" + r.Next(100, 999);

            MessagingCenter.Send(string.Empty, "CreateFlightShortcut", randomFlightNumber);

            await DisplayAlert("Flight Shortcut Created!", $"Long press on the app icon, and you will see \"Flight {randomFlightNumber}\" shortcut created", "OK");
        }

        private void ClearCreatedDynamicShortcut_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(string.Empty, "ClearDynamicShortcuts");
        }

        protected override void OnAppearing()
        {
            // Show the flight information, if there's any 
            if (!string.IsNullOrWhiteSpace(pendingFlightStatusToDisplay_FlightNumber))
            {
                DisplayAlert($"Flight {pendingFlightStatusToDisplay_FlightNumber} Status", "Status: On-time\nCheck in counter: D5", "OK");
                pendingFlightStatusToDisplay_FlightNumber = string.Empty;
            }
        }
    }
}


