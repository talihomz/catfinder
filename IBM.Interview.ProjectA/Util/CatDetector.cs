using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OpenCV.Android;
using OpenCV.ObjDetect;

namespace IBM.Interview.ProjectA.Util
{
    public class CatDetector
    {
        private readonly CascadeClassifier _classifier;
        private const string CLASSIFIER_PATH = "";        // TODO Enter correct classifier path

        public CatDetector()
        {
            _classifier = new CascadeClassifier();
        }

        // detect cat in an image
        public Rect[] DetectCatFaces(Byte[] image)
        {
            // read the image

            return null;
        }
    }
}