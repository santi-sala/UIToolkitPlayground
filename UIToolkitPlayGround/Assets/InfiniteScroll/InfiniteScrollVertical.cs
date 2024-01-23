using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfiniteScrollVertical : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewportTransform;
    [SerializeField] private RectTransform contentPanelTransform;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;             

    public RectTransform[] itemsList;           // List of item prefabs to be used in the infinite scroll

    private Vector2 oldVelocity;    // Store the old velocity of the ScrollRect
    private bool isUpdated;         // Flag to check if the ScrollRect has been updated

    private void Start()
    {
        isUpdated = false;
        oldVelocity = Vector2.zero;

        // Calculate the number of items needed to fill the viewport initially
        int itemsToAdd = Mathf.CeilToInt(viewportTransform.rect.height / (itemsList[0].rect.height + verticalLayoutGroup.spacing));

        // Instantiate items to fill the content panel
        for (int i = 0; i < itemsToAdd; i++)
        {
            RectTransform extraButton = Instantiate(itemsList[i % itemsList.Length], contentPanelTransform);
            extraButton.SetAsLastSibling(); // Set the new item as the last child
            Debug.Log("in the 1st for loop");
        }

        // Instantiate additional items to create an initial duplicate set for looping
        for (int i = 0; i < itemsToAdd; i++)
        {
            int num = itemsList.Length - i - 1;
            while (num < 0)
            {
                num += itemsList.Length;
            }
            RectTransform extraButton = Instantiate(itemsList[num], contentPanelTransform);
            extraButton.SetAsFirstSibling(); // Set the new item as the first child
            Debug.Log("in the 2nd for loop");
        }

        // Adjust the content panel's initial local position to create a seamless loop
        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x, 0 + (itemsList[0].rect.height + verticalLayoutGroup.spacing) * itemsToAdd, contentPanelTransform.localPosition.z);
    }

    private void Update()
    {
        // If the ScrollRect has been updated, restore the old velocity to create a smooth scrolling effect
        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = oldVelocity;
        }

        // Check if the content panel has scrolled beyond the upper limit
        if (contentPanelTransform.localPosition.y > (itemsList.Length * (itemsList[0].rect.height + verticalLayoutGroup.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(0, itemsList.Length * (itemsList[0].rect.height + verticalLayoutGroup.spacing), 0);
            isUpdated = true; // Set the flag to true for future updates
        }

        // Check if the content panel has scrolled beyond the lower limit
        if (contentPanelTransform.localPosition.y < 0)
        {
            Canvas.ForceUpdateCanvases();
            oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(0, itemsList.Length * (itemsList[0].rect.height + verticalLayoutGroup.spacing), 0);
            isUpdated = true; // Set the flag to true for future updates
        }
    }
}
