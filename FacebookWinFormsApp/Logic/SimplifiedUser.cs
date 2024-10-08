﻿using System;
using System.ComponentModel;
using System.Drawing;

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
            Name = i_LoggedInUser.Name;
            Birthday = DateTime.TryParse(r_LoggedInUser.Birthday, out DateTime parsedDate) ? (DateTime?)parsedDate : null;
            City = r_LoggedInUser.Location.Name;
        }

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

        public string Name { get; set; }

        public FacebookObjectCollection<User> Friends => r_LoggedInUser.Friends;

        public FacebookObjectCollection<Page> LikedPages => r_LoggedInUser.LikedPages;

        public Page[] FavoriteTeams => r_LoggedInUser.FavofriteTeams;

        public DateTime? Birthday { get; set; }

        public string City { get; set; }

        public User.eGender? Gender => r_LoggedInUser.Gender;

        public FacebookObjectCollection<Post> Posts => r_LoggedInUser.Posts;

        public FacebookObjectCollection<Checkin> Checkins => r_LoggedInUser.Checkins;

        public FacebookObjectCollection<Album> Albums => r_LoggedInUser.Albums;
    }
}