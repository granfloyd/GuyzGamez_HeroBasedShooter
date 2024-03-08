using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelectUI : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] protected GameObject selectHeroUI;
    [SerializeField] protected GameObject selectHeroScreen;

    [SerializeField] protected List<Button> heroButtons;


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
        selectHeroUI.SetActive(false);
        Debug.Log("bye bye");
    }
    public void OpenSelectHeroScreen()
    {
        selectHeroScreen.SetActive(true);
    }
    public void CloseSelectHeroScreen()
    {
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
