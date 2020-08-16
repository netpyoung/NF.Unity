using NF.Common.PIDControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ex_pidcontroller : MonoBehaviour
{
    public Transform Source;
    public Transform Target;
    public Rigidbody SourceRigidbody;

    PIDController mPidController = new PIDController();

    void Start()
    {
        SourceRigidbody = Source.GetComponent<Rigidbody>();
        mPidController.ResetHistory();
        mPidController.Kp = 2;
        mPidController.Ki = 0.1f;
        mPidController.Kd = 5;
        mPidController.KPlant = 1;
    }

    // Update is called once per frame
    void Update()
    {
        var currError = Target.position - Source.position;
        if (currError.magnitude < 0.01f)
        {
            return;
        }

        var error = 0;
        mPidController.AddSample(error);
        var output = mPidController.GetLastOutput();
    }
}
