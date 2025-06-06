﻿using UnityEngine.Events;

namespace Gley.GameServices.Internal
{
    public class GameServicesManager
    {
        private LogInManager logInManager = new LogInManager();
        public AchievementsManager achievementsManager = new AchievementsManager();
        private LeaderboardManager leaderboardManager = new LeaderboardManager();


        //automatically creates an instance at first call
        private static GameServicesManager instance;
        public static GameServicesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameServicesManager();
                }
                return instance;
            }
        }


        /// <summary>
        /// Login the player, should be called just once at the beginning of your game
        /// </summary>
        /// <param name="LoginComplete">if bool is true, login was successfully else an error occurred</param>
        public void LogIn(UnityAction<bool> LoginComplete = null)
        {
            logInManager.LogiInServices(LoginComplete);
        }


        /// <summary>
        /// Used to submit an achievement
        /// </summary>
        /// <param name="achievementName"> AchievementNames is an enum auto-generated when you use the SettingsWindow to set up the plugin
        /// It contains all your achievements so it is easy for you to call it from your code</param>
        /// <param name="SubmitComplete">callback if bool is true, achievement submitted successfully, else you get an error message </param>
        public void SubmitAchievement(AchievementNames achievementName, UnityAction<bool, GameServicesError> SubmitComplete = null)
        {
            if (logInManager.IsLoggedIn())
            {
                achievementsManager.SumbitAchievement(achievementName, SubmitComplete);
            }
            else
            {
                if (SubmitComplete != null)
                {
                    achievementsManager.SumbitAchievement(achievementName, SubmitComplete); //tetap submit, login di luar API
                    //SubmitComplete(false, GameServicesError.NotLoggedIn);
                }
            }
        }


        /// <summary>
        /// Submit an incremental achievement
        /// </summary>
        /// <param name="achievementName">AchievementNames is an enum auto-generated when you use the SettingsWindow to set up the plugin</param>
        /// <param name="steps">how many units to be incremented</param>
        /// <param name="SubmitComplete">callback if bool is true, achievement submitted successfully, else you get an error message</param>
        public void IncrementAchievement(AchievementNames achievementName, int steps, UnityAction<bool, GameServicesError> SubmitComplete = null)
        {
            if (logInManager.IsLoggedIn())
            {
                achievementsManager.IncrementAchievement(achievementName, steps, SubmitComplete);
            }
            else
            {
                if (SubmitComplete != null)
                {
                    achievementsManager.IncrementAchievement(achievementName, steps, SubmitComplete);//tetap inc, login di luar API
                    //SubmitComplete(false, GameServicesError.NotLoggedIn);
                }
            }
        }


        /// <summary>
        /// Shows the default list of all game achievements
        /// </summary>
        public void ShowAchievementsUI()
        {
            if (logInManager.IsLoggedIn())
            {
                achievementsManager.ShowAchievements();
            }
        }


        /// <summary>
        /// Submits the score to desired leaderboard
        /// </summary>
        /// <param name="score"> the player score </param>
        /// <param name="leaderboardName"> LeaderboardsNames is an enum with all game leaderboards automatically generated from SettingsWindow</param>
        /// <param name="SubmitComplete"> callback if bool is true score was submitted successfully else you get en error messages</param>
        public void SubmitScore(long score, LeaderboardNames leaderboardName, UnityAction<bool, GameServicesError> SubmitComplete = null)
        {
            if (logInManager.IsLoggedIn())
            {
                leaderboardManager.SubmitScore(score, leaderboardName.ToString(), SubmitComplete);
            }
            else
            {
                if (SubmitComplete != null)
                {
                    leaderboardManager.SubmitScore(score, leaderboardName.ToString(), SubmitComplete); //login di luar API
                    //SubmitComplete(false, GameServicesError.NotLoggedIn);
                }
            }
        }


        /// <summary>
        /// Shows all game leaderboards
        /// </summary>
        public void ShowLeaderboadsUI()
        {
            if (logInManager.IsLoggedIn())
            {
                leaderboardManager.ShowLeaderboards();
            }
        }


        /// <summary>
        /// Shows a single game Leaderboard
        /// </summary>
        /// <param name="leaderboardName">the name of the Leaderboard to display</param>
        public void ShowSpecificLeaderboard(LeaderboardNames leaderboardName)
        {
            leaderboardManager.ShowSingleLeaderboard(leaderboardName);
        }


        /// <summary>
        /// Get the highest score from Leaderboard for the current user  
        /// </summary>
        /// <param name="leaderboardName">name of the leaderboard</param>
        /// <param name="CompleteMethod">method to call after score is read</param>
        public void GetPlayerScore(LeaderboardNames leaderboardName, UnityAction<long> CompleteMethod)
        {
            leaderboardManager.GetPlayerScore(leaderboardName,
                (long score) =>
                {
                    if (CompleteMethod != null)
                    {
                        CompleteMethod(score);
                    }
                }
                );
        }


        /// <summary>
        /// Get rank from Leaderboard for the current user  
        /// </summary>
        /// <param name="leaderboardName">name of the leaderboard</param>
        /// <param name="CompleteMethod">method to call after score is read</param>
        public void GetPlayerRank(LeaderboardNames leaderboardName, UnityAction<long> CompleteMethod)
        {
            leaderboardManager.GetPlayerRank(leaderboardName,
                (long rank) =>
                {
                    if (CompleteMethod != null)
                    {
                        CompleteMethod(rank);
                    }
                }
                );
        }


        /// <summary>
        /// Used to check if user is logged in
        /// </summary>
        /// <returns></returns>
        public bool IsLoggedIn()
        {
            return logInManager.IsLoggedIn();
        }


        public bool IsComplete(AchievementNames name)
        {
            return achievementsManager.IsComplete(name);
        }
    }
}
