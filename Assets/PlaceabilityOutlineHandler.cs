using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceabilityOutlineHandler : MonoBehaviour {

    //each cell has an overlay sprite
    public Sprite NotPlaceableOverlay;
    public Sprite OuterCornerOverlay;
    public Sprite InnerCornerOverlay;
    public Sprite StraightEdgeOverlay;
    public Sprite KittyCornerOverlay;

    private List<Cell> cells = new List<Cell>();

    public void AddCell(Cell toAdd)
    {
        cells.Add(toAdd);
        updateOverlayForCell(toAdd, toAdd.getGameCell().isExplored);
        toAdd.getGameCell().NotifyVisibilityChanged += OnVisibilityChanged;
    }

    private void OnVisibilityChanged(BulletGridGenerator.GameCell cell, bool newVisibility)
    {
        updateOverlayForCell(cell.Cell.GetComponent<Cell>(), newVisibility);
    }

    private void updateOverlayForCell(Cell cell, bool isExplored)
    {
        if (!isExplored)
        {
            // create a gameobj
            GameObject overlay = new GameObject();
            overlay.name = "NotPlaceableOverlay";
            overlay.AddComponent<SpriteRenderer>().sprite = NotPlaceableOverlay;
            overlay.GetComponent<SpriteRenderer>().sortingOrder = 1;
            overlay.transform.parent = cell.gameObject.transform;
            overlay.transform.position = cell.gameObject.transform.position;
            overlay.transform.localScale = cell.gameObject.transform.localScale * 1.29f; // TODO stupid brittle hacky magic number, FIXME
        }
        else
        {
            Transform child = cell.gameObject.transform.FindChild("NotPlaceableOverlay");
            if (child != null) Destroy(child.gameObject);
        }
    }
}
