using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    public static HeroUI Instance { get; private set; }

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider ability3chargeSlider;
    [SerializeField] public TMP_Text displayTMP;
    [SerializeField] public Slider displaySlider;
    public TMP_Text ability1Text;
    public TMP_Text ability2Text;

    
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
    }
    public void DisplayDurationSlider(float duration)
    {
        displaySlider.gameObject.SetActive(true);
        displaySlider.maxValue = duration;
        displaySlider.value = duration;        
    }
    public void UpdateDurationSlider(float currentDuration)
    {
        displaySlider.value = currentDuration;
        if (currentDuration < 0)
        {
            displaySlider.gameObject.SetActive(false);
        }
    }

    public void RemoveDurationSlider()
    {
        displaySlider.gameObject.SetActive(false);
    }

    public void DisplayText(string displaythis)
    {
        if(displayTMP.gameObject.activeSelf == false)
        {
            displayTMP.gameObject.SetActive(true);
            displayTMP.text = displaythis;
        }
        else
        {
               displayTMP.text = displaythis;
        }
        
    }

    public void SetUltSlider()
    {
        ability3chargeSlider.maxValue = PlayerController.Player.ability3MaxCharge;
        ability3chargeSlider.value = PlayerController.Player.ability3Charge;
    }

    public void UpdateUltSlider(float howmuch)
    {
        HeroBase player = PlayerController.Player;
        if (ability3chargeSlider.value < ability3chargeSlider.maxValue && player.canGainUltCharge)
        {
            ability3chargeSlider.value += howmuch;
            player.ability3Charge = ability3chargeSlider.value;
        }
    }

    public void ResetUltSlider()
    {
        HeroBase player = PlayerController.Player;
        ability3chargeSlider.value = 0;
        player.ability3Charge = ability3chargeSlider.value;
    }

    public void UpdateAbilityCD(AbilityBase ability, TMP_Text abilityText)
    {
        float cdLeft = ability.GetCooldownTimeLeft();
        abilityText.text = ((int)cdLeft).ToString();
    }
}
