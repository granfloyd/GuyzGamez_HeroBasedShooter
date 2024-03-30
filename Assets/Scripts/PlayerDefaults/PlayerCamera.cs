using System;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("   CameraStuff")]
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    public Transform cameraPos;
    public Transform camOrientationy;//pivot y
    public Transform camOrientationx;
    public Transform gunOrientation;

    public float xRotation;
    public float yRotation;

    public static bool iscamset = false;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
    }
    public void SetCamera() //called in hero base
    {
        Debug.Log("Setting Camera");
        HeroBase player = PlayerController.Player;
        cameraPos = player.gameObject.transform.GetChild(2);
        camOrientationy = player.gameObject.transform;
        camOrientationx = player.gameObject.transform.GetChild(1);
        gunOrientation = player.gameObject.transform.GetChild(0);
        iscamset = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Player != null)
        {
            if (!iscamset)
                SetCamera();
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 65f);

        //rotate camera orientation
        if (iscamset)
        {
            transform.position = cameraPos.transform.position;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            camOrientationy.rotation = Quaternion.Euler(0f, yRotation, 0f);
            camOrientationx.rotation = Quaternion.Euler(xRotation, 0f, 0f);
            gunOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}