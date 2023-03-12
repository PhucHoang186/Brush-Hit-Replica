using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformContainer : MonoBehaviour
{
    [SerializeField] List<Transform> platforms;
    [SerializeField] Transform startPoint;

    void Awake()
    {
        GameManager.Instance.platformContainer = this;
    }

    public Transform GetStartPoint()
    {
        return startPoint;
    }

    public Vector3 GetRandomPositionOnPlatform()
    {
        var randomPos = platforms[Random.Range(0, platforms.Count)].transform.position + Vector3.one * Random.Range(0, 10); // each platform width and heigh is 10
        randomPos.y = 1.7f; // the heigth above the grass
        return randomPos;
    }

    public int GetTotalPoints()
    {
        return platforms.Count * 10;
    }
}
