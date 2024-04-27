using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public float cooldown { get; set; }
    public float duration { get; set; }
    public float timer { get; set; }
    public float durationTimer { get; set; }
    public Ability(float Cooldown, float Duration)
    {
        cooldown = Cooldown;
        duration = Duration;
        timer = cooldown;
        durationTimer = 0;
    }

    public void UpdateTimer()
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

    public bool IsReady()
    {
        return timer >= cooldown && durationTimer <= 0;
    }

    public void Use()
    {
        if (IsReady())
        {
            timer = 0;
            durationTimer = duration;
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