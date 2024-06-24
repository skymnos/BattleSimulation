using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionRectangle : MonoBehaviour
{
    public Vector3 startingPoint;
    private Vector3 endingPoint;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startingPoint;
        endingPoint = new Vector3(Input.mousePosition.x - startingPoint.x,startingPoint.y - Input.mousePosition.y, 0);
        rectTransform.sizeDelta = endingPoint;
    }
}
