using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
    public static HeroUI Instance { get; private set; }
    [Header("Player Stuff")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider ability3chargeSlider;

    [Header("Duration Slider")]
    [SerializeField] public Slider durationSlider;

    [Header("Abiliy CDs")]
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
    public void SetDurationSlider(Ability ability)
    {
        durationSlider.maxValue = ability.duration;
        durationSlider.value = ability.duration;
        durationSlider.gameObject.SetActive(true);
    }
    public void UpdateDurationSlider(Ability ability)
    {
        if (durationSlider.value > 0)
        {
            durationSlider.value -= Time.deltaTime;
        }
        else
        {
            durationSlider.gameObject.SetActive(false);
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
        if (ability3chargeSlider.value < ability3chargeSlider.maxValue)
        {
            ability3chargeSlider.value += howmuch;
            player.ability3Charge = ability3chargeSlider.value;
        }
    }

    public void UpdateAbilityCD(Ability ability, TMP_Text abilityText)
    {
        float cdLeft = ability.GetCooldownTimeLeft();
        abilityText.text = ((int)cdLeft).ToString();
    }
}
