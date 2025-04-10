using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField]
    private AudioSource AudioSource;
    [SerializeField]
    private AudioClip pickUpClip, dropClip;

    private bool dragging;

    private Vector3 offset;

    void Update()
    {
        if (!dragging)
            return;

        var mousePos = this.GetMousePos();
        this.gameObject.transform.SetPositionAndRotation( mousePos - offset, this.transform.rotation); // new Vector3(mousePos.x - offset.x, mousePos.y - offset.y, transform.position.z);
        
    }

    private void OnMouseDown()
    {

        dragging = true;
        //AudioSource.PlayOneShot(pickUpClip);

        offset = this.GetMousePos() - transform.position;
        Debug.Log("HELLO" + offset);

    }

    Vector3 GetMousePos()
    {
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)));
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
    }
}
