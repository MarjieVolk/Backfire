using UnityEngine;
using System.Collections;
using System;

public class BulletGridGenerator : MonoBehaviour {

    public GameObject NormalCell;
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

        for (int x = 0; x < cellsWidth; x++)
        {
            for (int y = 0; y < cellsHeight; y++)
            {
                Vector2 cellCenter = botLeft + new Vector2(cellWidth * (x + 0.5f), cellHeight * (y + 0.5f));

                //pick the cell type to place based on the color of the pixel at this location
                GameObject cellPrefab = getPrefabForColor(levelDescriptor.GetPixel(x, y));
                GameObject cell = (GameObject)Instantiate(cellPrefab, cellCenter, Quaternion.identity);
                cell.GetComponent<Cell>().setPosition(x, y);
                GameGrid[x][y] = new GameCell();
                GameGrid[x][y].Cell = cell;
            }
        }
        GameObject newBotPrefab = (GameObject)Instantiate(NormalNanoBot, new Vector2(cellWidth * 0.5f, cellHeight * 0.5f), Quaternion.identity);
        GridPosition position = newBotPrefab.AddComponent<GridPosition>();
        position.x = 0;
        position.y = 0;
        GameGrid[0][0].Nanobot = newBotPrefab;
	}

    private GameObject getPrefabForColor(Color color)
    {
        Color grey = new Color(127/255.0f, 127/255.0f, 127/255.0f);
        if (color.Equals(grey)) // grey
        {
            return NormalCell;
        }
        Debug.Log(color);
        Debug.Log(grey);
        throw new ArgumentException();
    }

    public void moveMe(GameObject movee, int x, int y) {
        GridPosition source = movee.GetComponent<GridPosition>();
        //Debug.Log(String.Format("Moving a nanobot from {0}/{1} to {2}/{3}.", source.x, source.y, source.x + x, source.y + y));
        if (movee != GameGrid[source.x][source.y].Nanobot) {
            throw new Exception(String.Format("NanoBot {4} requesting move to ({0}/{1}) does not match NanoBot {5} at ({2}/{3}).",
                source.x + x, source.y + y, source.x, source.y, movee, GameGrid[source.x][source.y].Nanobot));
        }
        GameGrid[source.x + x][source.y + y].Nanobot = movee;
        GameGrid[source.x][source.y].Nanobot = null;
        movee.transform.position = GameGrid[source.x + x][source.y + y].Cell.transform.position;
        source.x += x;
        source.y += y;
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
