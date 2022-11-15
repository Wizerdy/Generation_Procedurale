using UnityEngine;
using UnityEngine.UI;

public class LifeDisplayController : MonoBehaviour
{
    public Image heartPrefab;
    public int maxHeartDisplay;
    
    private void Start()
    {
        for (var i = 0; i < maxHeartDisplay; i++)
        {
            Instantiate(heartPrefab, transform);
        }

        var playerLife = FindObjectOfType<PlayerController>().GetComponent<Life>();
        ChangeHeartsCount(playerLife.startLife);
        playerLife.onDamageTaken.AddListener(ChangeHeartsCount);
    }

    private void ChangeHeartsCount(uint newCount)
    {
        var childrenCount = transform.childCount;
        if(newCount == childrenCount)
            return;

        for (var i = 0; i < maxHeartDisplay; i++)
        {
            transform.GetChild(i).gameObject.SetActive(newCount>i);
        }
    }
}
