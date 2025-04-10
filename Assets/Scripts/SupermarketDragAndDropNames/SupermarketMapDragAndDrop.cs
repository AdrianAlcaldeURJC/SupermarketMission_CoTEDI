using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupermarketMapDragAndDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToDrag;
    [SerializeField]
    private GameObject posWhereToDrag;

    private float dropDistance = 40;
    private bool isLocked;

    private Vector2 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = objectToDrag.transform.position;
    }

    public void DragObject()
    {
        if (!isLocked)
        {
            objectToDrag.transform.position = Input.mousePosition;
        }
    }

    public void DropObject()
    {
        float distance = Vector3.Distance(objectToDrag.transform.position, posWhereToDrag.transform.position);
        if (distance < dropDistance)
        {
            isLocked = true;
            objectToDrag.transform.position = posWhereToDrag.transform.position;
        }
        else
        {
            objectToDrag.transform.position = initialPos;
        }
    }
}
