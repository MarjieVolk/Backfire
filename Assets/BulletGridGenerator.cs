using UnityEngine;
using System.Collections;
using System;

public class BulletGridGenerator : MonoBehaviour {

    public GameObject NormalCell;
    public GameObject BombCell;
    public GameObject NormalNanoBot;
    public int startTileX, startTileY;

    public GameCell[][] GameGrid;

	// Use this for initialization
	void Start () {
        // Set up bullet grid based on levelDescriptor's size
        Texture2D levelDescriptor = GetComponent<SpriteRenderer>().sprite.texture;
        int cellsWidth = levelDescriptor.width;
        int cellsHeight = levelDescriptor.height;
        GameGrid = new GameCell[cellsWidth][];
        for (int x = 0; x < cellsWidth; x++)
        {
            GameGrid[x] = new GameCell[cellsHeight];
        }

        // In-world grid size is based on this GameObject's size
        Vector2 center = gameObject.transform.position;
        float realWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        float realHeight = GetComponent<SpriteRenderer>().bounds.size.y;

        // Populate bullet grid
        Vector2 botLeft = center - new Vector2(realWidth / 2, realHeight / 2);
        float cellWidth = realWidth / cellsWidth;
        float cellHeight = realHeight / cellsHeight;

        for (int x = 0; x < cellsWidth; x++) {
            for (int y = 0; y < cellsHeight; y++) {
                Vector2 cellCenter = botLeft + new Vector2(cellWidth * (x + 0.5f), cellHeight * (y + 0.5f));

                //pick the cell type to place based on the color of the pixel at this location
                GameObject cellPrefab = getPrefabForColor(levelDescriptor.GetPixel(x, y));
                GameObject cell = (GameObject)Instantiate(cellPrefab, cellCenter, Quaternion.identity);
                cell.transform.localScale = new Vector2(0.9f * cellWidth / cell.GetComponent<SpriteRenderer>().bounds.size.x, 0.9f * cellHeight / cell.GetComponent<SpriteRenderer>().bounds.size.y);
                cell.GetComponent<Cell>().GridPosition = new GridPosition(x, y);
                GameGrid[x][y] = new GameCell();
                GameGrid[x][y].Cell = cell;
            }
        }

        GameGrid[startTileX][startTileY].isExplored = true;
	}

    private GameObject getPrefabForColor(Color color)
    {
        Color grey = new Color(127 / 255.0f, 127 / 255.0f, 127 / 255.0f);
        Color red = new Color(255 / 255.0f, 0 / 255.0f, 0 / 255.0f);
        if (color.Equals(grey)) // grey
        {
            return NormalCell;
        }
        if (color.Equals(red))
        {
            return BombCell;
        }
        Debug.Log(color);
        Debug.Log(grey);
        Debug.Log(red);
        throw new ArgumentException();
    }

    public GameCell getCellAt(GridPosition position)
    {
        return GameGrid[position.X][position.Y];
    }

    public GridPosition applyDelta(GridPosition origin, GridPosition delta)
    {
        GridPosition dest = new GridPosition(origin.X + delta.X, origin.Y + delta.Y);
        if (dest.X >= GameGrid.Length || dest.X < 0) return null;
        dest.Y = ((dest.Y % GameGrid[0].Length) + GameGrid[0].Length) % GameGrid[0].Length; // NOTE C#'s modulus operate incorrectly can generate negative numbers!
        
        return dest;
    }

    private void setBotPosition( GameObject bot, GridPositionComponent botPosition, GridPosition position) {
        if (position == null) {
            return;
        }
        GameGrid[position.X][position.Y].Nanobot = bot;
        if (bot != null) {
            GameGrid[position.X][position.Y].isExplored = true;
        }
        Vector3 screenPosition = GameGrid[position.X][position.Y].Cell.transform.position;
        bot.transform.position = new Vector3(screenPosition.x, screenPosition.y, bot.transform.position.z);
        botPosition.position = position;
    }

    public void moveBot(GridPosition source, Nanobot nanobot, GridPosition offset)
    {
        GridPosition newPosition = applyDelta(source, offset);
        placeBot(newPosition, nanobot);
        GameGrid[source.X][source.Y].Nanobot = null; // TODO race condition?
    }

    public void placeBot(GridPosition position, Nanobot nanobot)
    {
        if (nanobot.schematic == null)
        {
            return;
        }
        GameObject newBot = Instantiate(nanobot.gameObject);
        GridPositionComponent gridPosition = newBot.GetComponent<GridPositionComponent>();
        setBotPosition(newBot, gridPosition, position);
    }

    public class GameCell {
        public GameObject Cell;
        public GameObject Nanobot;
        public bool isExplored = false;
    }
}
