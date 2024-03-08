using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : HeroBase
{
    [Header("   PlayerController")]
    [SerializeField] protected HeroBase currentHero;
    [SerializeField] private List<HeroBase> heroes = new List<HeroBase>();
    [SerializeField] private InputAction ability1Action;
    [SerializeField] private InputAction ability2Action;
    [SerializeField] private InputAction ability3Action;
    public enum HeroIndex
    {
        DamageMain,
        TankMain,
        SupportMain,
        Default = DamageMain
    }
    void Awake()
    {
        ability1Action = new InputAction(binding: "<Keyboard>/leftShift");
        ability1Action.performed += OnAbility1;

        ability2Action = new InputAction(binding: "<Keyboard>/e");
        ability2Action.performed += OnAbility2;

        ability3Action = new InputAction(binding: "<Keyboard>/q");
        ability3Action.performed += OnAbility3;
    }
    public void SelectHero(HeroIndex heroindex)
    {
        switch (heroindex)
        {
            case HeroIndex.DamageMain:
                currentHero = heroes[0];
                break;
            case HeroIndex.TankMain:
                currentHero = heroes[1];
                break;
            case HeroIndex.SupportMain:
                currentHero = heroes[2];
                break;
            default:
                currentHero = heroes[0];
                break;
        }
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

    public override void CollisionEnter(Collider other)
    {
        if (other.tag != CollisionPlayer.SpawnCollision)
        {
            Debug.Log(other.tag);
            return;
        }
        else
        {
            
            SpawnArea spawnArea = other.GetComponentInParent<SpawnArea>();
            spawnArea.EnteredSpawnArea(); 
            Debug.Log("in spawn");
        }
    }
    public override void CollisionExit(Collider other)
    {
        if (other.tag != CollisionPlayer.SpawnCollision)
        {
            Debug.Log(other.tag);
            return;
        }
        else
        {
            
            Debug.Log("out spawn");
        }
    }
}
