using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;

    [Button("SetUp Level")]
    public void SetUpLevel()
    {
        var weight = (int)transform.localScale.x;
        var height = (int)transform.localScale.y;
        var posX = (int)transform.position.x;
        var posZ = (int)transform.position.y;

        for (int k = 0; k < this.transform.childCount; k++)
        {
            DestroyImmediate(this.transform.GetChild(k).gameObject);
        }

        for (int i = -weight / 2 + 1; i < weight / 2; i++)
        {
            for (int j = -height / 2 + 1; j < height / 2; j++)
            {
                if (Physics.Raycast(new Vector3(i + posX, 0f, j + posZ), Vector3.down, 1f))
                {
                    var newObstacle = Instantiate(obstaclePrefab, new Vector3(i + posX, 0f, j + posZ), Quaternion.identity, this.transform);
                    newObstacle.transform.localScale = new Vector3(1f / weight, 1f, 1f / height);
                }
            }
        }
        Debug.LogError(this.transform.childCount);
        Debug.LogError(weight);
        Debug.LogError(height);
    }
}
