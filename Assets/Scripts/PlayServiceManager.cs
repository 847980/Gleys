using System;
using System.Collections;
using System.Collections.Generic;
using Gley.GameServices;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;
using Firebase.Analytics;
using Gley.GameServices.Internal;

public class PlayServiceManager : MonoBehaviour
{
    public static PlayServiceManager I;
    public GameObject[] PlayServices;
    // Start is called before the first frame update
    public int score;
    public void SetScore(string score)
    {
        this.score = int.Parse(score);
    }
    void Start()
    {
        I = this;
        PlayGamesPlatform.DebugLogEnabled = true;
        API.LogIn(loginResult);
        //PlayGamesPlatform.Activate();
        //Social.localUser.Authenticate(loginResult);


        //trackInc = PlayerPrefs.GetInt("trackInc", 0);
        //if (trackInc >= 20)
        //{
        //    incAchButton.SetActive(false);
        //}


    }

    public void LoadAndCheckIfAchievementUnlocked(AchievementNames achievementName)
    {
        string achievementID = GameServicesManager.Instance.achievementsManager.GetAchievementId(achievementName);
        Social.LoadAchievements(achievements =>
        {
            if (achievements == null || achievements.Length == 0)
            {
                Debug.Log("Tidak ada achievement yang ditemukan atau gagal load.");
                return;
            }

            foreach (var achievement in achievements)
            {
                if (achievement.id == achievementID)
                {
                    if (achievement.completed)
                    {
                        Debug.Log($"Achievement '{achievementID}' sudah UNLOCKED!");
                        switch (achievementName)
                        {
                            case AchievementNames.achieve1:
                                unlockedAchButton.SetActive(false);
                                break;
                            case AchievementNames.achieve2:
                                incAchButton.SetActive(false);
                                break;
                        }
                    }
                    else
                    {
                        Debug.Log($"Achievement '{achievementID}' masih LOCKED. Progres: {achievement.percentCompleted}%");
                        if (achievementName == AchievementNames.achieve2)
                        {
                            //trackInc = achievement.percentCompleted * 20/100f; //kalau butuh tau berapa kali inc dari total --> * steps/100
                            trackInc = achievement.percentCompleted; //100% kebuka
                        }
                    }
                    return;
                }
            }

            Debug.Log($"Achievement '{achievementID}' tidak ditemukan dalam daftar.");
        });
    }


    private void loginResult(bool success)
    {
        if (success)
        {
            print("sukses login");
            SetActiveArray(PlayServices, true);
            CheckAchievements();
        }
        else
        {
            //show login button
            print("gagal login");
            SetActiveArray(PlayServices, false);
        }
    }

    private void CheckAchievements()
    {
        for (int i = 0; i < Enum.GetValues(typeof(AchievementNames)).Length; i++)
        {
            LoadAndCheckIfAchievementUnlocked((AchievementNames)i);
        }
    }

    public void SetActiveArray(GameObject[] array, bool active)
    {
        // Ambil util code
        foreach (var item in array)
        {
            item.SetActive(active);
        }
    }
    //public void SubmitAchievement() => API.SubmitAchievement(AchievementNames.achieve1);
    public void SubmitAchievement()
    {
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login ulang sukses, mencoba load achievements...");
                API.SubmitAchievement(AchievementNames.achieve1, (success, error) =>
                {
                    if (success)
                    {
                        print("sukses buka achievement");
                        FirebaseManager.I.LogEvent("achievement_unlocked", new Firebase.Analytics.Parameter[]
                        {
                            new (FirebaseAnalytics.ParameterAchievementID, AchievementNames.achieve1.ToString())
                        });
                    }
                    else
                    {
                        print($"failed submit achievement {error}");
                    }
                });
            }
            else
            {
                Debug.LogError("Re-login gagal, tidak bisa load achievements.");
            }
        });
    }

    public double trackInc = 0;
    public GameObject incAchButton;
    public GameObject unlockedAchButton;
    public void IncrementAchievement()
    {
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login ulang sukses, mencoba load achievements...");
                API.IncrementAchievement(AchievementNames.achieve2, 1, (success, error) =>
                {
                    if (success)
                    {
                        print("sukses isi achievement");
                        print($"poin sebelum {trackInc}");
                        trackInc++;
                        //PlayerPrefs.SetInt("trackInc", trackInc);
                        //PlayerPrefs.Save();
                        FirebaseManager.I.LogEvent("achievement_increment", new Firebase.Analytics.Parameter[]
                        {
                            new (FirebaseAnalytics.ParameterAchievementID, AchievementNames.achieve2.ToString()),
                            new (FirebaseAnalytics.ParameterScore, 1)
                        });
                        print($"poin setelah {trackInc}");
                        if (trackInc >= 100)
                        {
                            print($"buka deh");
                            FirebaseManager.I.LogEvent("achievement_unlocked", new Firebase.Analytics.Parameter[]
                            {
                                new (FirebaseAnalytics.ParameterAchievementID, AchievementNames.achieve1.ToString())
                            });
                            incAchButton.SetActive(false);
                        }
                    }
                    else
                    {
                        print($"failed increment achievement {error}");
                    }
                });
            }
            else
            {
                Debug.LogError("Re-login gagal, tidak bisa load achievements.");
            }
        });
    }


    //public void ShowAchievement() => API.ShowAchievementsUI();
    public void ShowAchievement()
    {
        //if (API.IsLoggedIn())
        //{
        //    print("tes achieve ui, login aman");
        //    API.ShowAchievementsUI();
        //}
        //else print("tes achieve ui, blom login");
        //print("auth cek manual" + PlayGamesPlatform.Instance.IsAuthenticated());
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login ulang sukses, mencoba load achievements...");
                Social.LoadAchievements(achievements =>
                {
                    if (achievements.Length > 0)
                    {
                        Debug.Log("Berhasil memuat achievements!");
                        Social.ShowAchievementsUI();
                    }
                    else
                    {
                        Debug.LogWarning("Tidak ada achievements ditemukan.");
                    }
                });
            }
            else
            {
                Debug.LogError("Re-login gagal, tidak bisa load achievements.");
            }
        });
    }
    //public void SubmitScore() => API.SubmitScore(score, LeaderboardNames.topskor);
    public void SubmitScore()
    {
        //if (API.IsLoggedIn())
        //{
        //    print("tes submit score, login aman");
        //    API.SubmitScore(score, LeaderboardNames.topskor, onsubmit);
        //}
        //else print("tes submit score, blom login");
        //print("auth cek manual" + PlayGamesPlatform.Instance.IsAuthenticated());
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login ulang sukses, mencoba load achievements...");
                API.SubmitScore(score, LeaderboardNames.topskor, onsubmit); //~ada modif di wrapper
            }
            else
            {
                Debug.LogError("Re-login gagal, tidak bisa load achievements.");
            }
        });
    }

    private void onsubmit(bool arg0, GameServicesError arg1)
    {
        print($"callback submit score bool {arg0} error {arg1}");
    }

    public void ShowLeaderboard()
    {
        //API.ShowSpecificLeaderboard(LeaderboardNames.topskor); //bisa kalau login manual in game
        //if (API.IsLoggedIn())
        //{
        //    print("tes leaderboard ui, login aman");
        //    API.ShowLeaderboadsUI();
        //}
        //else print("tes leaderboard ui, blom login");
        //print("auth cek manual" + PlayGamesPlatform.Instance.IsAuthenticated());
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login ulang sukses, mencoba load achievements...");
                Social.ShowLeaderboardUI();
            }
            else
            {
                Debug.LogError("Re-login gagal, tidak bisa load achievements.");
            }
        });
    }
    //public void ShowLeaderboard() => API.ShowLeaderboadsUI();
    public void Login() => PlayGamesPlatform.Instance.ManuallyAuthenticate(OnSignInResult);
    private void OnSignInResult(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            loginResult(true);
        }
        else
        {
            loginResult(false);
        }
    }
}
