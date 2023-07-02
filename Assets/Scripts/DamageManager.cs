using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Monobehavior
public class DamageManager
{
    // Singleton
    public static DamageManager Instance;

    // Observer
    public delegate void DamageTakenHandler(int amount);
    public event DamageTakenHandler OnDamageTaken;

    public delegate void DamagePopupHandler(Vector3 position, int amount);
    public event DamagePopupHandler OnMakePopup;

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

    static DamageManager()
    {
        Instance = new DamageManager();
    }

    public void AddDamage(int amount)
    {
        OnDamageTaken?.Invoke(amount);
    }

    public void GenerateDamagePopup(Vector3 position, int amount)
    {
        OnMakePopup?.Invoke(position, amount);
    }
}
