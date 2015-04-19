using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public GameObject item {
        get {
            if (transform.childCount > 0) {
                return transform.GetChild(0).gameObject;
            } else {
                return null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if (!item) {
            eventData.selectedObject.transform.parent = transform;
        }
    }
}
