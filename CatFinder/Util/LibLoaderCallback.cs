﻿using Android.Content;
using Android.Util;
using CatFinder.Activities;
using Java.IO;
using OpenCV.Android;
using OpenCV.ObjDetect;

namespace CatFinder.Util
{
    public class LibLoaderCallback : BaseLoaderCallback
    {
        private readonly FinderActivity _activity;
        private const string LoggerMessage = "LoaderCallback";

        public LibLoaderCallback(FinderActivity activity, Context context) : base(context)
        {
            _activity = activity;
        }

        public override void OnManagerConnected(int status)
        {
            switch (status)
            {
                case LoaderCallbackInterface.Success:
                    {
                        Log.Info(LoggerMessage, "OpenCV loaded successfully");

                        ConfigureDetector();

                        // ConfigureCamera
                        ConfiureCamera();
                    }
                    break;
                default:
                    {
                        base.OnManagerConnected(status);
                    }
                    break;
            }
        }

        private void ConfiureCamera()
        {
            _activity.OpenCvCamera.EnableView();
        }

        private void ConfigureDetector()
        {
            try
            {
                File cascadeDir;

                using (var inputFileStream = _activity.Resources.OpenRawResource(Resource.Raw.haarcascade_frontalcatface))
                {
                    cascadeDir = _activity.GetDir("cascade", FileCreationMode.Private);
                    _activity.CascadeFile = new File(cascadeDir, "haarcascade_frontalcatface.xml");
                    using (var os = new FileOutputStream(_activity.CascadeFile))
                    {
                        int byteRead;
                        while ((byteRead = inputFileStream.ReadByte()) != -1)
                        {
                            os.Write(byteRead);
                        }
                    }
                }

                // create the detector
                _activity.Detector = new CascadeClassifier(_activity.CascadeFile.AbsolutePath);
                if (_activity.Detector.Empty())
                {
                    Log.Error(LoggerMessage, "Failed to load cascade classifier");
                    _activity.Detector = null;
                }
                else
                    Log.Info(LoggerMessage, "Loaded cascade classifier from " + _activity.CascadeFile.AbsolutePath);

                cascadeDir.Delete();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
                Log.Error(LoggerMessage, "Failed to load cascade. Exception thrown: " + e);
            }
        }
    }
}