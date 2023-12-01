using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarController : MonoBehaviour
{
    [SerializeField]private WheelCollider[] _wheelColliders = new WheelCollider[4];
    [SerializeField]private Transform[] _wheelMeshes = new Transform[4];

    protected float acc = 1500f;
    protected float breakfocrce = 2000f;

    protected float curacc = 0f;
    protected float curbreak = 0f;
    
    protected float maxTurnAngel = 25f;
    protected float curTurnAngel = 0f;
    protected bool _isBreaking;


    protected bool _isFrontDrive = false;
    private void FixedUpdate()
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
        trans.position = Pos;
        trans.rotation = Rot;

    }
}
