using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    //private GameObject _selected;
    private GameObject _icon;

    private void Awake()
    {
        _icon = transform.Find("PointerIcon").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        _icon.SetActive(false);

    }

    public void OnSelect(BaseEventData eventData)
    {
        _icon.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _icon.SetActive(false);
    }
}
