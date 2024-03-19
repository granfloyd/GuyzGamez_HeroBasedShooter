using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("   CameraStuff")]
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    public Transform orientationy;//pivot y
    public Transform orientationx;
    public Transform gunOrientation;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 65f);

        //rotate camera orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientationy.rotation = Quaternion.Euler(0f, yRotation, 0f);
        orientationx.rotation = Quaternion.Euler(xRotation,0f, 0f);
        gunOrientation.rotation = Quaternion.Euler(xRotation,yRotation, 0f);
    }
}
