using UnityEngine;
using System.Collections;
using System;

public class Cell : MonoBehaviour {

    // resources / 'tile features', in some order, with some amount each
    // events that fire when tile features are depleted?
    // those are part of the tile features
    public CellFeature[] CellFeatures;
    public GridPosition GridPosition;

    private BulletGridGenerator level;

	// Use this for initialization
	void Start () {
        level = GameObject.FindObjectOfType<BulletGridGenerator>();
        for (int i = 0; i < CellFeatures.Length; i++)
        {
            CellFeatures[i] = Instantiate<CellFeature>(CellFeatures[i]);
            CellFeatures[i].Cell = this;
            CellFeatures[i].transform.parent = transform;
        }
	}

    // Update is called once per frame
    void Update()
    {
        //find the topmost resource and update the SpriteRenderer
        //TODO Don't do this every frame, dumbass
        int i = 0;
        while (i < CellFeatures.Length && CellFeatures[i].Amount == 0)
        {
            i++;
        }

        if (i == CellFeatures.Length)
        {
            // default texture of some kind?
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = CellFeatures[i].Sprite;
        }

        //Eat(1, false);
	}

    void OnMouseUpAsButton() {
        PlacementMenuHandler placementHandler = GameObject.FindObjectOfType<PlacementMenuHandler>();
        if (nanobotCanBePlacedHere() && placementHandler.isNanobotDragging() && GameObject.FindObjectOfType<Resources>().trySpendPlacementResource(placementHandler.getDraggedNanobot().price)) {
            // Add nanobot to this cell

            getGameCell().Nanobot = GameObject.Instantiate(placementHandler.getDraggedNanobot()).gameObject;
            getGameCell().Nanobot.transform.position = transform.position;
            GridPositionComponent gridPosition = getGameCell().Nanobot.GetComponent<GridPositionComponent>();
            gridPosition.position.X = this.GridPosition.X;
            gridPosition.position.Y = this.GridPosition.Y;
            placementHandler.stopDragging();
        }
    }

    public void Eat(int eatAmount, bool exploded)
    {
        for (int featureIndex = 0; featureIndex < CellFeatures.Length && eatAmount > 0; featureIndex++)
        {
            int consumedAmount = CellFeatures[featureIndex].TryEat(eatAmount, exploded);
            eatAmount -= consumedAmount;
        }
    }

    public bool nanobotCanBePlacedHere() {
        return getGameCell().Nanobot == null && getGameCell().isExplored;
    }

    private BulletGridGenerator.GameCell getGameCell() {
        return level.GameGrid[GridPosition.X][GridPosition.Y];
    }
}
