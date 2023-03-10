using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [SerializeField] float rotateSpeed;
    [SerializeField] float distanceBetweenPoints;
    private Transform parentPoint;
    private Transform childPoint;
    private int currentDir = 1;
    private bool isChanging;
    private PivotType currentPivotType;

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
        currentPivotType = PivotType.FirstPivotPoint;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, firstPivotPoint.position);
        lineRenderer.SetPosition(1, secondPivotPoint.position);
        UpdateMovement();
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
