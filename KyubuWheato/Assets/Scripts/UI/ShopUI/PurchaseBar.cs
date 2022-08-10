using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PurchaseBar : MonoBehaviour
{
    public GameObject shopItem;
    void Start()
    {

    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != shopItem)
        {
            this.gameObject.SetActive(false);
        } else
        {
            this.gameObject.SetActive(true);
        }
    }
}
