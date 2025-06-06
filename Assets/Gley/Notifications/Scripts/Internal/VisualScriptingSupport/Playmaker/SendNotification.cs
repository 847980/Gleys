﻿#if GLEY_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://gley.gitbook.io/mobile-notifications/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Displays a notification")]
    public class SendNotification : FsmStateAction
    {
        [Tooltip("Notification Title")]
        public string gameTitle;
        [Tooltip("Notification Body")]
        public string notificationBody;
        [Tooltip("Hours until notification will be shown")]
        public int hours;
        [Tooltip("Minutes until notification will be shown")]
        public int minutes;
        [Tooltip("Seconds until notification will be shown")]
        public int seconds;
        [Tooltip("Small icon name")]
        public string smallIcon;
        [Tooltip("Large icon name")]
        public string largeIcon;

        public override void OnEnter()
        {
            Gley.Notifications.API.SendNotification(gameTitle, notificationBody, new System.TimeSpan(hours, minutes, seconds), smallIcon, largeIcon, "Opened from Notification");
            Finish();
        }
    }
}
#endif
