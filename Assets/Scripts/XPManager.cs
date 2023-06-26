using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  : MonoBehaviour
public class XPManager
{
    // Singleton
    public static XPManager Instance;

    // Observer
    public delegate void XPChangeHandler(int amount);
    public event XPChangeHandler OnXPChange;

    public delegate void LevelUpPopupHandler(Vector3 position, int level);
    public event LevelUpPopupHandler OnLevelUpPopup;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(this);
    //    }
    //    else
    //    {
    //        Instance = this;
    //    }
    //}

    static XPManager()
    {
        Instance = new XPManager();
    }

    public void AddExperience(int amount)
    {
        OnXPChange?.Invoke(amount);
    }

    public void LevelUpPopup(Vector3 position, int level)
    {
        OnLevelUpPopup?.Invoke(position, level);
    }


}
