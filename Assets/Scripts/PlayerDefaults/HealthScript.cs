using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : NetworkBehaviour
{
    public int maxHealth;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(500);
    public TMP_Text healthText; // Replace Slider with TMP_Text
    public bool isPlayer;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            InitializeHealth();
        }
        else
        {
            currentHealth.OnValueChanged += OnHealthChange;
            InitializeHealth();
        }
    }

    private void OnHealthChange(int previousHealth, int newHealth)
    {
        Debug.Log("Health changed from " + previousHealth + " to " + newHealth);
        healthText.text = newHealth.ToString(); // Update the text with the new health value
    }

    private void InitializeHealth()
    {
        currentHealth.Value = maxHealth; // Set the initial health value
        healthText.text = currentHealth.Value.ToString(); // Update the text with the initial health value
    }

    public void CalculateDamage(int damage)
    {
        Debug.Log("Calculating damage");
        ApplyDamageServerRpc(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ApplyDamageServerRpc(int damage)
    {
        if (!IsServer) return;
        Debug.Log("Applying damage on server");
        currentHealth.Value -= damage;
        UpdateClientHealthClientRpc(currentHealth.Value);
    }

    [ClientRpc]
    private void UpdateClientHealthClientRpc(int updatedHealth)
    {
        Debug.Log("Updating client health: " + updatedHealth);
        healthText.text = updatedHealth.ToString(); // Update the text with the updated health value
    }

    void Update()
    {
        OrientTowardsCamera();
    }

    private void OrientTowardsCamera()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}