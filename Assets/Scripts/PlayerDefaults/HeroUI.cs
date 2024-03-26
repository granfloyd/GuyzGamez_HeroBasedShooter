using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUI : MonoBehaviour
{
    //[SerializeField] public Slider durationSlider;
    //[SerializeField] public Slider ability3Slider;
    //public void SetDurationSlider(float duration)
    //{
    //    HeroBase player = PlayerController.Player;
    //    player.durationSlider.gameObject.SetActive(true);
    //    player.durationSlider.maxValue = duration;
    //    player.durationSlider.value = duration;
    //}
    //public void UpdateDurationSlider()
    //{
    //    HeroBase player = PlayerController.Player;
    //    if (player == null)
    //    {
    //        Debug.LogError("Player is not set yet.");
    //        return;
    //    }
    //    if (player.durationSlider.value > 0)
    //    {
    //        player.durationSlider.value -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        player.durationSlider.gameObject.SetActive(false);
    //    }
    //}

    //public void SetUltSlider(float maxcharge)
    //{
    //    HeroBase player = PlayerController.Player;
    //    player.ability3Slider.maxValue = maxcharge;
    //    player.ability3Slider.value = 0;
    //    player.ability3Charge = player.ability3Slider.value;
    //}   
    //public void UpdateUltSlider(float howmuch)
    //{
    //    HeroBase player = PlayerController.Player;
    //    Debug.Log(player.ability3Slider.value);
    //    if (player.ability3Slider.value < player.ability3Slider.maxValue)
    //    {
    //        player.ability3Slider.value += howmuch;
    //        player.ability3Charge = player.ability3Slider.value;
    //        Debug.Log(player.ability3Slider.value);
    //    }
    //}
}
