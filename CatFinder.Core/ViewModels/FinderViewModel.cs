using System.IO;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.PictureChooser;

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
