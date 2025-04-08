using System;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class tesManual : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("aktivasi");
        PlayGamesPlatform.Activate();
        print("auth");
        PlayGamesPlatform.Instance.Authenticate(AuthResult);
        print("done");
        //Gley.GameServices.API.LogIn();
    }

    private void AuthResult(SignInStatus status)
    {
        print(status);
        if (status == SignInStatus.Success)
        {
            print("sukses login");
        }
        else
        {
            print("gagal login");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
