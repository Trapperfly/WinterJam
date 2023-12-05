using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    [SerializeField] float sensitivityY;
    [SerializeField] float sensitivityX;

    [SerializeField] Transform cameraRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        float yRot = 0;
        yRot += mouseX;
        float xRot = 0;
        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -90, 90);

        cameraRot.rotation = Quaternion.Euler(xRot, yRot, 0);
        transform.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
