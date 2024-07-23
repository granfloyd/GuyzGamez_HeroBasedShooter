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
        duration = 15f;
        durationTimer = 0;
        timer = 0;       
        isActive = false;
    }

    public override void Use()
    {
        if (IsReady())
        {
            HeroBase player = PlayerController.Player;
            base.Use();
            HeroUI.Instance.DisplayDurationSlider(duration);
            HeroUI.Instance.ResetUltSlider();
            boostDirection = Camera.main.gameObject.transform.up;
            isBoosting = true;
            player.isAffectedByGravity = false;
            player.isFlying = true;
            player.canGainUltCharge = false;
            player.primaryRecovery /= ultMultiplier;
            player.secondaryRecovery /= ultMultiplier;     
            player.BaseAbility1.cooldown = 0f;
        }
    }
    public void BoostMovement()
    {
        if (isBoosting)
        {
            if(durationTimer >= 14.5f)
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
        player.BaseAbility1.cooldown = 4f;

    }
    public override void AbilityUpdate()
    {
        base.AbilityUpdate();
        BoostMovement();
    }
}
