using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public string id;
    
    public string name; 
    
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
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        
        var x = h*Time.deltaTime * verticalSpeed;
        var y = h*Time.deltaTime * verticalSpeed;
        var z = v*Time.deltaTime * verticalSpeed;
    
        transform.Rotate(0,y,0);
        transform.Translate(0,0,z);

        UpdateStatusToServer();
        
    }
    
    void UpdateStatusToServer()
    {
        JSONObject jsonObject = new JSONObject();
        jsonObject["local_player_id"] = id;
        jsonObject["position"] = transform.position.x+":"+transform.position.y+":"+transform.position.z;
        jsonObject["rotation"] = transform.rotation.y.ToString();
        NetworkManager.instance.EmitMoveAndRotate(jsonObject);
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
