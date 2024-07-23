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
    public bool isInstanced;
    protected AbilityBase()
    {
        cooldown = 0;
        duration = 0;
        timer = 0;
        durationTimer = 0;
    }

    public virtual bool IsReady()
    {
        return timer <= 0 && durationTimer <= 0;
    }

    public virtual void Use()
    {
        isActive = true;
        timer = cooldown;
        durationTimer = duration;        
    }
    public virtual void AbilityUpdate()
    {
        UpdateTimer();
    }

    public void UpdateTimer()
    {
        if (durationTimer >= 0)
        {
            durationTimer -= Time.deltaTime;
        }
        else if (timer >= 0)
        {
            timer -= Time.deltaTime;
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
        float timeLeft = timer;
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


