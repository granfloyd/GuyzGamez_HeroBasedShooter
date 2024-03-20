using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PlayerMovement
{
    [Header("   PlayerController")]
    [SerializeField] public HeroBase currentHero;
    [SerializeField] public GameObject currentHeroGameObject = null;
    [SerializeField] private List<HeroBase> heroes = new List<HeroBase>();
    [SerializeField] private InputActionAsset myActions;
    [SerializeField] private InputAction ability1Action;
    [SerializeField] private InputAction ability2Action;
    [SerializeField] private InputAction ability3Action;
    [SerializeField] private InputAction crouchAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction changeHeroAction;
    [SerializeField] private InputAction primaryFireAction;
    [SerializeField] private InputAction secondaryFireAction;
    [SerializeField] static public HeroBase Player = null;
    [SerializeField] private NetworkManager networkManager;
    private void Awake()
    {
        networkManager = NetworkManager.Singleton;
        Player = null;       
    }
   
    public enum HeroIndex
    {
        DamageMain,
        TankMain,
        SupportMain,
        Default = DamageMain
    }
    [ServerRpc]
    public void ServerSpawnHeroServerRpc(HeroIndex heroIndex, ServerRpcParams rpcParams = default)
    {
        SelectHero(heroIndex, rpcParams.Receive.SenderClientId);
    }
    public void SelectHero(HeroIndex heroindex, ulong clientId)
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
                selectedHeroGameObject = heroes[1].gameObject;
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
        Player = Instantiate(currentHero, transform.position, Quaternion.identity);
        Player.NetworkObject.SpawnWithOwnership(clientId);
        transform.SetParent(Player.transform);
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
        if(changeHeroAction != null)
        {
            changeHeroAction.performed += OnChangeHero;
            changeHeroAction.Enable();
        }
        if(crouchAction != null)
        {
            crouchAction.started += OnCrouch;
            crouchAction.canceled += OnCrouch;
            crouchAction.Enable();
        }
        if(jumpAction != null)
        {
            jumpAction.started += OnJump;
            jumpAction.canceled += OnJump;
            jumpAction.Enable();
        }   
        if(primaryFireAction != null)
        {
            primaryFireAction.performed += OnPrimaryFire;
            primaryFireAction.Enable();
        }
        if(secondaryFireAction != null)
        {
            secondaryFireAction.performed += OnSecondaryFire;
            secondaryFireAction.Enable();
        }

    }
    private void Update()
    {
        if (Player == null)
        {
            return;
        }
        else
        {
            Player.MyInput();
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
        if(crouchAction != null)
        {
            crouchAction.started -= OnCrouch;
            crouchAction.canceled -= OnCrouch;
        }
        if(jumpAction != null)
        {
            jumpAction.started -= OnJump;
            jumpAction.canceled -= OnJump;
        }  
        if(primaryFireAction != null)
        {
            primaryFireAction.performed -= OnPrimaryFire;
        }
        if(secondaryFireAction != null)
        {
            secondaryFireAction.performed -= OnSecondaryFire;
        }
    }
    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player.primaryFireTimer >= currentHero.recovery)
                {
                    ulong clientId = NetworkManager.Singleton.LocalClientId;
                    Player.PrimaryFire(clientId);
                    Player.primaryFireTimer = 0;
                }
                else
                {
                    Debug.Log("M1 is on cooldown");
                }
            }
        }
    }   
    public void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player.secondaryFireTimer >= currentHero.recovery)
                {
                    Player.SecondaryFire();
                    Player.secondaryFireTimer = 0;
                }
                else
                {
                    Debug.Log("M2 is on cooldown");
                }
            }
        }
    }
    public void OnAbility1(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player.ability1Timer >= currentHero.ability1Cooldown)
                {
                    Player.Ability1();
                    Player.ability1Timer = 0;
                }
                else
                {
                    Debug.Log("Ability 1 is on cooldown");
                }
            }
        }
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                if (Player.ability2Timer >= currentHero.ability2Cooldown)
                {
                    Player.Ability2();
                    Player.ability2Timer = 0;
                }
                else
                {
                    Debug.Log("Ability 2 is on cooldown");
                }
            }
        }
    }

    public void OnAbility3(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
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
        if (!IsOwner) return;
        if (context.performed)
        {
            if (HeroSelectUI.Instance.isInSpawnArea == true)
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
        if (!IsOwner) return;
        if (context.started)
        {
            Player.isMovingDown = true;
        }
        if (context.canceled)
        {
            Player.isMovingDown = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.started)
        {
            Player.isMovingUp = true;
        }
        if (context.canceled)
        {
            Player.isMovingUp = false;
        }
    }   
}
