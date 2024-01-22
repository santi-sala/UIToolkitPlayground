using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class InfiniteScrollVertical : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewportTransform;
    public RectTransform contentPanelTransform;
    public VerticalLayoutGroup VLG;

    public RectTransform[] itemsList;

    Vector2 oldVelocity;
    bool isUpdated;

    private void Start()
    {
        isUpdated = false;
        oldVelocity = Vector2.zero;
        int itemsToAdd = Mathf.CeilToInt(viewportTransform.rect.height / (itemsList[0].rect.height + VLG.spacing));
       // int itemsToAdd = Mathf.CeilToInt(viewportTransform.rect.width / (itemsList[0].rect.width + HLG.spacing));

        Debug.Log("Vertical items to add is: " + itemsToAdd);
        Debug.Log("Vertical rect.height is: " + viewportTransform.rect.height);
        Debug.Log("Vertical other is " + itemsList[0].rect.height + VLG.spacing);

        for (int i = 0; i < itemsToAdd; i++)
        {
            RectTransform RT = Instantiate(itemsList[i % itemsList.Length], contentPanelTransform);
            RT.SetAsLastSibling();
            Debug.Log("in the 1st for loop");
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

            Debug.Log("in the 2nd for loop");

        }

       // Debug.Log("in the 3rd debug");

        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x, 0 + (itemsList[0].rect.height + VLG.spacing) * itemsToAdd, contentPanelTransform.localPosition.z);
    }

    private void Update()
    {
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = oldVelocity;
        }
        //Debug.Log("vertical " + itemsList.Length * (itemsList[0].rect.height + VLG.spacing));
        //Debug.Log("sup " + contentPanelTransform.localPosition.y);

        
        if (contentPanelTransform.localPosition.y > (itemsList.Length * (itemsList[0].rect.height + VLG.spacing)))
        {
            Debug.Log("first y");
            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3( 0, itemsList.Length * (itemsList[0].rect.height + VLG.spacing), 0);
            isUpdated = true;
        }


        if (contentPanelTransform.localPosition.y < 0)
        {
            Debug.Log("second y");

            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3( 0, itemsList.Length * (itemsList[0].rect.height + VLG.spacing), 0);
            isUpdated = true;
        }
        
        
    }
}
