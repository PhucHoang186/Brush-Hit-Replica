using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    bool isChangeColor;

    private void OnTriggerEnter(Collider other)
    {
        if(isChangeColor)
            return;
        isChangeColor = true;
        Debug.LogError("Trigger");
        meshRenderer.material = PlayerController.Instance.GetCollideMat();
    }
}
