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
                    verticalEdges[x][y].transform.localScale /= 2;
                    verticalEdges[x][y].GetComponent<SpriteRenderer>().sortingOrder = 2;

                    horizontalEdges[x][y] = new GameObject();
                    horizontalEdges[x][y].AddComponent<SpriteRenderer>();
                    horizontalEdges[x][y].transform.position = placeHorizontalEdge(x, y);
                    horizontalEdges[x][y].transform.localScale /= 2;
                    horizontalEdges[x][y].GetComponent<SpriteRenderer>().sortingOrder = 2;
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

    private void updateCornerSprite(int x, int y)
    {

    }

    private void updateVerticalEdgeSprite(int x, int y)
    {
        GameObject edge = verticalEdges[x][y];

        bool leftVisible = false;
        if (x != 0)
        {
            leftVisible = grid.getCellAt(new GridPosition(x - 1, y)).isExplored;
        }

        bool rightVisible = false;
        if (x != grid.GameGrid.Length)
        {
            rightVisible = grid.getCellAt(new GridPosition(x, y)).isExplored;
        }

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

        bool bottomVisible = false;
        if (y != 0)
        {
            bottomVisible = grid.getCellAt(new GridPosition(x, y - 1)).isExplored;
        }

        bool topVisible = false;
        if (y != grid.GameGrid[0].Length)
        {
            topVisible = grid.getCellAt(new GridPosition(x, y)).isExplored;
        }

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
