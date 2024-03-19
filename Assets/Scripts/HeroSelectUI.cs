using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectUI : NetworkBehaviour
{ 
    public static HeroSelectUI Instance { get; private set; }
    public bool isInSpawnArea = false;
    [SerializeField] public PlayerController playerController; //called in player controller start

    [SerializeField] protected GameObject selectHeroUI;
    [SerializeField] protected GameObject selectHeroScreen;

    [SerializeField] protected List<Button> heroButtons;

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
    private void Update()
    {
        if (NetworkManager.LocalClient.PlayerObject == null)
        {
            return;
        }
        else
        {
            playerController = NetworkManager.LocalClient.PlayerObject.GetComponent<PlayerController>();
        }        
        
        if (playerController != null)
        {
            if (playerController.currentHero == null)
            {
                OpenSelectHeroScreen();
            }
        }
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
                playerController.SelectHero(PlayerController.HeroIndex.DamageMain);
                CloseSelectHeroScreen();
                break;
            case 1:
                playerController.SelectHero(PlayerController.HeroIndex.TankMain);
                CloseSelectHeroScreen();
                break;
            case 2:
                playerController.SelectHero(PlayerController.HeroIndex.SupportMain);
                CloseSelectHeroScreen();
                break;
            default:
                Debug.Log("Invalid button index");
                break;
        }
    }
}
