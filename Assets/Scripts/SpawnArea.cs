using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnArea : MonoBehaviour//HeroSelectUI
{
    public void EnteredSpawnArea()//safezone
    {
        HeroSelectUI.Instance.RenderUI(); 
        HeroSelectUI.Instance.isInSpawnArea = true;
    }
    public void ExitedSpawnArea()//safezone
    {
        HeroSelectUI.Instance.HideUI();    
        HeroSelectUI.Instance.isInSpawnArea = false;
    }

}
