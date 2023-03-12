using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectItemTrigger : MonoBehaviour
{
    [SerializeField] CollectItem collectItemPrefab;
    [SerializeField] int numberOfItems;
    [SerializeField] float minRadius = 3f;
    [SerializeField] float maxRadius = 7f;
    private bool isInteract;

    private void OnTriggerEnter(Collider collider)
    {
        SpawnCollectItem();
    }

    private void SpawnCollectItem()
    {
        if (GameManager.Instance.currentGameState != GameState.Running &&  GameManager.Instance.currentGameState != GameState.Frenzy)
            return;
        
        if(isInteract)
            return;
        isInteract = true;

        for (int i = 0; i < numberOfItems; i++)
        {
            var newCollectItem = Instantiate(collectItemPrefab, this.transform.position, Quaternion.Euler(new Vector3(0f, Random.Range(-306f, 360f), 0f)), this.transform);
            newCollectItem.transform.DOJump(PickRandomPosition(newCollectItem.transform.position), 5f, 1, 1f).OnComplete(() => newCollectItem.canInteract = true);
        }
    }

    private Vector3 PickRandomPosition(Vector3 currentPosition)
    {
        if (GameManager.Instance)
        {
            return GameManager.Instance.platformContainer.GetRandomPositionOnPlatform();
        }
        else // pick randomly around this trigger points
        {
            var randomPos = (Vector3)(Random.Range(minRadius, maxRadius) * Random.insideUnitCircle);
            return currentPosition + new Vector3(randomPos.x, 0f, randomPos.y);
        }
    }
}
