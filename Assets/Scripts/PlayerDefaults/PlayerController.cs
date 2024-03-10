using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("   PlayerController")]
    [SerializeField] public HeroBase currentHero;
    [SerializeField] public GameObject currentHeroGameObject = null;
    [SerializeField] private List<HeroBase> heroes = new List<HeroBase>();
    [SerializeField] private InputActionAsset myActions;
    [SerializeField] private InputAction ability1Action;
    [SerializeField] private InputAction ability2Action;
    [SerializeField] private InputAction ability3Action;
    [SerializeField] private InputAction changeHeroAction;

    public enum HeroIndex
    {
        DamageMain,
        TankMain,
        SupportMain,
        Default = DamageMain
    }
    public void SelectHero(HeroIndex heroindex)
    {
        HeroBase selectedHero = null;
        GameObject selectedHeroGameObject = null;
        switch (heroindex)
        {
            case HeroIndex.DamageMain:
                selectedHero = heroes[0];
                selectedHeroGameObject = heroes[0].gameObject;
                break;
            case HeroIndex.TankMain:
                selectedHero = heroes[1];
                break;
            case HeroIndex.SupportMain:
                selectedHero = heroes[2];
                break;
            default:
                selectedHero = heroes[0];
                break;
        }

        if (selectedHero == currentHero)
        {
            Debug.Log("same hero");
            return;
        }

        if (currentHero != null)
        {
            Destroy(currentHeroGameObject);
        }

        currentHero = selectedHero;
        currentHeroGameObject = selectedHeroGameObject;
        HeroBase newHero = Instantiate(currentHero, transform.position, Quaternion.identity);
        newHero.transform.SetParent(transform);
    }
    void OnEnable()
    {
        ability1Action = myActions.FindAction("Ability1");
        ability2Action = myActions.FindAction("Ability2");
        ability3Action = myActions.FindAction("Ability3");
        changeHeroAction = myActions.FindAction("ChangeHero");
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
        if(changeHeroAction != null)
        {
            changeHeroAction.performed += OnChangeHero;
            changeHeroAction.Enable();
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
        if(changeHeroAction != null)
        {
            changeHeroAction.performed -= OnChangeHero;
        }
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        Debug.Log("Ability LSHIFT activated");
        if (context.performed)
        {
            if (currentHero == null)
            {
                Debug.Log("tried to press LSHIFT Bozo didnt pick hero");
                return;
            }
            else
            {
                currentHero.Ability1();
            }
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        Debug.Log("Ability 2 E activated");
        if (context.performed)
        {
            if (currentHero == null)
            {
                Debug.Log("tried to press E Bozo didnt pick hero");
                return;
            }
            else
            {
                currentHero.Ability2();
            }
        }
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        Debug.Log("Ability 3 Q activated");
        if (context.performed)
        {
            if(currentHero == null)
            {
                Debug.Log("tried to press Q Bozo didnt pick hero");
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
        Debug.Log("Change Hero activated");
        if (context.performed)
        {
            if(HeroSelectUI.Instance.isInSpawnArea == true)
            {
                HeroSelectUI.Instance.OpenSelectHeroScreen();
            }
            else
            {
                Debug.Log("not in spawn area");
            }
        }
    }
}
