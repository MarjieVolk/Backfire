using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    private static GameObject itemBeingDragged;

    private Vector3 startPosition;
    private Transform startParent;

    public void OnBeginDrag(PointerEventData eventData) {

        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData) {
        itemBeingDragged = null;
        transform.position = startPosition;
    }
}
