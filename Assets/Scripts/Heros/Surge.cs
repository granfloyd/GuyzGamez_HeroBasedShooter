using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surge : AbilityBase
{
    public Surge() : base()
    {
        cooldown = 7f;
        duration = 5f;
        timer = cooldown;
        durationTimer = 0;
    }

    public override void Use()
    {
        if (IsReady())
        {
            base.Use();
            HeroUI.Instance.DisplayDurationSlider(duration);
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
    public override void End()
    {
        base.End();
    }
    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
        if (duration > 0)
        {
            HeroUI.Instance.UpdateDurationSlider(durationTimer);
        }

    }
}
