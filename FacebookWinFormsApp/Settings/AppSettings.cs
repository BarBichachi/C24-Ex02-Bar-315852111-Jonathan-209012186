using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BasicFacebookFeatures.UI
{
    public class AppSettings
    {
        #region Fields

        private static readonly string sr_FileName;
        private static AppSettings s_This;
        private static readonly object sr_Lock = new object();

        #endregion

        #region Properties

        public bool AutoLogin { get; set; }

        public string SavedAccessToken { get; set; }

        public string SavedUsername { get; set; }

        #endregion

        #region CTOR

        private AppSettings()
        {
        }

        static AppSettings()
        {
            sr_FileName = Application.ExecutablePath + ".settings.xml";
        }

        #endregion

        #region Methods

        public static AppSettings Instance
        {
            get
            {
                if (s_This == null)
                {
                    lock (sr_Lock)
                    {
                        if (s_This == null)
                        {
                            s_This = fromFileOrDefault();
                        }
                    }
                }

                return s_This;
            }
        }

        public void Save()
        {
            using (FileStream stream = new FileStream(sr_FileName, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                serializer.Serialize(stream, this);
            }
        }

        private static AppSettings fromFileOrDefault()
        {
            AppSettings loadedThis = null;

            if (File.Exists(sr_FileName))
            {
                using (FileStream stream = new FileStream(sr_FileName, FileMode.OpenOrCreate))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                    loadedThis = (AppSettings)serializer.Deserialize(stream);
                }
            }
            else
            {
                loadedThis = new AppSettings()
                                 {
                                     AutoLogin = false,
                                     SavedUsername = "defaultUser",
                                     SavedAccessToken = "defaultToken",
                                 };
            }

            return loadedThis;
        }

        #endregion
    }
}