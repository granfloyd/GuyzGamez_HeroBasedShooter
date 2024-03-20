using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectUI : MonoBehaviour
{
    public static HeroSelectUI Instance { get; private set; }
    public bool isInSpawnArea = false;
    [SerializeField] public PlayerController playerController;

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
                playerController.ServerSpawnHeroServerRpc(PlayerController.HeroIndex.DamageMain);
                CloseSelectHeroScreen();
                break;
            case 1:
                playerController.ServerSpawnHeroServerRpc(PlayerController.HeroIndex.TankMain);
                CloseSelectHeroScreen();
                break;
            case 2:
                playerController.ServerSpawnHeroServerRpc(PlayerController.HeroIndex.SupportMain);
                CloseSelectHeroScreen();
                break;
            default:
                Debug.Log("Invalid button index");
                break;
        }
    }
}
