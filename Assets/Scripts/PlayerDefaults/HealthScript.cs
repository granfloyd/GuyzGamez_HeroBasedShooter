using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;
    public bool isPlayer;
    void Start()
    {
        // At the start of the game, the enemy's health is maxed out
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.value = currentHealth;
    }
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0); ;
    }
}