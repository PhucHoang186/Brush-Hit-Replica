using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectItemUIContainer : MonoBehaviour
{
    [SerializeField] List<Image> collectItemUI;
    int currentItemCollected;

    void Start()
    {
        ResetCollectItems();
    }

    public void CollectItem()
    {
        currentItemCollected++;
        currentItemCollected = Mathf.Clamp(currentItemCollected, 0, collectItemUI.Count);
        collectItemUI[currentItemCollected - 1].gameObject.SetActive(true);
        if(currentItemCollected == collectItemUI.Count)
        {
        GameManager.ON_CHANGE_STATE?.Invoke(GameState.Frenzy);            
        }
    }

    public void ResetCollectItems()
    {
        foreach (var item in collectItemUI)
        {
            item.gameObject.SetActive(false);
        }
    }

}
