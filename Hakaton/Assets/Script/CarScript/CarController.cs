using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarController : MonoBehaviour
{
    [SerializeField]private WheelCollider[] _wheelColliders = new WheelCollider[4];
   

    [SerializeField]protected float acc = 1500f;
    [SerializeField] protected float breakfocrce = 2000f;

    [SerializeField] protected float curacc = 0f;
    [SerializeField] protected float curbreak = 0f;

    [SerializeField] protected float maxTurnAngel = 25f;
    [SerializeField] protected float curTurnAngel = 0f;
    protected bool _isBreaking;


    [SerializeField] protected bool _isFrontDrive = false;

    protected void Drive()
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

 
}
