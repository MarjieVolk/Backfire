using UnityEngine;
using System.Collections;
using System;

public class Cell : MonoBehaviour {

    // resources / 'tile features', in some order, with some amount each
    // events that fire when tile features are depleted?
    // those are part of the tile features
    public CellFeature[] CellFeatures;

    private int x, y;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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

        Eat(1);
	}

    void OnClick() {
        if (nanobotCanBePlacedHere() && GameObject.FindObjectOfType<PlacementMenuHandler>().isNanobotDragging()) {
            // Add nanobot to this cell

        }
    }

    public void Eat(int eatAmount)
    {
        for (int featureIndex = 0; featureIndex < CellFeatures.Length && eatAmount > 0; featureIndex++)
        {
            // compute how much we will be able to eat
            int cellAmount = CellFeatures[featureIndex].Amount;
            int consumedAmount = Math.Min(cellAmount, eatAmount);

            // eat it
            eatAmount -= consumedAmount;
            CellFeatures[featureIndex].Amount -= consumedAmount;
        }
    }

    public void setPosition(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public bool nanobotCanBePlacedHere() {
        return getGameCell().Nanobot == null;
    }

    private BulletGridGenerator.GameCell getGameCell() {
        return GameObject.FindObjectOfType<BulletGridGenerator>().GameGrid[x][y];
    }
}
