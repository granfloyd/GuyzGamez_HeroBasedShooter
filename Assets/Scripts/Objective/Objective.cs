using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Objective : NetworkBehaviour
{
    [SerializeField] private float currentProgress;
    [SerializeField] private float minProgress;
    [SerializeField] private float maxProgress;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeObjective();
        }
    }
        
    private void InitializeObjective()
    {
        maxProgress = 500;
        minProgress = 0;
        currentProgress = minProgress;
        
    }

    private void Update()
    {
        //if (IsServer)
        //{
        //    UpdateObjective(0.1f);
        //}
    }
    public void UpdateObjective(float progress)
    {
        currentProgress += progress;
        if (currentProgress >= maxProgress)
        {
            ObjectiveCompleted();
        }
    }

    private void ObjectiveCompleted()
    {
        Debug.Log("Objective Completed");
    }
}

