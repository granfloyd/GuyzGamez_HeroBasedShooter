using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    //[SerializeField] static public HeroBase Player = null;
    public static Dictionary<ulong, HeroBase> Players = new Dictionary<ulong, HeroBase>();
    public static List<HeroBase> PlayersList = new List<HeroBase>();
    public List<HeroBase> openPlayersList = new List<HeroBase>();
    public enum HeroIndex
    {
        DamageMain,
        TankMain,
        SupportMain,
        Default = DamageMain
    }
    private void Awake()
    {
        
    }
    [ServerRpc]
    public void ServerSpawnHeroServerRpc(HeroIndex heroIndex, ServerRpcParams rpcParams = default)
    {
        ClientSelectHeroClientRpc(heroIndex, rpcParams.Receive.SenderClientId);
        
        HeroBase player = Instantiate(currentHero, transform.position, Quaternion.identity);
        player.NetworkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId);
        transform.SetParent(player.transform);
        LazySet(rpcParams.Receive.SenderClientId, player);
        Players[rpcParams.Receive.SenderClientId] = player;
    }
    public void LazySet(ulong id, HeroBase player)
    {
        PlayersList.Insert((int)id,player);
    }

    private new void Update() // Use the 'new' keyword to hide the inherited member
    {
        base.Update(); // Call the base class's Update method
                       // Debug log for Players dictionary
        foreach (KeyValuePair<ulong, HeroBase> player in Players)
        {
            //.Log($"Key: {player.Key}, Value: {player.Value}");
        }
    }

    [ClientRpc]
    public void ClientSelectHeroClientRpc(HeroIndex heroIndex, ulong clientId, ClientRpcParams rpcParams = default)
    {
        SelectHero(heroIndex, clientId);
    }
    public void SelectHero(HeroIndex heroindex, ulong clientId)
    {
        Debug.Log("SelectHero called with hero index: " + heroindex + " and client ID: " + clientId);

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

        Debug.Log("Selected hero: " + selectedHero);
        Debug.Log("Current hero: " + currentHero);

        //if (NetworkManager.Singleton.IsServer)
        //{
        //    HeroBase player = Instantiate(currentHero, transform.position, Quaternion.identity);
        //    player.NetworkObject.SpawnWithOwnership(clientId);
        //    transform.SetParent(player.transform);
        //    Players[clientId] = player;
        //}
        Debug.Log(clientId + "client id spawned hero");
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
        if (context.performed)
        {
            if (!IsOwner) return;
            if (currentHero == null)
            {
                return;
            }
            else
            {
                HeroBase player;
                if (Players.TryGetValue(NetworkManager.Singleton.LocalClientId, out player))
                {
                    if (player.primaryFireTimer >= currentHero.recovery)
                    {
                        player.PrimaryFire(NetworkManager.Singleton.LocalClientId);
                        player.primaryFireTimer = 0;
                    }
                    else
                    {
                        Debug.Log(NetworkManager.Singleton.LocalClientId+ "M1 is on cooldown");
                    }
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
                HeroBase player;
                if (Players.TryGetValue(NetworkManager.Singleton.LocalClientId, out player))
                {
                    if (player.secondaryFireTimer >= currentHero.recovery)
                    {
                        player.SecondaryFire();
                        player.secondaryFireTimer = 0;
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
        if (!IsOwner) return;
        if (context.performed)
        {
            if (currentHero == null)
            {
                return;
            }
            else
            {
                //if (Player.ability1Timer >= currentHero.ability1Cooldown)
                //{
                //    Player.Ability1();
                //    Player.ability1Timer = 0;
                //}
                //else
                //{
                //    Debug.Log("Ability 1 is on cooldown");
                //}
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
                //if (Player.ability2Timer >= currentHero.ability2Cooldown)
                //{
                //    Player.Ability2();
                //    Player.ability2Timer = 0;
                //}
                //else
                //{
                //    Debug.Log("Ability 2 is on cooldown");
                //}
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
            currentHero.isMovingDown = true;
        }
        if (context.canceled)
        {
            currentHero.isMovingDown = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;
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
