using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectUI : MonoBehaviour
{
    public static HeroSelectUI Instance { get; private set; }
    public bool isInSpawnArea = false;
    [SerializeField] private PlayerController playerController;

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
        if(playerController.currentHero == null)
        {
            OpenSelectHeroScreen();
        }
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
        Debug.Log("Hallo");
    }
    public void HideUI()
    {
        Debug.Log("bye bye");
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
