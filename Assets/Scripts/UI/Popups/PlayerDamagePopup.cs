using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDamagePopup : DamagePopup
{
    public override void Setup(int damageAmount)
    {
        base._textMesh.SetText(damageAmount.ToString());
        base._textMesh.color = Color.red;
    }
}
