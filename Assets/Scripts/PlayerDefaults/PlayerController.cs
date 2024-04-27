using System;
using System.Collections.Generic;
using System.Linq;
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
        //if (Player != null)
        //{
        //    Debug.Log(Player.gameObject.name);
        //}
        //else
        //{
        //    Debug.Log("Player is null");    
        //}
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player != null)
                {
                    if (Player.primaryFireTimer >= currentHero.recovery)
                    {
                        Player.PrimaryFire();
                        Player.primaryFireTimer = 0;
                    }
                    else
                    {
                        Debug.Log(NetworkManager.Singleton.LocalClientId + "M1 is on cooldown");
                    }
                }
            }
        }
    }
    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player != null)
                {
                    if (Player.secondaryFireTimer >= currentHero.recovery)
                    {
                        Player.SecondaryFire();
                        Player.secondaryFireTimer = 0;
                    }
                    else
                    {
                        Debug.Log(" M2 is on cooldown");
                    }
                }
            }
        }
    }
    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Player == null || Player.baseAbility1 == null)
            {
                Debug.LogError("Player or Ability1 is not set yet.");
                return;
            }
            if (Player.baseAbility1.IsReady())
            {
                Player.Ability1();
                Player.baseAbility1.Use();
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
            if (Player == null || Player.baseAbility2 == null)
            {
                Debug.LogError("Player or Ability2 is not set yet.");
                return;
            }
            if (Player.baseAbility2.IsReady())
            {
                Player.Ability2();
                Player.baseAbility2.Use();
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
            if(currentHero == null)
            {
                return;
            }
            else
            {

                currentHero.Ability3();
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
        if (context.started)
        {
            currentHero.isMovingDown = true;
        }
        if (context.canceled)
        {
            currentHero.isMovingDown = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentHero.isMovingUp = true;
        }
        if (context.canceled)
        {
            currentHero.isMovingUp = false;
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
            crouchAction.started += OnCrouch;
            crouchAction.canceled += OnCrouch;
            crouchAction.Enable();
        }
        if (jumpAction != null)
        {
            jumpAction.started += OnJump;
            jumpAction.canceled += OnJump;
            jumpAction.Enable();
        }
        if (primaryFireAction != null)
        {
            primaryFireAction.performed += OnPrimaryFire;
            primaryFireAction.Enable();
        }
        if (secondaryFireAction != null)
        {
            secondaryFireAction.performed += OnSecondaryFire;
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
            crouchAction.started -= OnCrouch;
            crouchAction.canceled -= OnCrouch;
        }
        if (jumpAction != null)
        {
            jumpAction.started -= OnJump;
            jumpAction.canceled -= OnJump;
        }
        if (primaryFireAction != null)
        {
            primaryFireAction.performed -= OnPrimaryFire;
        }
        if (secondaryFireAction != null)
        {
            secondaryFireAction.performed -= OnSecondaryFire;
        }
    }
}
