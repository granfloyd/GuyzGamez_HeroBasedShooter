using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : NetworkBehaviour
{
    public int maxHealth = 100;
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100);
    public Slider healthBar;
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
        }
    }
    private void OnHealthChange(int previousHealth, int newHealth)
    {
        Debug.Log("Health changed from " + previousHealth + " to " + newHealth);
        currentHealth.Value = newHealth;
    }
    private void InitializeHealth()
    {
        currentHealth.Value = maxHealth; // Set the initial health value
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth.Value;
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
        healthBar.value = updatedHealth;
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