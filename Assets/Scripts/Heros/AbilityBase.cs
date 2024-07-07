using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public abstract class AbilityBase : IAbility
{
    public float cooldown;
    public float duration;
    public float timer;
    public float durationTimer;
    public bool isActive;
    protected AbilityBase()
    {
        cooldown = 0;
        duration = 0;
        timer = cooldown; // Initialize timer to be ready for first use
        durationTimer = 0;
    }

    public virtual void UpdateTimer()
    {
        if (durationTimer > 0)
        {
            durationTimer -= Time.deltaTime;
        }
        else if (timer < cooldown)
        {
            timer += Time.deltaTime;
        }
    }
    public virtual void AbilityUpdate()
    {

        HeroBase player = PlayerController.Player;
        UpdateTimer();
        HeroUI.Instance.UpdateAbilityCD(player.BaseAbility1, HeroUI.Instance.ability1Text);
        HeroUI.Instance.UpdateAbilityCD(player.BaseAbility2, HeroUI.Instance.ability2Text);
    }
    public virtual bool IsReady()
    {
        return timer >= cooldown && durationTimer <= 0;
    }

    public virtual void Use()
    {
        if (IsReady())
        {
            
            isActive = true;
            timer = 0;
            durationTimer = duration;
        }
    }

    public virtual void End()
    {
        if(isActive)
        {
            isActive = false;
        }
    }
    public float GetCooldownTimeLeft()
    {
        float timeLeft = cooldown - timer;
        if (timeLeft > 0)
        {
            return timeLeft;
        }
        else
        {
            return 0;
        }
    }
}


