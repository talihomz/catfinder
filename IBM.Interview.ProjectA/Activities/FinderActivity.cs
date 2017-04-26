using Android.App;
using IBM.Interview.ProjectA.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace IBM.Interview.ProjectA.Activities
{
    [Activity(Label = "Finder", Theme = "@style/App")]
    public class FinderActivity : MvxActivity<FinderViewModel>
    {
        protected override void OnViewModelSet()
        {
            // Create your application here
            SetContentView(Resource.Layout.Finder);
        }
    }
}