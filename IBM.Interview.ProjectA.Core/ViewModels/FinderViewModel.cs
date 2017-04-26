using System.IO;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.PictureChooser;

namespace IBM.Interview.ProjectA.Core.ViewModels
{
    public class FinderViewModel : MvxViewModel
    {
        private readonly IMvxPictureChooserTask _pictureChooser;
        private byte[] _bytes;
        private IMvxCommand _takePicture, _openGallery;

        public byte[] Bytes
        {
            get { return _bytes; }
            set
            {
                _bytes = value;
                RaisePropertyChanged(() => Bytes);
            }
        }

        public IMvxCommand TakePictureCommand => _takePicture ?? (_takePicture = new MvxCommand(TakePicture));
        public IMvxCommand OpenGalleryCommand => _openGallery ?? (_openGallery = new MvxCommand(OpenGallery));

        // constructor
        public FinderViewModel(IMvxPictureChooserTask pictureChooser)
        {
            _pictureChooser = pictureChooser;
        }

        // open gallery
        private void OpenGallery()
        {
            _pictureChooser.ChoosePictureFromLibrary(400, 95, OnPicture, () => { });
        }

        // take picture
        private void TakePicture()
        {
            _pictureChooser.TakePicture(400, 95, OnPicture, () => { });
        }

        // handle image selection
        private void OnPicture(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes = memoryStream.ToArray();
        }
    }
}
