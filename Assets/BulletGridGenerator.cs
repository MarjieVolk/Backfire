using UnityEngine;
using System.Collections;
using System;

public class BulletGridGenerator : MonoBehaviour {

    public GameObject NormalCell;
    public GameObject BombCell;
    public GameObject NormalNanoBot;

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

        NanobotSchematic schematic = new NanobotSchematic();
        schematic.transformation[0][1] = schematic;
        placeBot(new GridPosition(0, 0), schematic);
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
        dest.Y %= GameGrid[0].Length;

        return dest;
    }

    private void setBotPosition( GameObject bot, GridPositionComponent botPosition, GridPosition position) {
        if (position == null) {
            return;
        }
        GameGrid[position.X][position.Y].Nanobot = bot;
        Vector3 screenPosition = GameGrid[position.X][position.Y].Cell.transform.position;
        bot.transform.position = new Vector3(screenPosition.x, screenPosition.y, bot.transform.position.z);
        botPosition.position = position;
    }

    public void moveMe(GameObject movee, int x, int y) {
        GridPositionComponent source = movee.GetComponent<GridPositionComponent>();
        GridPosition newPosition = applyDelta(source.position, new GridPosition(x, y));
        if (movee != GameGrid[source.position.X][source.position.Y].Nanobot) {
            throw new Exception(String.Format("NanoBot {4} requesting move to ({0}/{1}) does not match NanoBot {5} at ({2}/{3}).",
                newPosition.X, newPosition.Y, source.position.X, source.position.Y, movee, GameGrid[source.position.X][source.position.Y].Nanobot));
        }
        GameGrid[source.position.X][source.position.Y].Nanobot = null;
        setBotPosition(movee, source, newPosition);
    }

    public void moveBot(GridPosition source, NanobotSchematic schematic, GridPosition offset)
    {
        GridPosition newPosition = applyDelta(source, offset);
        placeBot(newPosition, schematic);
        GameGrid[source.X][source.Y] = null; // TODO race condition?
    }

    public void placeBot(GridPosition position, NanobotSchematic schematic)
    {
        if (schematic == null)
        {
            return;
        }
        GameObject newBot = Instantiate(NormalNanoBot);
        newBot.GetComponent<Move>().schematic = schematic;
        GridPositionComponent gridPosition = newBot.GetComponent<GridPositionComponent>();
        setBotPosition(newBot, gridPosition, position);
    }

    // Update is called once per frame
    void Update()
    {
	
	}

    public class GameCell {
        public GameObject Cell;
        public GameObject Nanobot;
    }
}
