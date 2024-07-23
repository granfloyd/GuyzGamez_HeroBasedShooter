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
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Update()
    {
        if (PlayerController.Player != null)
        {
            UpdateDurationSlider();
        }        
    }
    public void DisplayDurationSlider(float duration)
    {
        displaySlider.maxValue = duration;
        displaySlider.value = duration;        
        displaySlider.gameObject.SetActive(true);
    }
    public void UpdateDurationSlider()
    {
        if (displaySlider.gameObject.activeSelf)
        {
            displaySlider.value -= Time.deltaTime;
            if (displaySlider.value <= 0)
            {
                displaySlider.gameObject.SetActive(false);
            }
        }
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
