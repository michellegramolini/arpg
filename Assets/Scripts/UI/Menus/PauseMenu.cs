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
}
