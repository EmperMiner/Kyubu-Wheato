using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 targetPosition;
    private GameObject ExitHoePos;
    private RectTransform pointerRectTransform;

    private void Awake() 
    {
        ExitHoePos = GameObject.FindGameObjectWithTag("ExitHoeContainer");
        targetPosition = new Vector3(-14.5f, 16.5f);
        pointerRectTransform = transform.Find("ArrowImage").GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        if (angle < 0) angle += 360f;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
