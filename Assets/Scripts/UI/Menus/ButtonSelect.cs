using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelect : MonoBehaviour
{
    // Set in inspector
    [SerializeField]
    private GameObject primaryButton;

    private void OnEnable()
    {
        // set a new selected object
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(primaryButton);
    }

}
