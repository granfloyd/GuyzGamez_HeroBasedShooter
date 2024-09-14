using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    public TMP_Text objtxt;
    void Update()
    {
        OrientTowardsCamera();
    }

    private void OrientTowardsCamera()
    {
        if(PlayerController.Player != null)
        {
            if (PlayerController.Player.isOnObjective)
                objtxt.gameObject.SetActive(false);
            else
                objtxt.gameObject.SetActive(true);
        }       

        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
