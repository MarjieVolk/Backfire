using UnityEngine;
using System.Collections;
using System;

public class Cell : MonoBehaviour {

    // resources / 'tile features', in some order, with some amount each
    // events that fire when tile features are depleted?
    // those are part of the tile features
    public CellFeature[] CellFeatures;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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
            // set texture to that of CellFeatures[i]
        }

        Eat(1);
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
}
