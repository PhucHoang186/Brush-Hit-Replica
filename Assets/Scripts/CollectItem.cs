using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectItem : MonoBehaviour
{
    [SerializeField] GameObject hitParticle;
    [SerializeField] float moveDuration = 2f;
    private void OnTriggerEnter(Collider collider)
    {
        UIManager.Instance.collectItemUI.CollectItem();
        hitParticle.SetActive(true);
        transform.DOMoveY(50f, moveDuration).OnComplete(() => this.gameObject.SetActive(false));
    }
}
