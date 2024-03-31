using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectUI : NetworkBehaviour
{
    //cool names Gaius,Jericho,Keturah,Tirzah,Eden
    //IMPORTANT SORT ORDER FOR CANVAS
    // 5 = MOST IMPORTANT (HERO SELECT UI) OR ERROR MESSAGE
    //4 = HERO UI
    //3IDK
    //2IDK
    //1IDK
    //0 IDK
    public static HeroSelectUI Instance { get; private set; }
    [SerializeField] private List<HeroBase> heroes = new List<HeroBase>();
    [SerializeField] protected GameObject selectHeroUI;
    [SerializeField] protected GameObject selectHeroScreen;
    [SerializeField] protected List<Button> heroButtons;
    public HeroBase bussy;
    public HeroBase newhero;
    private void Awake()
    {
        // If an instance already exists, destroy this one
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // Set this as the instance
        Instance = this;  
        
    }
    private void Start()
    {
        for (int i = 0; i < heroButtons.Count; i++)
        {
            int index = i; // To avoid the issue with modified closure in C#
            heroButtons[i].onClick.AddListener(() => ButtonClicked(index));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ServerDespawnHeroServerRpc(ulong heroId, ServerRpcParams rpcParams = default)
    {
        var heroToDespawn = NetworkManager.Singleton.SpawnManager.SpawnedObjects[heroId].GetComponent<HeroBase>();
        heroToDespawn.NetworkObject.Despawn(true);
    }

    [ServerRpc(RequireOwnership = false)]//so client can use should come back here later to change
    public void ServerSpawnHeroServerRpc(PlayerController.HeroIndex heroIndex, ServerRpcParams rpcParams = default)
    {
        ClientSelectHeroClientRpc(heroIndex, rpcParams.Receive.SenderClientId);
        // Instantiate the selected hero
        HeroBase instantiatedHero = Instantiate(PlayerController.currentHero, //1. set hero
            SpawnArea.Instance.spawnPoint.position, //2. set position
            Quaternion.identity);                  //3. rotation
                                                   // Set ownership to the client that requested the spawn
        instantiatedHero.NetworkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId);

        // Assign the instantiated hero to the Player variable of the client that instantiated the hero
        ClientSlutPlayerClientRpc(instantiatedHero.NetworkObject.NetworkObjectId, rpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    public void ClientSelectHeroClientRpc(PlayerController.HeroIndex heroIndex, ulong clientId, ClientRpcParams rpcParams = default)
    {
        SelectHero(heroIndex, clientId);
    }

    [ClientRpc]
    private void ClientSlutPlayerClientRpc(ulong spawnheroId, ulong clientId, ClientRpcParams rpcParams = default)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            Debug.Log("setting player");
            var spawnhero = NetworkManager.Singleton.SpawnManager.SpawnedObjects[spawnheroId].GetComponent<HeroBase>();
            PlayerController.Player = spawnhero;
        }
    }
    public void SelectHero(PlayerController.HeroIndex heroindex, ulong clientId)
    {
        Debug.Log("SelectHero called with hero index: " + heroindex + " and client ID: " + clientId);

        HeroBase selectedHero = null;

        switch (heroindex)
        {
            case PlayerController.HeroIndex.DamageMain:
                selectedHero = heroes[0];
                break;
            case PlayerController.HeroIndex.TankMain:
                selectedHero = heroes[1];
                break;
            case PlayerController.HeroIndex.SupportMain:
                selectedHero = heroes[2];
                break;
        }

        if (selectedHero == PlayerController.currentHero)
        {
            Debug.Log("same hero");
        }
       
        PlayerController.currentHero = selectedHero;
        bussy = PlayerController.currentHero;
    }
    public void RenderUI()
    {
        selectHeroUI.SetActive(true);
    }
    public void HideUI()
    {
        selectHeroUI.SetActive(false);        
    }
    public void OpenSelectHeroScreen()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        selectHeroScreen.SetActive(true);
    }
    public void CloseSelectHeroScreen()
    {
        Cursor.visible = false;
        selectHeroScreen.SetActive(false);
    }
    private void ButtonClicked(int buttonIndex)
    {

        switch (buttonIndex)
        {
            case 0:
                if(PlayerController.Player != null)
                ServerDespawnHeroServerRpc(PlayerController.Player.NetworkObject.NetworkObjectId);
                ServerSpawnHeroServerRpc(PlayerController.HeroIndex.DamageMain);
                Debug.Log("press1");
                CloseSelectHeroScreen();
                break;
            case 1:
                if (PlayerController.Player != null)
                    ServerDespawnHeroServerRpc(PlayerController.Player.NetworkObject.NetworkObjectId);
                ServerSpawnHeroServerRpc(PlayerController.HeroIndex.TankMain);
                Debug.Log("press2");
                CloseSelectHeroScreen();
                break;
            case 2:
                if (PlayerController.Player != null)
                    ServerDespawnHeroServerRpc(PlayerController.Player.NetworkObject.NetworkObjectId);
                ServerSpawnHeroServerRpc(PlayerController.HeroIndex.SupportMain);
                Debug.Log("press3");
                CloseSelectHeroScreen();
                break;
            default:
                Debug.Log("Invalid button index");
                break;
        }
    }
}
