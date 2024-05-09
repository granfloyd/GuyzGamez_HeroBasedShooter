using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Modifiers : MonoBehaviour
{
    public enum ModifierIndex
    {
        MERLIN_EDMG_MULTIPLIER,//100% dmg bonus / 2x dmg 
        SOMETHINGELSE_DEBUFF//idk maybe like 50% less dmg 
    }

    public List<ModifierIndex> modifiersList = new List<ModifierIndex>();
    public int ApplyToDamage(int damage)
    {
        int modifiedDamage = damage;
        modifiedDamage = (int)(modifiedDamage * SetDamageModifier());

        return modifiedDamage;
    }

    public void AddModifier(ModifierIndex id)
    {
        if (!modifiersList.Contains(id))
        modifiersList.Add(id);
    }
    public void RemoveModifier(ModifierIndex id) { modifiersList.Remove(id);}

    public float SetDamageModifier()
    {
        float dmgModifier = 1f;
        foreach (ModifierIndex id in modifiersList)
        {
            switch (id)
            {
                case ModifierIndex.MERLIN_EDMG_MULTIPLIER:
                    dmgModifier *= ModifierSignifiers.MERLIN_EDMG_MULTIPLIER;
                    break;
                case ModifierIndex.SOMETHINGELSE_DEBUFF:
                    dmgModifier *= ModifierSignifiers.SOMETHINGELSE_DEBUFF;
                    break;
            }
        }
        return dmgModifier;
    }

}
public static class ModifierSignifiers
{
    public const float MERLIN_EDMG_MULTIPLIER = 1f;
    public const float SOMETHINGELSE_DEBUFF = 0.50f;
}