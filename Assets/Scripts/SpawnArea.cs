using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public static SpawnArea Instance;
    public Transform spawnPoint;
    public bool isInSpawnArea = false;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }   
    void Start()
    {
        //spawnPoint = transform;
    }
    public void EnteredSpawnArea()//safezone
    {
        //HeroSelectUI.Instance.RenderUI(); 
        isInSpawnArea = true;
    }
    public void ExitedSpawnArea()//safezone
    {
        //HeroSelectUI.Instance.HideUI();    
        isInSpawnArea = false;
    }

}
