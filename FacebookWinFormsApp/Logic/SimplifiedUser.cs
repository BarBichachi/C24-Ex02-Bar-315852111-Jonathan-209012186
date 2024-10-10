using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using CefSharp.DevTools.Debugger;

using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.Logic
{
    internal class SimplifiedUser
    {
        private readonly User r_LoggedInUser;

        public SimplifiedUser(User i_LoggedInUser)
        {
            r_LoggedInUser = i_LoggedInUser ?? throw new ArgumentNullException(nameof(i_LoggedInUser));

            initializeProperties();
        }

        private void initializeProperties()
        {
            Name = r_LoggedInUser.Name;
            Birthday = DateTime.TryParseExact(r_LoggedInUser.Birthday, "MM/dd/yyyy", null, DateTimeStyles.None, out DateTime parsedDate) ? (DateTime?)parsedDate : null;
            City = r_LoggedInUser.Location.Name;
        }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }

        public string City { get; set; }

        public User.eGender? Gender => r_LoggedInUser.Gender;

        public Image ProfileImage => r_LoggedInUser.ImageLarge;

        public Image CoverImage
        {
            get
            {
                Image coverPhoto = null;

                foreach (Album album in r_LoggedInUser.Albums)
                {
                    if (album.Name == "Cover photos" && album.Photos.Count > 0)
                    {
                        coverPhoto = album.Photos[0].ImageNormal;
                    }
                }

                return coverPhoto; // Return null if no cover photo found
            }
        }

        public FacebookObjectCollection<User> Friends => r_LoggedInUser.Friends;

        public FacebookObjectCollection<Page> LikedPages => r_LoggedInUser.LikedPages;

        public Page[] FavoriteTeams => r_LoggedInUser.FavofriteTeams;

        public FacebookObjectCollection<Post> Posts => r_LoggedInUser.Posts;

        public FacebookObjectCollection<Checkin> Checkins => r_LoggedInUser.Checkins;

        public FacebookObjectCollection<Album> Albums => r_LoggedInUser.Albums;
    }
}