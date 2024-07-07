using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : AbilityBase
{
    public Grapple()
    {
        cooldown = 4f;
        duration = 0f;
        timer = cooldown;
        durationTimer = 0;
        Debug.Log("Grapple created");
    }
    public override void Use()
    {
        if (IsReady())
        {
            base.Use();
        }
    }
    public override bool IsReady()
    {
        return base.IsReady();
    }
    public override void UpdateTimer()
    {
        base.UpdateTimer();
    }


}
