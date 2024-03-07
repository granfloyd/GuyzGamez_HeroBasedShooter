using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : HeroBase
{
    private InputAction ability1Action;
    private InputAction ability2Action;
    private InputAction ability3Action;

    void Awake()
    {
        ability1Action = new InputAction(binding: "<Keyboard>/e");
        ability1Action.performed += OnAbility1;

        ability2Action = new InputAction(binding: "<Keyboard>/q");
        ability2Action.performed += OnAbility2;

        ability3Action = new InputAction(binding: "<Keyboard>/leftShift");
        ability3Action.performed += OnAbility3;
    }

    void OnEnable()
    {
        ability1Action.Enable();
        ability2Action.Enable();
        ability3Action.Enable();
    }

    void OnDisable()
    {
        ability1Action.Disable();
        ability2Action.Disable();
        ability3Action.Disable();
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        Debug.Log("Ability 1 activated");
        if (context.performed)
        {
            Debug.Log("E key pressed");
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        Debug.Log("Ability 2 activated");
        if (context.performed)
        {
            Debug.Log("Q key pressed");
        }
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        Debug.Log("Ability 3 activated");
        if (context.performed)
        {
            Debug.Log("Left Shift key pressed");
        }
    }
}
