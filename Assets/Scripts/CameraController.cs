using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    public static Action<Transform> ON_LOCK_AT_TARGET;
    [SerializeField] Camera maincCam;
    [SerializeField] CinemachineVirtualCamera virtualCam;

    void Start()
    {
        ON_LOCK_AT_TARGET += SetLockAtTarget;
    }
     
    void OnDestroy()
    {
        ON_LOCK_AT_TARGET -= SetLockAtTarget;
    }

    public void SetLockAtTarget(Transform target)
    {
        // virtualCam.LookAt = target;
        virtualCam.Follow = target;
    }
}
