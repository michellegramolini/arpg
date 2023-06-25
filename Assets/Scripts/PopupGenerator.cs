using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGenerator : MonoBehaviour
{
    //// Damage popup prefab
    //[SerializeField]
    //private Transform _pfDamagePopup;
    //// Level Up popup prefab
    //[SerializeField]
    //private Transform _pfLevelUpPopup;

    private void OnEnable()
    {
        DamageManager.Instance.OnMakePopup += GenerateDamagePopup;
        XPManager.Instance.OnLevelUpPopup += GenerateLevelUpPopup;
    }

    private void OnDisable()
    {
        DamageManager.Instance.OnMakePopup -= GenerateDamagePopup;
        XPManager.Instance.OnLevelUpPopup -= GenerateLevelUpPopup;
    }

    // TODO: move to DamagePopup, maybe interface Generate, or spawn from object pool
    private void GenerateDamagePopup(Vector3 position, int amount)
    {
        //Transform dp = Instantiate(_pfDamagePopup, position, Quaternion.identity);
        GameObject dp = ObjectPooler.Instance.SpawnFromPool("damage_popup", position, Quaternion.identity);
        dp.GetComponent<DamagePopup>().Setup(amount);
    }

    // TODO: move to DamagePopup, maybe interface Generate, or spawn from object pool
    private void GenerateLevelUpPopup(Vector3 position)
    {
        //Transform lup = Instantiate(_pfLevelUpPopup, position, Quaternion.identity);
        GameObject lup = ObjectPooler.Instance.SpawnFromPool("levelup_popup", position, Quaternion.identity);
    }
}
