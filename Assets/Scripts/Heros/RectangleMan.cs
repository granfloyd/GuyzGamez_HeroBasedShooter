using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class RectangleMan : HeroBase
{
    private void Start()
    {
        HeroBase player = PlayerController.Player;
        if (IsOwner)
        {            
            PlayerCamera.iscamset = false;
            player.BaseAbility1 = new Grapple();
            player.BaseAbility2 = new ChainTogether();
            
            HeroUI.Instance.SetUltSlider();
            if (player == null)
            {
                Debug.LogError("Player is not set yet.");
                return;
            }
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
    protected new void Update()
    {
        base.Update();
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            player.BaseAbility1.AbilityUpdate();
            player.BaseAbility2.AbilityUpdate();
            HeroUI.Instance.UpdateAbilityCD(player.BaseAbility1, HeroUI.Instance.ability1Text);
            //HeroUI.Instance.UpdateAbilityCD(player.baseAbility2, HeroUI.Instance.ability2Text);
        }
    }
    public override void PrimaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;

        }
    }
    public override void SecondaryFire()
    {
        if (IsOwner)
        {
            HeroBase player = PlayerController.Player;
            player.BaseAbility2.Use();
            //player.BaseAbility2.End();
        }
    }

    public override void Ability1()
    {
        HeroBase player = PlayerController.Player;
        if (IsOwner)
        {
            player.BaseAbility1.Use();
            if (player.BaseAbility1.duration > 0)
            {
                Invoke("Ability1End", player.BaseAbility1.duration);
            }
        }
    }

    public override void Ability1End()
    {
        base.Ability1End();
    }

    public override void Ability2()
    {
        if (IsOwner)
        {

        }
    }

    public override void Ability2End()
    {
        if (IsOwner)
        {

        }
    }
    public override void Ability3()
    {
        if (IsOwner)
        {

        }
    }

    public override void Ability3End()
    {
        if (IsOwner)
        {

        }
    }
}
