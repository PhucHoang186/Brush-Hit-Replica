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
    [SerializeField] float frenzyModeCamFov;
    float initCamFov;

    void Start()
    {
        ON_LOCK_AT_TARGET += SetLockAtTarget;
        GameManager.ON_CHANGE_STATE += OnChangeState;
        initCamFov = virtualCam.m_Lens.FieldOfView;
    }

    void OnDestroy()
    {
        ON_LOCK_AT_TARGET -= SetLockAtTarget;
        GameManager.ON_CHANGE_STATE -= OnChangeState;
    }

    public void SetLockAtTarget(Transform target)
    {
        virtualCam.Follow = target;
    }

    public void OnChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Running:
                virtualCam.m_Lens.FieldOfView = initCamFov;
                Debug.LogError("Normal Mode");
                break;
            case GameState.Frenzy:
                virtualCam.m_Lens.FieldOfView = frenzyModeCamFov;
                Debug.LogError("Frenzy Mode");
                break;
            default:
                break;
        }
    }

}
