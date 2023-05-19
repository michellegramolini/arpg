using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, ISubmitHandler
{
    //TODO: ref to HUD
    private Image _HUDItemImage;
    private Image _itemImage;

    private void Awake()
    {

        _itemImage = transform.Find("Image").GetComponent<Image>();
        _HUDItemImage = GameObject.Find("Canvas/HUD/ItemContainer/ItemImage").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void OnSubmit(BaseEventData eventData)
    {
        // TODO:
        _HUDItemImage.sprite = _itemImage.sprite;
    }

}
