using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{
    public Transform localPlayerTarget;
    
    public float damping = 5f;
    
    public Vector3 offset;
    
    public Transform cameraToTarget;
    
    void Update()
    {
        if(localPlayerTarget && cameraToTarget)
        {
            Vector3 targetPos = cameraToTarget.position + localPlayerTarget.forward * offset.z + localPlayerTarget.up * offset.y + localPlayerTarget.right * offset.x;
            
            Quaternion newRotation = Quaternion.LookRotation(cameraToTarget.position - targetPos,Vector3.up);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, damping* Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos, damping * Time.deltaTime);
        }
    }
    public void SetTarget(Transform _target, Transform _cameratotarget)
    {
        localPlayerTarget = _target;
        cameraToTarget = _cameratotarget;
    }
}
