using System;
using System.Windows.Forms;

namespace BasicFacebookFeatures.UI
{
    public class LazyPictureBox : PictureBox
    {
        public string URL { private get; set; }
        bool isImageLoaded = false;

        public void Load(string i_UrlToLoad)
        {
            URL = i_UrlToLoad;
            isImageLoaded = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (!isImageLoaded && !string.IsNullOrEmpty(URL))
            {
                base.ImageLocation = URL;
                isImageLoaded = true;
            }

            base.OnPaint(pe);
        }
    }
}
