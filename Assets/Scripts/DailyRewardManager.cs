using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DailyRewardManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Gley.DailyRewards.API.Calendar.AddClickListener(OnClickDay);
    }

    private void OnClickDay(int day, int reward, Sprite sprite)
    {
        print($"dapet {reward} hari ke {day}");
    }

    public void SHowCalendar()
    {
        Gley.DailyRewards.API.Calendar.Show();
    }
}
