using MvvmCross.Core.ViewModels;

namespace Catfinder.Core.ViewModels
{
    public class FinderViewModel : MvxViewModel
    {
        private string _detectorStatus;

        public FinderViewModel()
        {
            DetectorStatus = "Loading Detector...";
        }

        public string DetectorStatus
        {
            get { return _detectorStatus; }
            set
            {
                _detectorStatus = value; 
                RaisePropertyChanged( () => DetectorStatus );
            }
        }
    }
}
