using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public float cooldown { get; set; }
    public float duration { get; set; }
    public float timer { get; set; }

    public Ability(float Cooldown, float Duration)
    {
        cooldown = Cooldown;
        duration = Duration;
        timer = 0;
    }

    public void UpdateTimer()
    {
        if (timer < cooldown)
        {
            timer += Time.deltaTime;
        }
    }

    public bool IsReady()
    {
        return timer >= cooldown;
    }

    public void Use()
    {
        if (IsReady())
        {
            timer = 0;
        }
    }
}