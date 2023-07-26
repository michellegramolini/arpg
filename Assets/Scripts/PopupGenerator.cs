using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGenerator : MonoBehaviour
{
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
    private void GenerateDamagePopup(string tag, Vector3 position, int amount)
    {
        //Transform dp = Instantiate(_pfDamagePopup, position, Quaternion.identity);
        GameObject dp = ObjectPooler.Instance.SpawnFromPool(tag, position, Quaternion.identity);
        dp.GetComponent<DamagePopup>().Setup(amount);
    }

    // TODO: move to DamagePopup, maybe interface Generate, or spawn from object pool
    private void GenerateLevelUpPopup(Vector3 position, int level)
    {
        //Transform lup = Instantiate(_pfLevelUpPopup, position, Quaternion.identity);
        GameObject lup = ObjectPooler.Instance.SpawnFromPool("levelup_popup", position, Quaternion.identity);
        lup.GetComponent<LevelUpPopup>().Setup(level);
    }
}
