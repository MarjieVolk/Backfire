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

    private BulletGridGenerator grid;
    private GameObject[][] corners;
    private GameObject[][] verticalEdges;
    private GameObject[][] horizontalEdges;

    private Vector2 xOffset;
    private Vector2 yOffset;

    public void AddCell(Cell toAdd)
    {
        if (grid == null)
        {
            grid = toAdd.getGameCell().Grid;
            xOffset = grid.gridPositionToWorldPosition(new GridPosition(0, 0)) - grid.gridPositionToWorldPosition(new GridPosition(1, 0));
            yOffset = grid.gridPositionToWorldPosition(new GridPosition(0, 0)) - grid.gridPositionToWorldPosition(new GridPosition(0, 1));

            int gridWidth = toAdd.getGameCell().Grid.GameGrid.Length;
            int gridHeight = toAdd.getGameCell().Grid.GameGrid[0].Length;
            corners = new GameObject[gridWidth + 1][];
            verticalEdges = new GameObject[gridWidth + 1][];
            horizontalEdges = new GameObject[gridWidth + 1][];
            for (int x = 0; x < corners.Length; x++)
            {
                corners[x] = new GameObject[gridHeight + 1];
                verticalEdges[x] = new GameObject[gridHeight + 1];
                horizontalEdges[x] = new GameObject[gridHeight + 1];
                for (int y = 0; y < corners[x].Length; y++)
                {
                    verticalEdges[x][y] = new GameObject();
                    verticalEdges[x][y].AddComponent<SpriteRenderer>();
                    verticalEdges[x][y].transform.position = placeVerticalEdge(x, y);
                    verticalEdges[x][y].transform.localScale = (toAdd.transform.localScale / BulletGridGenerator.CELL_SCALE) / 2;
                    verticalEdges[x][y].GetComponent<SpriteRenderer>().sortingOrder = 2;

                    horizontalEdges[x][y] = new GameObject();
                    horizontalEdges[x][y].AddComponent<SpriteRenderer>();
                    horizontalEdges[x][y].transform.position = placeHorizontalEdge(x, y);
                    horizontalEdges[x][y].transform.localScale = (toAdd.transform.localScale / BulletGridGenerator.CELL_SCALE) / 2;
                    horizontalEdges[x][y].GetComponent<SpriteRenderer>().sortingOrder = 2;

                    corners[x][y] = new GameObject();
                    corners[x][y].AddComponent<SpriteRenderer>();
                    corners[x][y].transform.position = placeCorner(x, y);
                    corners[x][y].transform.localScale = (toAdd.transform.localScale / BulletGridGenerator.CELL_SCALE) / 2;
                    corners[x][y].GetComponent<SpriteRenderer>().sortingOrder = 2;
                }
            }
        }
        OnVisibilityChanged(toAdd.getGameCell(), toAdd.getGameCell().isExplored);
        toAdd.getGameCell().NotifyVisibilityChanged += OnVisibilityChanged;
    }

    private Vector2 placeVerticalEdge(int x, int y)
    {
        return grid.gridPositionToWorldPosition(new GridPosition(x - 1, y)) - xOffset / 2.0f;
    }

    private Vector2 placeHorizontalEdge(int x, int y)
    {
        return grid.gridPositionToWorldPosition(new GridPosition(x, y - 1)) - yOffset / 2.0f;
    }

    private Vector2 placeCorner(int x, int y)
    {
        return grid.gridPositionToWorldPosition(new GridPosition(x - 1, y - 1)) - xOffset / 2.0f - yOffset / 2.0f;
    }

    private void OnVisibilityChanged(BulletGridGenerator.GameCell cell, bool newVisibility)
    {
        updateOverlayForCell(cell.Cell.GetComponent<Cell>(), newVisibility);

        GridPosition cellPosition = cell.Cell.GetComponent<Cell>().GridPosition;
        int x = cellPosition.X;
        int y = cellPosition.Y;

        updateCornerSprite(x, y);
        updateCornerSprite(x + 1, y);
        updateCornerSprite(x, y + 1);
        updateCornerSprite(x + 1, y + 1);

        updateHorizontalEdgeSprite(x, y);
        updateHorizontalEdgeSprite(x, y + 1);

        updateVerticalEdgeSprite(x, y);
        updateVerticalEdgeSprite(x + 1, y);
    }

    private bool getCellIsVisible(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GameGrid.Length && y < grid.GameGrid[0].Length)
        {
            return grid.getCellAt(new GridPosition(x, y)).isExplored;
        }

        return false;
    }

    private void updateCornerSprite(int x, int y)
    {
        GameObject corner = corners[x][y];

        bool botLeftVisible = getCellIsVisible(x - 1, y - 1);
        bool topLeftVisible = getCellIsVisible(x - 1, y);
        bool botRightVisible = getCellIsVisible(x, y - 1);
        bool topRightVisible = getCellIsVisible(x, y);

        int numVisible = 0;
        if (botLeftVisible) numVisible++;
        if (botRightVisible) numVisible++;
        if (topLeftVisible) numVisible++;
        if (topRightVisible) numVisible++;

        if (numVisible == 0 || numVisible == 4)
        {
            corner.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        if (numVisible == 1)
        {
            corner.GetComponent<SpriteRenderer>().sprite = InnerCornerOverlay;
            if (botRightVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
            if (topRightVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            }
            if (topLeftVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }
            if (botLeftVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
            }
        }

        if (numVisible == 3)
        {
            corner.GetComponent<SpriteRenderer>().sprite = OuterCornerOverlay;
            if (!botRightVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
            if (!topRightVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            }
            if (!topLeftVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }
            if (!botLeftVisible)
            {
                corner.transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
            }
        }

        if (numVisible == 2)
        {
            //straight edges
            if (botLeftVisible && botRightVisible)
            {
                corner.GetComponent<SpriteRenderer>().sprite = StraightEdgeOverlay;
                corner.transform.localRotation = Quaternion.AngleAxis(270.0f, Vector3.forward);
            }
            if (topLeftVisible && topRightVisible)
            {
                corner.GetComponent<SpriteRenderer>().sprite = StraightEdgeOverlay;
                corner.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
            }
            if (botLeftVisible && topLeftVisible)
            {
                corner.GetComponent<SpriteRenderer>().sprite = StraightEdgeOverlay;
                corner.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
            }
            if (botRightVisible && topRightVisible)
            {
                corner.GetComponent<SpriteRenderer>().sprite = StraightEdgeOverlay;
                corner.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
            }

            //kitty corners
            if (botLeftVisible && topRightVisible)
            {
                corner.GetComponent<SpriteRenderer>().sprite = KittyCornerOverlay;
                corner.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
            }
            if (topLeftVisible && botRightVisible)
            {
                corner.GetComponent<SpriteRenderer>().sprite = KittyCornerOverlay;
                corner.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
            }
        }
    }

    private void updateVerticalEdgeSprite(int x, int y)
    {
        GameObject edge = verticalEdges[x][y];

        bool leftVisible = getCellIsVisible(x - 1, y);
        bool rightVisible = getCellIsVisible(x, y);

        if (leftVisible == rightVisible)
        {
            edge.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        edge.GetComponent<SpriteRenderer>().sprite = StraightEdgeOverlay;

        if (leftVisible)
        {
            edge.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
        }
        else
        {
            edge.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.forward);
        }
    }

    private void updateHorizontalEdgeSprite(int x, int y)
    {
        GameObject edge = horizontalEdges[x][y];

        bool bottomVisible = getCellIsVisible(x, y - 1);
        bool topVisible = getCellIsVisible(x, y);

        if (bottomVisible == topVisible)
        {
            edge.GetComponent<SpriteRenderer>().sprite = null;
            return;
        }

        edge.GetComponent<SpriteRenderer>().sprite = StraightEdgeOverlay;

        if (bottomVisible)
        {
            edge.transform.localRotation = Quaternion.AngleAxis(270.0f, Vector3.forward);
        }
        else
        {
            edge.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.forward);
        }
    }

    private void updateOverlayForCell(Cell cell, bool isExplored)
    {
        Transform child = cell.gameObject.transform.FindChild("NotPlaceableOverlay");
        if (!isExplored)
        {
            if (child != null) return;
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
            if (child != null) Destroy(child.gameObject);
        }
    }
}
