using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace CatFinder
{
    [Activity(
        Label = "Cat Finder"
        , Theme = "@style/App"
        , MainLauncher = true
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.Splash)
        {

        }
    }
}
