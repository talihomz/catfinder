using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Catfinder.Core.ViewModels;
using CatFinder.Util;
using Java.IO;
using Java.Lang;
using MvvmCross.Droid.Views;
using OpenCV.Android;
using OpenCV.Core;
using OpenCV.ImgProc;
using OpenCV.ObjDetect;

namespace CatFinder.Activities
{
    [Activity(Label = "Cat Finder", Theme = "@style/App",
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class FinderActivity : MvxActivity<FinderViewModel>, CameraBridgeViewBase.ICvCameraViewListener2, SeekBar.IOnSeekBarChangeListener
    {
        private CameraBridgeViewBase _openCvCamera;
        private LibLoaderCallback _loaderCallback;
        private Mat _mRgba, _mGray;
        private static readonly Scalar FaceRectColor = new Scalar(0, 255, 0, 255);
        private float _mRelativeFaceSize = 0.2f;
        private int _mAbsoluteFaceSize = 0;
        private SeekBar _seekBar;
        private const string ActivityLogger = "FinderActivity";

        public File CascadeFile { get; set; }
        public CascadeClassifier Detector { get; set; }
        public CameraBridgeViewBase OpenCvCamera => _openCvCamera;
         
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // configure screen
            ConfigureScreen();

            // set content view
            SetContentView(Resource.Layout.Finder);

            // configure camera
            ConfigureCamera();

            // configure the seekbar
            ConfigureSeekbar();
        }

        private void ConfigureSeekbar()
        {
            _seekBar = FindViewById<SeekBar>(Resource.Id.minimumFaceSize);
        }
        private void SetMinFaceSize(float faceSize)
        {
            _mRelativeFaceSize = faceSize;
            _mAbsoluteFaceSize = 0;
        }


        protected override void OnResume()
        {
            base.OnResume();
            if (!OpenCVLoader.InitDebug())
            {
                Log.Debug(ActivityLogger, "Internal OpenCV library not found. Using OpenCV Manager for initialization");
                OpenCVLoader.InitAsync(OpenCVLoader.OpencvVersion300, this, _loaderCallback);
            }
            else
            {
                Log.Debug(ActivityLogger, "OpenCV was found successfully and is in use!");
                _loaderCallback.OnManagerConnected(LoaderCallbackInterface.Success);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            _openCvCamera?.DisableView();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _openCvCamera.DisableView();
        }

        private void ConfigureCamera()
        {
            _openCvCamera = FindViewById<CameraBridgeViewBase>(Resource.Id.cameraSurfaceView);
            _openCvCamera.Visibility = ViewStates.Visible;
            _openCvCamera.SetCvCameraViewListener2(this);

            _loaderCallback = new LibLoaderCallback(this, this);
        }

        private void ConfigureScreen()
        {
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
        }

        public void OnCameraViewStarted(int p0, int p1)
        {
            // initialise 
            _mGray = new Mat();
            _mRgba = new Mat();
        }

        public void OnCameraViewStopped()
        {
            _mGray.Release();
            _mRgba.Release();
        }

        public Mat OnCameraFrame(CameraBridgeViewBase.ICvCameraViewFrame inputFrame)
        {
            _mRgba = inputFrame.Rgba();
            _mGray = inputFrame.Gray();

            if (_mAbsoluteFaceSize == 0)
            {
                var height = _mGray.Rows();
                if (Math.Round(height * _mRelativeFaceSize) > 0)
                {
                    _mAbsoluteFaceSize = Math.Round(height * _mRelativeFaceSize);
                }
            }

            var faces = new MatOfRect();

            Detector?.DetectMultiScale(_mGray, faces, 1.1, 2, 2, new OpenCV.Core.Size(_mAbsoluteFaceSize, _mAbsoluteFaceSize), new OpenCV.Core.Size());

            // draw faces
            var facesArray = faces.ToArray();
            foreach (var t in facesArray)
                Imgproc.Rectangle(_mRgba, t.Tl(), t.Br(), FaceRectColor, 3);

            return _mRgba;
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (fromUser)
            {
                SetMinFaceSize( 0.1f * progress);
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            System.Diagnostics.Debug.WriteLine("Tracking changes.");
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            System.Diagnostics.Debug.WriteLine("Stopped tracking changes.");
        }
    }
}