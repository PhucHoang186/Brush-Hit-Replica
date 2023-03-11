using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] float distanceBetweenPoints;
    [SerializeField] float distanceBetweenPointsWhenFrenzy;
    [SerializeField] float accelarateSpeed;
    [SerializeField] float frenzyModeDuration;
    private Transform parentPoint;
    private Transform childPoint;
    private int currentDir = 1;
    private bool isChanging;
    private PivotType currentPivotType;

    private Vector3 boxColliderSize;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        lineRenderer.positionCount = 2;
        parentPoint = firstPivotPoint;
        childPoint = secondPivotPoint;
        secondPivotPoint.transform.parent = firstPivotPoint;
        currentPivotType = PivotType.FirstPivotPoint;
        GameManager.ON_CHANGE_STATE += OnChangeState;
    }

    void OnDestroy()
    {
        GameManager.ON_CHANGE_STATE -= OnChangeState;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, firstPivotPoint.position);
        lineRenderer.SetPosition(1, secondPivotPoint.position);
        UpdateMovement();

        if (GameManager.Instance.IsFrenzyMode)
        {
            distanceBetweenPoints = Mathf.Lerp(distanceBetweenPoints, distanceBetweenPointsWhenFrenzy, Time.deltaTime * accelarateSpeed);
        }
    }

    public void OnChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Frenzy:
                EnterFrenzyMode();
                break;
            default:
                break;
        }
    }

    private void EnterFrenzyMode()
    {
        StartCoroutine(CorEnterFrenzyMode());
    }

    private IEnumerator CorEnterFrenzyMode()
    {
        boxCollider.size = new Vector3(distanceBetweenPointsWhenFrenzy, boxCollider.size.y, boxCollider.size.z);
        yield return new WaitForSeconds(frenzyModeDuration);
        GameManager.ON_CHANGE_STATE?.Invoke(GameState.Running);
        boxCollider.size = new Vector3(distanceBetweenPoints, boxCollider.size.y, boxCollider.size.z);
    }

    public Material GetCollideMat()
    {
        return collideMat;
    }

    private void UpdateMovement()
    {
        if (Input.GetMouseButtonDown(0))
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

    private void SwitchPivot()
    {
        if (currentPivotType == PivotType.FirstPivotPoint)
        {
            parentPoint = secondPivotPoint;
            childPoint = firstPivotPoint;
            currentPivotType = PivotType.SecondPivotPoint;
            secondPivotPoint.parent = this.transform;
            firstPivotPoint.parent = parentPoint;
        }
        else
        {
            parentPoint = firstPivotPoint;
            childPoint = secondPivotPoint;
            currentPivotType = PivotType.FirstPivotPoint;
            firstPivotPoint.parent = this.transform;
            secondPivotPoint.parent = parentPoint;
        }
        currentDir *= -1;
        CameraController.ON_LOCK_AT_TARGET(parentPoint);
    }
}
