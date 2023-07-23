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

    public delegate void DamagePopupHandler(string poolTag, Vector3 position, int amount);
    public event DamagePopupHandler OnMakePopup;

    static DamageManager()
    {
        Instance = new DamageManager();
    }

    public void AddDamage(int amount)
    {
        OnDamageTaken?.Invoke(amount);
    }

    public void GenerateDamagePopup(string poolTag, Vector3 position, int amount)
    {
        OnMakePopup?.Invoke(poolTag, position, amount);
    }
}
