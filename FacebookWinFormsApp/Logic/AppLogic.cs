using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using BasicFacebookFeatures.Strategy;
using BasicFacebookFeatures.UI;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;

using Timer = System.Windows.Forms.Timer;


namespace BasicFacebookFeatures.Logic
{
    internal class AppLogic
    {
        private static readonly AppLogic sr_Instance = new AppLogic();
        private static readonly AppSettings sr_AppSettings = AppSettings.Instance;
        private SimplifiedUser m_SimplifiedUser;
        private LoginResult m_LoginResult;
        private List<string> m_RandomFacts;
        private readonly Random r_Random = new Random();
        private Timer m_Timer;
        private int m_RemainingSeconds;

        public event EventHandler TimerElapsed;
        public event EventHandler TimeUpdated;

        public AppLogic()
        {
        }

        public void Initialize()
        {
            connectToUser();
            new Thread(initializeRandomFacts) { IsBackground = true }.Start();
            initializeTimer();
        }

        private void initializeTimer()
        {
            m_RemainingSeconds = Math.Max(1, (DateTime.Now.Year - (UserBirthday?.Year ?? DateTime.Now.Year))) * 60;
            m_Timer = new Timer { Interval = 1000 };
            m_Timer.Tick += onTimerTick;

            m_Timer.Start();
        }

        private void onTimerTick(object sender, EventArgs e)
        {
            if (--m_RemainingSeconds <= 0)
            {
                m_Timer.Stop();
                TimerElapsed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                TimeUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public string GetRemainingTime()
        {
            return TimeSpan.FromSeconds(m_RemainingSeconds).ToString(@"hh\:mm\:ss");
        }

        private bool isAlreadyLoggedIn()
        {
            return sr_AppSettings.SavedAccessToken != "defaultToken";
        }

        private void connectToUser()
        {
            try
            {
                if (!isAlreadyLoggedIn())
                {
                    m_LoginResult = FacebookService.Connect("EAAF5GQRl3KQBO2FKspw47BqSjwRQA8aVZCYMHI1n9sNfzf5PgurT6orBWpZBJPqIeUZAqB6pAb8OjjsUoSdd5gTzZBxfgHRYVjMB7IIAxbAshoPbV6mNNNoZCg9wyX7WHSRE0dZBTOzGbz9pfvhPlm1mLaHLxVBqfE0xCdaLejLSE77ThNmLi9tt2apOJKv6YoeCczzw2fkZArXqHZB0zoOUZB0PclqSWQA60wiGYYZCIUeFIs3ZCAnqm4ZD");
                    //m_loginResult = FacebookService.Login("414623331638436", 
                    //        //    // Permissions
                    //        //    "public_profile",
                    //        //    "email",
                    //        //    "user_age_range",
                    //        //    "user_birthday",
                    //        //    "user_events",
                    //        //    "user_friends",
                    //        //    "user_gender",
                    //        //    "user_hometown",
                    //        //    "user_likes",
                    //        //    "user_link",
                    //        //    "user_location",
                    //        //    "user_photos",
                    //        //    "user_posts",
                    //        //    "user_videos"
                    //        //);
                    sr_AppSettings.SavedAccessToken = m_LoginResult.AccessToken;
                    sr_AppSettings.SavedUsername = m_LoginResult.LoggedInUser.ToString();
                }
                else
                {
                    m_LoginResult = FacebookService.Connect(sr_AppSettings.SavedAccessToken);
                }

                m_SimplifiedUser = new SimplifiedUser(m_LoginResult.LoggedInUser);
            }
            catch (Exception e)
            {
                throw new ApplicationException("Connecting to user has failed! >\n" + e.Message, e);
            }
        }

        public static AppLogic Instance => sr_Instance;

        public SimplifiedUser SimplifiedUser => m_SimplifiedUser; // Also data source for two-way data binding (EditProfileForm)

        public Image UserProfileImage => m_SimplifiedUser.ProfileImage;

        public Image UserCoverImage => m_SimplifiedUser.CoverImage;

        public string UserName => m_SimplifiedUser.Name;

        public int UserFriendsCount => m_SimplifiedUser.Friends.Count;

        public DateTime? UserBirthday => m_SimplifiedUser.Birthday;

        public string UserCity => m_SimplifiedUser.City;

        public User.eGender? UserGender => m_SimplifiedUser.Gender;

        public bool AutoLogin
        {
            get => sr_AppSettings.AutoLogin;
            set => sr_AppSettings.AutoLogin = value;
        }


        private void initializeRandomFacts()
        {
            int facebookFoundedAge = Math.Max(0, 2004 - (m_SimplifiedUser.Birthday?.Year ?? 2004));
            string birthdayString = m_SimplifiedUser.Birthday?.ToString("dd/MM/yyyy") ?? "Not specified";

            m_RandomFacts = new List<string>
            {
                $"You have {m_SimplifiedUser.Posts.Count} posts.",
                $"You have liked {m_SimplifiedUser.LikedPages.Count} pages so far.",
                $"You’ve checked in at {m_SimplifiedUser.Checkins.Count} different places!",
                $"You have {m_SimplifiedUser.Albums.Count} albums.",
                $"You have {m_SimplifiedUser.Friends.Count} friends.",
                $"Your birthday is on - {birthdayString}",
                facebookFoundedAge == 0
                    ? "You were not born when Facebook was founded or you didn't insert your birthday!"
                    : $"You were {facebookFoundedAge} years old when Facebook was founded."
            };
        }

        public string GenerateRandomFact()
        {
            if (m_RandomFacts?.Count > 0)
            {
                int index = r_Random.Next(m_RandomFacts.Count);
                return m_RandomFacts[index];
            }

            return "No facts available.";
        }

        public void ResetAppSettings()
        {
            if (!sr_AppSettings.AutoLogin)
            {
                sr_AppSettings.SavedAccessToken = "defaultToken";
                sr_AppSettings.SavedUsername = "defaultUser";
            }

            SaveAppSettings();
        }

        public void SaveAppSettings() => sr_AppSettings.Save();

        public Region CreateCircularRegion(int i_Dimensions)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, i_Dimensions, i_Dimensions);
            return new Region(gp);
        }

        public void PopulateLayout(FlowLayoutPanel i_FlowLayoutPanel, int i_NumberOfColumns, int i_MaxBoxes, ILayoutPopulationStrategy i_Strategy)
        {
            i_Strategy.PopulateLayout(m_SimplifiedUser, i_FlowLayoutPanel, i_NumberOfColumns, i_MaxBoxes);
        }

        public void Logout()
        {
            sr_AppSettings.AutoLogin = false;
            ResetAppSettings();
            FacebookService.Logout();
        }

        public bool ShouldAutoLogin()
        {
            return sr_AppSettings.AutoLogin;
        }
    }
}