﻿using Android.App;
using Android.Content.PM;
using Android.Net.IpSec.Ike.Exceptions;
using Android.OS;

namespace Rezeptbuch
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Farbe der Statusleiste
            // {StaticResource Android}
            Window?.SetStatusBarColor(Android.Graphics.Color.ParseColor("#4284f5"));
        }
    }
}
