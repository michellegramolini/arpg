using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Methods for pause in-game menu
public class PauseMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit game.");
        Application.Quit();
    }

    public void SaveGame()
    {
        JSONSaver.Instance.SaveData();
    }

    public void LoadGame()
    {
        JSONSaver.Instance.LoadData();
    }
}
