using System;
using System.Collections;
using System.Collections.Generic;
using Gley.GameServices;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class BaseSaveButton : MonoBehaviour
{
    public string data;
    public string key;
    public TMP_InputField input;
    private void Start()
    {
        StartCoroutine(WaitAPI());
    }
    IEnumerator WaitAPI()
    {
        while (!API.IsLoggedIn()) yield return new WaitForEndOfFrame();
        //PlaySaveManager.I.LoadPlayerData(key, OnDataLoaded, () => { input.text = "0"; Save(); });
        print($"siap load data {gameObject.name}");
        Load(() => { input.text = "0"; print("ini exec fallback");  Save(); });
    }
    public void Save()
    {
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login ulang sukses, mencoba load achievements...");
                PlaySaveManager.I.SavePlayerData(key, data);
            }
            else
            {
                Debug.LogError("Re-login gagal, tidak bisa load achievements.");
            }
        });
    }
    public void Load(UnityAction fallbackAction = null)
    {
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login ulang sukses, mencoba load save...");
                PlaySaveManager.I.LoadPlayerData(key, OnDataLoaded, fallbackAction);
            }
            else
            {
                Debug.LogError("Re-login gagal, tidak bisa load save.");
            }
        });
    }

    private void OnDataLoaded(string data)
    {
        input.text = data;
    }
    public void SetData(string data) => this.data = data;
}
