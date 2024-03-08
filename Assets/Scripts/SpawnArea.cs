using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnArea : HeroSelectUI
{
    public void EnteredSpawnArea()//safezone
    {
        RenderUI();
        
    }
    public void ExitedSpawnArea()//safezone
    {
        HideUI();
        
    }

}
