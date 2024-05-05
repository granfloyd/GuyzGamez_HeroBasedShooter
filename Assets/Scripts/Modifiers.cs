using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : MonoBehaviour
{
    
    public bool isStatsChanged = false;
    public int ApplyModifier(int damage)
    {
        int modifiedDamage = damage;

            
        modifiedDamage = (int)(modifiedDamage * ModifierSignifiers.MERLIN_EDMG_MULTIPLIER);

        return modifiedDamage;
    }

    private void SetModifier(ModifierSignifiers.ModifierIndex id)
    {
        switch(id)
        {
            case ModifierSignifiers.ModifierIndex.MERLIN_EDMG_MULTIPLIER:

                break;


        }

    }

}
public static class ModifierSignifiers
{
    public const float MERLIN_EDMG_MULTIPLIER = 2f;
    public enum ModifierIndex
    {
        MERLIN_EDMG_MULTIPLIER,//100% dmg bonus / 2x dmg 
        SOMETHINGELSE_DEBUFF//idk maybe like 50% less dmg 
    }
}