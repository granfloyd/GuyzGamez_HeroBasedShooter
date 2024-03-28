using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleMan : HeroBase
{
    private void Start()
    {
        if (IsOwner)
        {
            PlayerController.Player.baseAbility1 = new Ability(1f, 5f);
            PlayerController.Player.baseAbility2 = new Ability(1f, 3f);
            PlayerController.Player.baseAbility3 = new Ability(1f, 10f);
            HeroBase player = PlayerController.Player;
            HeroUI.Instance.SetUltSlider();
            if (player == null)
            {
                Debug.LogError("Player is not set yet.");
                return;
            }
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

}
