﻿using Android.App;
using Android.OS;
using Android.Util;
using Android.Views;
using IBM.Interview.ProjectA.Core.ViewModels;
using IBM.Interview.ProjectA.Util;
using Java.IO;
using MvvmCross.Droid.Views;
using OpenCV.Android;
using OpenCV.Core;
using OpenCV.ImgProc;
using OpenCV.ObjDetect;

namespace IBM.Interview.ProjectA.Activities
{
    [Activity(Label = "Finder", Theme = "@style/App")]
    public class FinderActivity : MvxActivity<FinderViewModel>, CameraBridgeViewBase.ICvCameraViewListener2
    {
        private CameraBridgeViewBase _openCvCamera;
        private LibLoaderCallback _loaderCallback;
        private Mat _mRgba, _mGray;
        private int _mAbsoluteFaceSize;
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
            _mRgba = new Mat();
        }

        public void OnCameraViewStopped()
        {
            _mRgba.Release();
        }

        public Mat OnCameraFrame(CameraBridgeViewBase.ICvCameraViewFrame inputFrame)
        {
            _mRgba = inputFrame.Rgba();
            _mGray = inputFrame.Gray();

            var faces = new MatOfRect();

            Detector?.DetectMultiScale(_mGray, faces, 1.1, 2, 2, new OpenCV.Core.Size(_mAbsoluteFaceSize, _mAbsoluteFaceSize), new OpenCV.Core.Size());

            // draw faces
            //var facesArray = faces.ToArray();
            //foreach (Rect t in facesArray)
            //    Imgproc.Rectangle(_mRgba, t.Tl(), t.Br(), FaceRectColor, 3);

            return _mRgba;
        }
    }
}