using System;
using System.Collections.Generic;
using Gley.Notifications;
using TMPro;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public TextMeshProUGUI notifPayload;

    void Start()
    {
        API.Initialize();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            notifPayload.SetText("notif payload");
            API.SendNotification("NOTIF baru", "Anda keluar game?", new TimeSpan(0, 0, 10), "icon_0", "icon_1", "ini payload");
        }
        else
        {
            string openFromNotif = API.AppWasOpenFromNotification();
            notifPayload.SetText($"notif payload: {openFromNotif}");
        }
    }
}
