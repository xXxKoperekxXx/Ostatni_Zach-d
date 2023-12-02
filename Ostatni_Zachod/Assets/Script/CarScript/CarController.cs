using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public abstract class CarController : MonoBehaviour
{
    [SerializeField]private WheelCollider[] _wheelColliders = new WheelCollider[4];
    [SerializeField]private Transform[] _wheelMeshes = new Transform[4];

    [SerializeField]protected float acc = 1500f;
    [SerializeField] protected float breakfocrce = 2000f;

    [SerializeField] protected float curacc = 0f;
    [SerializeField] protected float curbreak = 0f;

    [SerializeField] protected float maxTurnAngel = 25f;
    [SerializeField] protected float curTurnAngel = 0f;
    protected bool _isBreaking;
    public string id;
    
    public string Name; 
    
    public bool isOnline;

    public bool isLocalPlayer;


    [SerializeField] protected bool _isFrontDrive = false;
    private void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            Move();
        }
    }

    public void Move()
    {
        curacc = acc * Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.Space))
        {
            curbreak = breakfocrce;
            _isBreaking = true;
        }
        else
        {
            _isBreaking = false;
            curbreak = 0f;
        }

        if (!_isFrontDrive)
        {
            RearAcc();
        }
        else
        {
            FrontAcc();
        }

        Break();

        Turn();
        int i = 0;
        foreach (var wheel in _wheelColliders)
        {
            UpdateWheel(wheel,_wheelMeshes[i]);
            i++;
        }
        UpdateStatusToServer();
    }

    private void Break()
    {
        _wheelColliders[0].brakeTorque = curbreak;
        _wheelColliders[1].brakeTorque = curbreak;
    }
    
    private void FrontAcc()
    {
        _wheelColliders[0].motorTorque = curacc;
        _wheelColliders[1].motorTorque = curacc;
    }
    private void RearAcc()
    {
        _wheelColliders[2].motorTorque = curacc;
        _wheelColliders[3].motorTorque = curacc;
    }

    private void Turn()
    {
        curTurnAngel = maxTurnAngel * Input.GetAxis("Horizontal");
        _wheelColliders[0].steerAngle = curTurnAngel;
        _wheelColliders[1].steerAngle = curTurnAngel;
    }

    private void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 Pos;
        Quaternion Rot;
        col.GetWorldPose(out Pos,out Rot);
        Debug.Log("Col " +Pos + " " + Rot);
        
        trans.position = Pos;
        trans.rotation = Rot;
        Debug.Log("trans " + trans.position + " " + trans.rotation);
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
