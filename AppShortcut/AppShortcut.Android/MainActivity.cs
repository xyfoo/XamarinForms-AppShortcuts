using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;

namespace AppShortcut.Droid
{
    [Activity(Label = "Sample Airlines",
        Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Exported= true,
        Name = "com.example.appshortcut.MainActivity")]
    [MetaData("android.app.shortcuts", Resource="@xml/shortcuts")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // Get the last path segment fron Intent.Data if available
            // Sample data: content://people/1
            LoadApplication(new App(Intent?.Data?.LastPathSegment));

            // Subscribe to CreateFlightShortcut message
            MessagingCenter.Subscribe<string, string>(string.Empty, "CreateFlightShortcut", CreateFlightShortcut);

            // Subscribe to ClearDynamicShortcuts message
            MessagingCenter.Subscribe<string>(string.Empty, "ClearDynamicShortcuts", ClearDynamicShortcuts);
        }

        /// <summary>
        /// Create a dynamic shortcut that will open the app and display the relevant info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="flightNumber"></param>
        public void CreateFlightShortcut(object sender, string flightNumber)
        {
            // Create a shortcut builder
            var shortcutBuilder = new ShortcutInfo.Builder(this, flightNumber);

            // Set the shortcut info
            shortcutBuilder.SetIcon(Android.Graphics.Drawables.Icon.CreateWithResource(this, Resource.Drawable.flight));
            shortcutBuilder.SetShortLabel($"Flight {flightNumber}");
            shortcutBuilder.SetLongLabel($"Flight {flightNumber}");
            Intent openFlightInfoIntent = new Intent(
                Intent.ActionView,                                                      // Action
                Android.Net.Uri.Parse($"content:////actions//flight-{flightNumber}"),   // Intent data
                this,                                                                   // Context
                typeof(MainActivity));                                                  // Activity class
            shortcutBuilder.SetIntent(openFlightInfoIntent);
            
            // Build the shortcut
            ShortcutInfo myFlightShortcut = shortcutBuilder.Build();

            // Get the shortcut manager
            ShortcutManager shortcutManager = (ShortcutManager)this.GetSystemService(Java.Lang.Class.FromType(typeof(ShortcutManager)));

            // Set the list of shortcuts as the App's shortcut
            shortcutManager.SetDynamicShortcuts(new System.Collections.Generic.List<ShortcutInfo>() { myFlightShortcut });
        }

        private void ClearDynamicShortcuts(string sender)
        {
            // Get the shortcut manager
            ShortcutManager shortcutManager = (ShortcutManager)this.GetSystemService(Java.Lang.Class.FromType(typeof(ShortcutManager)));

            // Remove all dynamic shortcuts
            shortcutManager.RemoveAllDynamicShortcuts();
        }
    }
}



