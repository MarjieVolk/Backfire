using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletGridGenerator : MonoBehaviour, TimestepManager.TimestepListener {

    public const float CELL_SCALE = 0.9f;

    public GameObject NormalCell;
    public GameObject BombCell;
    public GameObject ResourceRichCell;
    public GameObject PlaceOnlyCell;
    public GameObject PitOnlyCell;
    public GameObject JumpOnlyCell;
    public GameObject HiddenBombCell;

    public int startTileX, startTileY;
    public AudioClip crashSound, fallSound, placeBotSound, tileHoverSound;

    public GameCell[][] GameGrid;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<AudioSource>();
        GameObject.FindObjectOfType<TimestepManager>().addFinalizer(this);
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
                cell.transform.localScale = new Vector2(CELL_SCALE * cellWidth / cell.GetComponent<SpriteRenderer>().bounds.size.x, CELL_SCALE * cellHeight / cell.GetComponent<SpriteRenderer>().bounds.size.y);
                cell.GetComponent<Cell>().GridPosition = new GridPosition(x, y);
                cell.GetComponent<Cell>().placeBotSound = placeBotSound;
                cell.GetComponent<CellHighlighter>().tileHoverSound = tileHoverSound;
                GameGrid[x][y] = new GameCell();
                GameGrid[x][y].Cell = cell;
                GameGrid[x][y].Grid = this;
                GameGrid[x][y].movingHere = new List<GameObject>();
                GameGrid[x][y].goingAway = new List<GameObject>();
            }
        }

        GameGrid[startTileX][startTileY].isExplored = true;
	}

    private GameObject getPrefabForColor(Color color)
    {
        Color grey = new Color(127 / 255.0f, 127 / 255.0f, 127 / 255.0f); // 'normal'
        Color green = new Color(0 / 255.0f, 255 / 255.0f, 0 / 255.0f);    // resource rich
        Color red = new Color(255 / 255.0f, 0 / 255.0f, 0 / 255.0f);      // visible bomb
        Color yellow = new Color(255 / 255.0f, 255 / 255.0f, 0 / 255.0f); // place only
        Color black = new Color(0 / 255.0f, 0 / 255.0f, 0 / 255.0f);      // pit only
        Color purple = new Color(255 / 255.0f, 0 / 255.0f, 255 / 255.0f); // jump only
        Color blue = new Color(0 / 255.0f, 255 / 255.0f, 0 / 255.0f);     // hidden bomb

        if (color.Equals(grey)) return NormalCell;
        if (color.Equals(red)) return BombCell;
        if (color.Equals(green)) return ResourceRichCell;
        if (color.Equals(yellow)) return PlaceOnlyCell;
        if (color.Equals(black)) return PitOnlyCell;
        if (color.Equals(purple)) return JumpOnlyCell;
        if (color.Equals(blue)) return HiddenBombCell;

        throw new ArgumentException("Unrecognized color " + color);
    }

    public GameCell getCellAt(GridPosition position)
    {
        return GameGrid[position.X][position.Y];
    }

    public Vector2 gridPositionToWorldPosition(GridPosition position)
    {
        Vector2 xOffset = GameGrid[1][0].Cell.transform.position - GameGrid[0][0].Cell.transform.position;
        Vector2 yOffset = GameGrid[0][1].Cell.transform.position - GameGrid[0][0].Cell.transform.position;

        return (Vector2)GameGrid[0][0].Cell.transform.position + xOffset * position.X + yOffset * position.Y;
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
        GameGrid[position.X][position.Y].movingHere.Add(bot);
        Vector3 screenPosition = GameGrid[position.X][position.Y].Cell.transform.position;
        bot.transform.position = new Vector3(screenPosition.x, screenPosition.y, bot.transform.position.z);
        botPosition.position = position;
    }

    public void moveBotAnimated(GridPosition source, Nanobot nanobot, GridPosition offset, int durationInTicks, bool grow, bool die)
    {
        StartCoroutine(moveBotAnimatedCoroutine(source, nanobot, offset, durationInTicks, grow, die));
    }

    public IEnumerator moveBotAnimatedCoroutine(GridPosition source, Nanobot nanobot, GridPosition offset, int durationInTicks, bool grow, bool die)
    {
        bool destroyBot = false;
        GameObject bot;
        if (die) {
            bot = nanobot.gameObject;
            destroyBot = true;
        } else {
            bot = moveBot(source, nanobot, offset);
            if (bot == null) {
                destroyBot = true;
            }
        }

        // make a fake bot to animate, to handle things getting destroyed
        GameObject fakeBot = new GameObject();
        fakeBot.AddComponent<SpriteRenderer>().sprite = nanobot.GetComponent<SpriteRenderer>().sprite;
        fakeBot.transform.localScale = nanobot.transform.localScale;
        if(bot != null) bot.GetComponent<SpriteRenderer>().enabled = false;

        Vector2 initialPosition = gridPositionToWorldPosition(source);
        Vector2 finalPosition = gridPositionToWorldPosition(source + offset);
        Vector2 movement = finalPosition - initialPosition;
        for (int i = 0; i < durationInTicks; i++)
        {
            fakeBot.transform.position = initialPosition + (((float)i + 1) / durationInTicks) * movement;
            yield return null;
        }
        fakeBot.transform.position = finalPosition;

        // make the bot shrink to nothing all the time (if it's going to get destroyed) as a first approx
        if (destroyBot || bot == null) {
            if (SoundManager.instance != null) {
                if (!destroyBot)
                {
                    SoundManager.instance.PlaySingle(GetComponent<AudioSource>(), crashSound);
                }
                else
                {
                    SoundManager.instance.RandomizeSfx(GetComponent<AudioSource>(), fallSound);
                }
            }

            yield return null;
            Vector3 initialScale = fakeBot.transform.localScale;
            for (int i = 0; i < durationInTicks * 2; i++)
            {
                fakeBot.transform.localScale = initialScale * ((float)(durationInTicks * 2 - i) / (durationInTicks * 2));
                yield return null;
            }
        }

        Destroy(fakeBot);
        if(bot != null) bot.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void DestroyNanobotAt(GridPosition source) {
        if( GameGrid[source.X][source.Y].Nanobot != null) {
            GameGrid[source.X][source.Y].goingAway.Add(GameGrid[source.X][source.Y].Nanobot);
            GameGrid[source.X][source.Y].Nanobot = null;
        }
    }

    public GameObject moveBot(GridPosition source, Nanobot nanobot, GridPosition offset)
    {
        GridPosition newPosition = applyDelta(source, offset);
        GameObject newBot = placeBot(newPosition, nanobot);
        DestroyNanobotAt(source);
        return newBot;
    }

    public GameObject placeBot(GridPosition position, Nanobot nanobot)
    {
        if (nanobot.schematic == null || position == null) {
            return null;
        }
        GameObject newBot = Instantiate(nanobot.gameObject);
        GridPositionComponent gridPosition = newBot.GetComponent<GridPositionComponent>();
        setBotPosition(newBot, gridPosition, position);

        return newBot;
    }

    public void notifyTimestep() {
        foreach (GameCell[] row in GameGrid) {
            foreach (GameCell cell in row) {
                while( cell.goingAway.Count != 0 ) {
                    FindObjectOfType<TimestepManager>().removeListener(cell.goingAway[0].GetComponent<Nanobot>());
                    Destroy(cell.goingAway[0]);
                    cell.goingAway.RemoveAt(0);
                }
                if (cell.movingHere.Count == 0) {
                    continue;
                } else if (cell.movingHere.Count == 1) {
                    cell.Nanobot = cell.movingHere[0];
                    cell.isExplored = true;
                    cell.movingHere.RemoveAt(0);
                } else {
                    while( cell.movingHere.Count != 0) {
                        FindObjectOfType<TimestepManager>().removeListener(cell.movingHere[0].GetComponent<Nanobot>());
                        Destroy(cell.movingHere[0]);
                        cell.movingHere.RemoveAt(0);
                    }
                }
            }
        }
    }

    public class GameCell
    {
        public BulletGridGenerator Grid;
        public GameObject Cell;
        public GameObject Nanobot;
        public List<GameObject> movingHere;
        public List<GameObject> goingAway;

        public delegate void VisibilityChangeHandler(GameCell cell, bool visibility);
        public event VisibilityChangeHandler NotifyVisibilityChanged;
        private bool _isExplored = false;
        public bool isExplored
        {
            get { return _isExplored; }
            set
            {
                _isExplored = value;
                if(NotifyVisibilityChanged != null) NotifyVisibilityChanged(this, _isExplored);
            }
        }
    }

    public class RemoveOnNextTick : TimestepManager.TimestepListener
    {
        GameObject _toDestroy;

        public RemoveOnNextTick(GameObject toDestroy)
        {
            _toDestroy = toDestroy;
        }

        public void notifyTimestep()
        {
            Destroy(_toDestroy);
        }
    }
}
