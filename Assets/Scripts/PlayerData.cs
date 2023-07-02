using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    // attributes
    public int xp;
    public int level;

    public PlayerData(int xp, int level)
    {
        this.xp = xp;
        this.level = level;
    }

    public override string ToString()
    {
        return $"xp: {xp}, level: {level}";
    }
}
