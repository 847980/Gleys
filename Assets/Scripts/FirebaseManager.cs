using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.Messaging;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager I;
    private Firebase.FirebaseApp app;
    private bool firebaseReady;
    // Start is called before the first frame update
    void Start()
    {
        I = this;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            //Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
            //print(Firebase.FirebaseApp.LogLevel);
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                firebaseReady = true;
                InitFCM();
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }

    #region Firebase Messaging
    private void InitFCM()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }
    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        if (e.Message.Notification != null)
        {
            Debug.Log("Title: " + e.Message.Notification.Title);
            Debug.Log("Body: " + e.Message.Notification.Body);
        }

        if (e.Message.Data.Count > 0)
        {
            foreach (var key in e.Message.Data.Keys)
            {
                Debug.Log("Key: " + key + ", Value: " + e.Message.Data[key]);
            }
        }
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + e.Token);
    }
    #endregion

    public void UploadScore()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPostScore, new Parameter(FirebaseAnalytics.ParameterScore, PlayServiceManager.I.score));
        FirebaseAnalytics.LogEvent("score_progress", new Parameter(FirebaseAnalytics.ParameterScore, PlayServiceManager.I.score));
    }

    #region Firebase Analytics
    // https://support.google.com/analytics/table/13948007?visit_id=638793133800435775-440473301&rd=2
    public void LogEvent(string event_id) { if (firebaseReady) FirebaseAnalytics.LogEvent(event_id); }
    public void LogEvent(string event_id, Parameter[] parameters) { if (firebaseReady) FirebaseAnalytics.LogEvent(event_id, parameters); }
    #endregion
}
