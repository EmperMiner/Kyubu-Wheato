using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PurchaseBar : MonoBehaviour
{
    [SerializeField] private GameObject currentShopItem;
    [SerializeField] private GameObject currentBuyButton;
    [SerializeField] private GameObject currentRefundButton;

    private void Update()
    {
        GameObject pressed = EventSystem.current.currentSelectedGameObject;
        if (pressed != currentShopItem && pressed != currentBuyButton && pressed != currentRefundButton)
        {
            this.gameObject.SetActive(false);
        } else
        {
            this.gameObject.SetActive(true);
        }
    }
}
