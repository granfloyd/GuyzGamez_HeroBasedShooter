using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : MonoBehaviour
{
    public const float MERLIN_BONK_MULTIPLIER = 2f;

    public bool isBonk = false;
    public int ApplyModifier(int damage)
    {
        int modifiedDamage = damage;

        if (isBonk)
            modifiedDamage = (int)(modifiedDamage * MERLIN_BONK_MULTIPLIER);

        return modifiedDamage;
    }

}
