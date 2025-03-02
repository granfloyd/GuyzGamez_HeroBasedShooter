using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [Header("   PlayerController")]
    [SerializeField] static public HeroBase currentHero;
    [SerializeField] static public HeroBase Player = null;
    
    [SerializeField] private InputActionAsset myActions;
    [SerializeField] private InputAction ability1Action;
    [SerializeField] private InputAction ability2Action;
    [SerializeField] private InputAction ability3Action;
    [SerializeField] private InputAction crouchAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction changeHeroAction;
    [SerializeField] private InputAction primaryFireAction;
    [SerializeField] private InputAction secondaryFireAction;

    private bool isPrimaryFireButtonDown = false;
    private bool isSecondaryFireButtonDown = false;
    public enum HeroIndex
    {
        DamageMain,
        TankMain,
        SupportMain,
        Default = DamageMain
    }
    private void Awake()
    {
        Player = null;
    }
    private void Update()
    {
        if(isPrimaryFireButtonDown)
        {
            OnPrimaryFire(new InputAction.CallbackContext());
        }
        if (isSecondaryFireButtonDown)
        {
            OnSecondaryFire(new InputAction.CallbackContext());
        }
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isPrimaryFireButtonDown = true;
        }
        else if (context.canceled)
        {
            isPrimaryFireButtonDown = false;
        }
        else if(isPrimaryFireButtonDown)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player != null)
                {
                    if (Player.primaryFireTimer >= Player.primaryRecovery)
                    {
                        Player.PrimaryFire();
                        Player.primaryFireTimer = 0;
                    }
                    else
                    {
                        //Debug.Log(NetworkManager.Singleton.LocalClientId + "M1 is on cooldown");
                    }
                }
            }
        }
    }
    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSecondaryFireButtonDown = true;
        }
        else if (context.canceled)
        {
            isSecondaryFireButtonDown = false;
        }
        else if (isSecondaryFireButtonDown)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player != null)
                {
                    if (Player.secondaryFireTimer >= Player.secondaryRecovery)
                    {
                        Player.SecondaryFire();
                        Player.secondaryFireTimer = 0;
                    }
                    else
                    {
                        //Debug.Log(NetworkManager.Singleton.LocalClientId + "M2 is on cooldown");
                    }
                }
            }
        }
    }
    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player == null || Player.BaseAbility1 == null)
            {
                Debug.LogError("Player or Ability1 is not set yet.");
                return;
            }
            if (Player.BaseAbility1.IsReady())
            {
                Player.Ability1();
            }
            else
            {
                Debug.Log("Ability 1 is on cooldown");
            }
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player == null || Player.BaseAbility2 == null)
            {
                Debug.LogError("Player or Ability2 is not set yet.");
                return;
            }
            if (Player.BaseAbility2.IsReady())
            {
                Player.Ability2();
            }
            else
            {
                Debug.Log("Ability 2 is on cooldown");
            }
        }
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player == null || Player.BaseAbility3 == null)
            {
                Debug.LogError("Player or Ability3 is not set yet.");
                return;
            }
            if (Player.BaseAbility3.IsReady())
            {
                Player.Ability3();
            }
            else
            {
                Debug.Log("Ability 3 is on cooldown");
            }
        }
    }
    public void OnChangeHero(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (SpawnArea.Instance.isInSpawnArea == true)
            {
                HeroSelectUI.Instance.OpenSelectHeroScreen();
            }
            else
            {
                Debug.Log("not in spawn area");
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player.isFlying)
            {
                Player.isMovingDown = true;
            }
        }
        if (context.canceled || !Player.isFlying)
        {
            if(Player.isFlying)
            {
                Player.isMovingDown = false;
            }
            else
            {
                Player.isMovingDown = false;
            }
            
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(Player.isFlying)
            {
                Player.isMovingUp = true;
            }            
        }

        if (context.canceled || !Player.isFlying)
        {
            Player.isMovingUp = false;
        }
    }
    void OnEnable()
    {
        ability1Action = myActions.FindAction("Ability1");
        ability2Action = myActions.FindAction("Ability2");
        ability3Action = myActions.FindAction("Ability3");
        changeHeroAction = myActions.FindAction("ChangeHero");
        crouchAction = myActions.FindAction("Crouch");
        jumpAction = myActions.FindAction("Jump");
        primaryFireAction = myActions.FindAction("PrimaryFire");
        secondaryFireAction = myActions.FindAction("SecondaryFire");
        // Hook up the functions to the actions
        if (ability1Action != null)
        {
            ability1Action.performed += OnAbility1;
            ability1Action.Enable();
        }
        if (ability2Action != null)
        {
            ability2Action.performed += OnAbility2;
            ability2Action.Enable();
        }
        if (ability3Action != null)
        {
            ability3Action.performed += OnAbility3;
            ability3Action.Enable();
        }
        if (changeHeroAction != null)
        {
            changeHeroAction.performed += OnChangeHero;
            changeHeroAction.Enable();
        }
        if (crouchAction != null)
        {
            crouchAction.performed += OnCrouch;
            crouchAction.canceled += OnCrouch;
            crouchAction.Enable();
        }
        if (jumpAction != null)
        {
            jumpAction.performed += OnJump;
            jumpAction.canceled += OnJump;
            jumpAction.Enable();
        }
        if (primaryFireAction != null)
        {
            primaryFireAction.performed += OnPrimaryFire;
            primaryFireAction.canceled += OnPrimaryFire;
            primaryFireAction.Enable();
        }
        if (secondaryFireAction != null)
        {
            secondaryFireAction.performed += OnSecondaryFire;
            secondaryFireAction.canceled += OnSecondaryFire;
            secondaryFireAction.Enable();
        }

    }
    void OnDisable()
    {
        // Clean up - remove the functions from the actions
        if (ability1Action != null)
        {
            ability1Action.performed -= OnAbility1;
        }
        if (ability2Action != null)
        {
            ability2Action.performed -= OnAbility2;
        }
        if (ability3Action != null)
        {
            ability3Action.performed -= OnAbility3;
        }
        if (changeHeroAction != null)
        {
            changeHeroAction.performed -= OnChangeHero;
        }
        if (crouchAction != null)
        {
            crouchAction.performed -= OnCrouch;
            crouchAction.canceled -= OnCrouch;
        }
        if (jumpAction != null)
        {
            jumpAction.performed -= OnJump;
            jumpAction.canceled -= OnJump;
        }
        if (primaryFireAction != null)
        {
            primaryFireAction.performed -= OnPrimaryFire;
            primaryFireAction.canceled -= OnPrimaryFire;
        }
        if (secondaryFireAction != null)
        {
            secondaryFireAction.performed -= OnSecondaryFire;
            secondaryFireAction.canceled -= OnSecondaryFire;
        }
    }
}
