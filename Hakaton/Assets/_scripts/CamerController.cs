using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
  public float sensY = 400f;
  public float sensX = 400f;
  public Transform orientation;
  private float xRotation;
  private float yRotation;
  [SerializeField] private Transform camHolder;
   private void Start()
   {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
   }
   private void Update()
   {
    float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
    float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
    yRotation += mouseX;
    
    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -5f, 45f);
    
    camHolder.transform.rotation = Quaternion.Euler(xRotation , yRotation , 0);
    orientation.rotation = Quaternion.Euler(0 , yRotation ,0);
   }
}
