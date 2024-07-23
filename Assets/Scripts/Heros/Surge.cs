using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Surge : AbilityBase
{
    public Surge() : base()
    {
        cooldown = 1f;
        duration = 5f;
        timer = 0;
        durationTimer = 0;
        isActive = false;
    }

    public override void Use()
    {
        if (IsReady())
        {
            HeroBase player = PlayerController.Player;
            base.Use();
            Modifiers mod = player.gameObject.GetComponent<Modifiers>();
            mod.AddModifier(Modifiers.ModifierIndex.MERLIN_EDMG_MULTIPLIER);
            HeroUI.Instance.DisplayDurationSlider(duration);
        }
    }
    public override bool IsReady()
    {
        return base.IsReady();
    }
    public override void End()
    {
        base.End();
    }
    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
    }
}
