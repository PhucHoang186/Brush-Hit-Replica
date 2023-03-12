using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static Action<Transform> ON_LOCK_AT_TARGET;
    [SerializeField] Camera maincCam;
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] float frenzyModeCamFov;
    [SerializeField] Vector3 followOffset;
    private CinemachineTransposer transposer;
    float initCamFov;

    void Start()
    {
        transposer = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
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
        transposer.m_FollowOffset = followOffset;
    }

    public void OnChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Running:
                EnterNormalMode();
                break;
            case GameState.Frenzy:
                EnterFrenzyMode();
                break;
            default:
                break;
        }
    }

    private void EnterNormalMode()
    {
        StartCoroutine(CorEnterNormalMode());
    }

    private void EnterFrenzyMode()
    {
        StartCoroutine(CorEnterFrenzyMode());
    }

    private IEnumerator CorEnterFrenzyMode()
    {
        var t = 0f;
        while (t < 1f)
        {
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, frenzyModeCamFov, t);
            t += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CorEnterNormalMode()
    {
        var t = 0f;
        while (t < 1f)
        {
            virtualCam.m_Lens.FieldOfView = Mathf.Lerp(virtualCam.m_Lens.FieldOfView, initCamFov, t);
            t += Time.deltaTime;
            yield return null;
        }
    }

}
