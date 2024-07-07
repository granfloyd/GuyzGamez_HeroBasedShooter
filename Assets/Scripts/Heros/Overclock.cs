using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class Overclock : AbilityBase
{
    private float dashSpeed = 18f;
    private int ultMultiplier = 2;
    private bool isBoosting = false;
    private Vector3 boostDirection;
    public Overclock() : base()
    {
        cooldown = 0.7f;
        duration = 10f;
        timer = cooldown;
        durationTimer = 0;
    }

    public override void Use()
    {
        if (IsReady())
        {
            HeroUI.Instance.ResetUltSlider();
            HeroBase player = PlayerController.Player;
            base.Use();
            boostDirection = Camera.main.gameObject.transform.up;
            isBoosting = true;
            player.isAffectedByGravity = false;
            player.isFlying = true;
            player.canGainUltCharge = false;
            player.primaryRecovery /= ultMultiplier;
            player.secondaryRecovery /= ultMultiplier;          
        }
    }
    public void BoostMovement()
    {
        if (isBoosting)
        {
            if(durationTimer >= 9.5f)
            {
                HeroBase player = PlayerController.Player;
                Vector3 boostMovement = boostDirection * dashSpeed * Time.deltaTime;
                player.controller.Move(boostMovement);
            }
            else
            {
                isBoosting = false;
            }

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
        isBoosting = false;
         
        HeroBase player = PlayerController.Player;
        player.isAffectedByGravity = true;
        player.isFlying = false;
        player.canGainUltCharge = true;
        player.primaryRecovery *= ultMultiplier;
        player.secondaryRecovery *= ultMultiplier;
        
    }
    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
        BoostMovement();
        if (duration > 0)
        {
            HeroUI.Instance.UpdateDurationSlider(durationTimer);
        }
    }
}
