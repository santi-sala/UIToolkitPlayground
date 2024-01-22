using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class InfiniteScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewportTransform;
    public RectTransform contentPanelTransform;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] itemsList;

    Vector2 oldVelocity;
    bool isUpdated;

    private void Start()
    {
        isUpdated = false;
        oldVelocity = Vector2.zero;
        int itemsToAdd = Mathf.CeilToInt( viewportTransform.rect.width / (itemsList[0].rect.width + HLG.spacing));
       

        for (int i = 0; i < itemsToAdd; i++)
        {
            RectTransform RT = Instantiate(itemsList[i % itemsList.Length], contentPanelTransform);
            RT.SetAsLastSibling();
        }

        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = itemsList.Length - i - 1;
            while (num < 0)
            {
                num += itemsList.Length;
            }
            RectTransform RT = Instantiate(itemsList[num], contentPanelTransform);
            RT.SetAsFirstSibling();
        }

        contentPanelTransform.localPosition = new Vector3(0 - (itemsList[0].rect.width + HLG.spacing) * itemsToAdd, contentPanelTransform.localPosition.y, contentPanelTransform.localPosition.z);
    }

    private void Update()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = oldVelocity;
        }
        if (contentPanelTransform.localPosition.x > 0)
        {
            Debug.Log("first x");

            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(itemsList.Length * (itemsList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }

        Debug.Log("horizontal: " + itemsList.Length * (itemsList[0].rect.width + HLG.spacing));
        if (contentPanelTransform.localPosition.x < 0 - (itemsList.Length * (itemsList[0].rect.width + HLG.spacing)))
        {
            Debug.Log("second x");

            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(itemsList.Length * (itemsList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
    }
}
