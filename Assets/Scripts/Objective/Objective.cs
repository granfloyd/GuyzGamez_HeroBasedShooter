using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Objective : NetworkBehaviour
{
    //[SerializeField] public float currentProgress;
    public HealthScript ObjectiveHealthScript;
    [SerializeField] private TMP_Text progressTxt;
    [SerializeField] private float minProgress;
    [SerializeField] private float maxProgress;
    public NetworkVariable<float> currentProgress = new NetworkVariable<float>(0);
    public AudioSource underAttack;
    public AudioSource defendObjective;
    public AudioSource uploadingObjective;
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeObjective();
        }
        else
        {
            currentProgress.OnValueChanged += OnProgressChange;
            InitializeObjective();
        }
    }
    void OnProgressChange(float oldProgress, float newProgress)
    {
        currentProgress.Value = newProgress;
    }


    private void InitializeObjective()
    {
        maxProgress = 500;
        minProgress = 0;
        currentProgress.Value = minProgress;
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateObjectiveServerRpc()
    {
        if (currentProgress.Value >= maxProgress)
        {
            ObjectiveCompleted();
        }
        else
        {
            if (currentProgress.Value == 0 && !uploadingObjective.isPlaying)
                uploadingObjective.Play();
            else
            {
                currentProgress.Value += Time.deltaTime;
                UpdateObjectiveClientRpc();
            }
        }
    }

    [ClientRpc]
    public void UpdateObjectiveClientRpc()
    {
        UpdateProgressTXT((int)currentProgress.Value);
    }

    public void UpdateProgressTXT(float currentProgress)
    {
        if (progressTxt != null)
        {
            progressTxt.text = currentProgress.ToString() + "%";
        }
    }
    private void ObjectiveCompleted()
    {
        Debug.Log("Objective Completed");
    }
}

