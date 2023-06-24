using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    // Singleton
    public static XPManager Instance;

    // Observer
    public delegate void XPChangeHandler(int amout);
    public event XPChangeHandler OnXPChange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddExperience(int amount)
    {
        OnXPChange?.Invoke(amount);
    }
}
