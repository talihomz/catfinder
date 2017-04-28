using Android.Content;
using Catfinder.Core;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;

namespace CatFinder
{

    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {

        }

        protected override IMvxApplication CreateApp()
        {
            // just return a new instance of the core
            return new App();
        }
    }
}