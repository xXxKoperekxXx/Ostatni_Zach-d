using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class PlayerManager : CarController
{
    public string id;
    
    public string Name; 
    
    public bool isOnline;

    public bool isLocalPlayer;
    
    public Transform cameraToTarget;
    
    public float verticalSpeed = 3.0f;
    public float rotateSpeed = 150f;
    
    float v,h;
    
    
    public void Set3DName(string name)
    {
    //	GetComponentInChildren<TextMesh>().text = name;
    }
    
    void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            Move();
        }
    }
    void Move()
    {
        base.Drive();

        UpdateStatusToServer();
        
    }
    
    void UpdateStatusToServer()
    {
        JSONObject jsonObject = new JSONObject();
        jsonObject["local_player_id"] = id;
        jsonObject["position"] = transform.position.x+":"+transform.position.y+":"+transform.position.z;
        jsonObject["rotation"] = transform.rotation.y.ToString();
        NetWorkManager.instance.EmitMoveAndRotate(jsonObject);
    }
    
    public void UpdatePosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, position.z);
    }
    
    public void UpdateRotation(Quaternion _rotation)
    {
        transform.rotation = _rotation;    
    }

}
