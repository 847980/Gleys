using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.Events;
using System;
using Gley.GameServices;

public class PlaySaveManager : MonoBehaviour
{
    public static PlaySaveManager I;
    public AchievementNames tes;
    private void Start()
    {
        I = this;
    }
    #region SAVE & LOAD
    //public CanvasSave saveCanvas;
    //private bool isSaving = false;
    //private string data;

    /// <summary>
    /// Saves the provided data
    /// </summary>
    /// <param name="filename">A unique key for the saved data</param>
    /// <param name="data">The string data to be saved (JSON format is also supported)</param>
    /// <param name="action">An optional action to execute after the data is successfully saved</param>
    public void SavePlayerData(string filename, string data, UnityAction<string> action = null) => SaveLoadPlayerData(filename, data, true, action);

    /// <summary>
    /// Loads the saved data
    /// </summary>
    /// <param name="filename">A unique key for the saved dat</param>
    /// <param name="action">Set instance after data loaded</param>
    public void LoadPlayerData(string filename, UnityAction<string> action, UnityAction fallbackAction = null) => SaveLoadPlayerData(filename, "", false, action, fallbackAction);

    public void SaveLoadPlayerData(string filename, string data, bool saving, UnityAction<string> action = null, UnityAction fallbackAction = null)
    {
        Debug.Log("start trying save load");
        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                Debug.Log($"SavedGameRequestStatus {status}");
                if (status == SavedGameRequestStatus.Success)
                {
                    Debug.Log("status sukses");
                    if (saving)
                    {
                        byte[] dataByte = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
                        SavedGameMetadataUpdate updatedMetadata = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Saved game at " + DateTime.Now.ToString()).Build();
                        ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(game, updatedMetadata, dataByte, (SavedGameRequestStatus status, ISavedGameMetadata game) =>
                        {
                            if (status == SavedGameRequestStatus.Success)
                            {
                                Debug.Log("Successfull write game");
                                action?.Invoke(data); //kembalikan data yang di save
                            }
                            else
                            {
                                Debug.Log("failed write game");
                            }
                            });
                        }
                    else //loading
                    {
                        print("loading data");
                        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, (SavedGameRequestStatus status, byte[] data) =>
                        {
                            if (status == SavedGameRequestStatus.Success)
                            {
                                string loadedData = System.Text.ASCIIEncoding.ASCII.GetString(data);
                                print($"data di load: {loadedData}");
                                if (loadedData.Length == 0)
                                {
                                    print("blom ada save data");
                                    fallbackAction?.Invoke();
                                    Debug.Log("fallback, save default");
                                    return;
                                }
                                print(loadedData.Length);
                                action?.Invoke(loadedData);
                            }
                            else
                            {
                                Debug.Log("failed load game");
                                //fallbackAction?.Invoke();
                                //Debug.Log("fallback, save default");
                            }
                        });
                    }
                }
                else
                {
                    Debug.Log("Failed open file");
                    //fallbackAction?.Invoke();
                }
            });
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Successfull write game");
        }
        else
        {
            Debug.Log("failed write game");
        }
    }
    #endregion
}
