using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectItemTrigger : MonoBehaviour
{
    [SerializeField] GameObject collectItemPrefab;
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
        if (isInteract)
            return;
        isInteract = true;

        for (int i = 0; i < numberOfItems; i++)
        {
            var newCollectItem = Instantiate(collectItemPrefab, this.transform.position, Quaternion.Euler(new Vector3(0f, Random.Range(-306f, 360f), 0f)), this.transform);
            newCollectItem.transform.DOJump(PickRandomPosition(newCollectItem.transform.position), 10f, 1, 2f);
        }
    }

    private Vector3 PickRandomPosition(Vector3 currentPosition)
    {
        var randomPos = (Vector3)(Random.Range(minRadius, maxRadius) * Random.insideUnitCircle);

        return currentPosition + new Vector3(randomPos.x, 0f, randomPos.y);
    }
}
