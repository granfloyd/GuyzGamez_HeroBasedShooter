using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeupDash : AbilityBase
{
    private float dashSpeed = 15f;
    private Vector3 dashDirection;
    public ChargeupDash()
    {
        cooldown = 4f;
        duration = 0.7f;
        durationTimer = 0;
        timer = 0;
        isActive = false;
    }
    public override void Use()
    {
        if (IsReady())
        {
            base.Use();
            SetDash(true);
            
        }
    }
    void SetDash(bool isLshift)
    {
        HeroBase player = PlayerController.Player;
        if (isLshift)//Lshift
        {
            isActive = true;

            if (player.horizontalInput != 0)
            {
                dashDirection = 
                    player.orientation.forward * player.verticalInput +
                    player.orientation.right * player.horizontalInput;

            }
            else
            {
                if (player.verticalInput > 0 && player.horizontalInput == 0)
                {
                    float cameraPosY = Camera.main.gameObject.transform.forward.y;
                    dashDirection.y = cameraPosY;
                }
                dashDirection = Camera.main.gameObject.transform.forward;
            }
            
        }
        else
        {
            isActive = false;
        }
    }
    public void DashMovement()
    {
        if (isActive)
        {
            HeroBase player = PlayerController.Player;
            Vector3 dashMovement = dashDirection * dashSpeed * Time.deltaTime;
            player.verticalVelocity = 0;
            player.controller.Move(dashMovement);
            player.GetComponent<Villain>().AddToRage(1);
        }
    }
    public override void End()
    {
        base.End();
        SetDash(false);
    }
    public override void AbilityUpdate()
    {   
        base.AbilityUpdate();
        DashMovement();
    }
    
    public override bool IsReady()
    {
        return base.IsReady();
    }
}
