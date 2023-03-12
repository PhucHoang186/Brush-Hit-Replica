using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum PivotType
{
    FirstPivotPoint,
    SecondPivotPoint
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [SerializeField] Material collideMat;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform firstPivotPoint;
    [SerializeField] Transform secondPivotPoint;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] float rotateSpeed;
    [SerializeField] float initDistanceBetweenPoints = 2.5f;
    [SerializeField] float distanceBetweenPointsWhenFrenzy = 3.5f;
    [SerializeField] float accelarateSpeed;
    [SerializeField] float frenzyModeDuration;
    [SerializeField] float jumpPower;
    [SerializeField] Transform lookAtPos;
    private Transform parentPoint;
    private float distanceBetweenPoints;
    private Transform childPoint;
    private int currentDir = 1;
    private bool canMoving;
    private PivotType currentPivotType;
    private Vector3 initPos;

    private Vector3 boxColliderSize;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        Init();
        GameManager.ON_CHANGE_STATE += OnChangeState;
    }

    private void Init()
    {
        lineRenderer.positionCount = 2;
        parentPoint = firstPivotPoint;
        childPoint = secondPivotPoint;
        secondPivotPoint.transform.parent = firstPivotPoint;
        distanceBetweenPoints = initDistanceBetweenPoints;
        currentPivotType = PivotType.FirstPivotPoint;
    }

    void OnDestroy()
    {
        GameManager.ON_CHANGE_STATE -= OnChangeState;
    }

    void Update()
    {
        UpdateLineRenderer();
        UpdateMovement();

        if (GameManager.Instance.IsFrenzyMode)
        {
            distanceBetweenPoints = Mathf.Lerp(distanceBetweenPoints, distanceBetweenPointsWhenFrenzy, Time.deltaTime * accelarateSpeed);
        }
    }

    private void MergeTwoPointTogether(Action cb = null)
    {
        Debug.LogError("Merge");
        canMoving = false;
        Sequence s = DOTween.Sequence();
        var mergeTween = childPoint.DOLocalMoveX(0f, 1f);
        var jumPTween = transform.DOJump(transform.position + Vector3.down * 3f, jumpPower, 1, 2f).SetEase(Ease.InOutFlash); // make it jump below the platform

        s.Append(mergeTween).Append(jumPTween);
        s.Play().OnComplete(() => cb?.Invoke());
    }

    private void SplitTwoPointApart(Action cb = null)
    {
        transform.position = initPos + Vector3.down * 3f;
        ResetPlayer();

        Sequence s = DOTween.Sequence();
        var splitTween = childPoint.DOLocalMoveX(distanceBetweenPoints, 1f);
        var jumPTween = transform.DOJump(initPos, jumpPower, 1, 2f).SetEase(Ease.InOutFlash);

        s.Append(jumPTween).Append(splitTween);
        s.Play().OnComplete(() => cb?.Invoke());
    }

    private void ResetPlayer()
    {
        currentPivotType = PivotType.FirstPivotPoint;
        currentDir = currentPivotType == PivotType.FirstPivotPoint ? 1 : -1;
        parentPoint.localPosition = Vector3.zero;
        parentPoint.rotation = Quaternion.identity;
        childPoint.localPosition = Vector3.zero;
        childPoint.rotation = Quaternion.identity;

        parentPoint = firstPivotPoint;
        childPoint = secondPivotPoint;
        parentPoint.parent = this.transform;
        childPoint.parent = parentPoint;
        distanceBetweenPoints = initDistanceBetweenPoints;
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, firstPivotPoint.position);
        lineRenderer.SetPosition(1, secondPivotPoint.position);
    }

    public void OnChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.LoadLevel:
                GetLevelData();
                break;
            case GameState.Running:
                EnterNormalMode();
                break;
            case GameState.Frenzy:
                EnterFrenzyMode();
                break;
            case GameState.Lose:
                EndGame();
                break;
            case GameState.TransitionToNextLevel:
                MergeTwoPointTogether();
                break;
            default:
                break;
        }
    }

    private void GetLevelData()
    {
        StartCoroutine(CorGetLevelData());
    }

    private void EndGame()
    {
        MergeTwoPointTogether();
    }

    private IEnumerator CorGetLevelData()
    {
        while (GameManager.Instance.platformContainer == null)
            yield return null;

        initPos = GameManager.Instance.platformContainer.GetStartPoint().position;
        lookAtPos.position = initPos;
        CameraController.ON_LOCK_AT_TARGET?.Invoke(lookAtPos);
        SplitTwoPointApart(() =>
        {
            canMoving = true;
            CameraController.ON_LOCK_AT_TARGET(parentPoint);
        });
    }

    private void EnterFrenzyMode()
    {
        StartCoroutine(CorEnterFrenzyMode());
    }

    private IEnumerator CorEnterFrenzyMode()
    {
        boxCollider.center = new Vector3(distanceBetweenPointsWhenFrenzy / 2, boxCollider.center.y, boxCollider.center.z);
        boxCollider.size = new Vector3(distanceBetweenPointsWhenFrenzy, boxCollider.size.y, boxCollider.size.z);
        yield return new WaitForSeconds(frenzyModeDuration);
        GameManager.ON_CHANGE_STATE?.Invoke(GameState.Running);
        boxCollider.center = new Vector3(initDistanceBetweenPoints / 2, boxCollider.center.y, boxCollider.center.z);
        boxCollider.size = new Vector3(initDistanceBetweenPoints, boxCollider.size.y, boxCollider.size.z);
    }

    public Material GetCollideMat()
    {
        return collideMat;
    }

    private void UpdateMovement()
    {
        if (!canMoving)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchPivot();
        }
        MovingPlayer();
    }

    private void MovingPlayer()
    {
        var rotateTo = parentPoint.eulerAngles.y + currentDir * rotateSpeed * Time.deltaTime;
        parentPoint.eulerAngles = new Vector3(parentPoint.eulerAngles.x, rotateTo, parentPoint.eulerAngles.z);
        childPoint.localPosition = new Vector3(currentDir * distanceBetweenPoints, childPoint.transform.localPosition.y, childPoint.transform.localPosition.z);
    }

    private void CheckLoseGame()
    {
        if (!Physics.Raycast(parentPoint.position, Vector3.down))
        {
            GameManager.ON_CHANGE_STATE?.Invoke(GameState.Lose);
        }
    }

    private void SwitchPivot()
    {
        currentDir *= -1;
        if (currentPivotType == PivotType.FirstPivotPoint)
        {
            parentPoint = secondPivotPoint;
            childPoint = firstPivotPoint;
            currentPivotType = PivotType.SecondPivotPoint;
        }
        else
        {
            parentPoint = firstPivotPoint;
            childPoint = secondPivotPoint;
            currentPivotType = PivotType.FirstPivotPoint;
        }

        parentPoint.parent = this.transform;
        childPoint.parent = parentPoint;
        CameraController.ON_LOCK_AT_TARGET(parentPoint);
        CheckLoseGame();
    }

    private void EnterNormalMode()
    {
        StartCoroutine(CorEnterNormalMode());
    }

    private IEnumerator CorEnterNormalMode()
    {
        var t = 0f;
        while (t < 1f)
        {
            distanceBetweenPoints = Mathf.Lerp(distanceBetweenPoints, initDistanceBetweenPoints, t);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
