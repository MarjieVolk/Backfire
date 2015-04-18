using UnityEngine;
using System.Collections;
using System;

public class CellFeature : MonoBehaviour {
    public Sprite Sprite;
    public int StartingAmount;

    public int Amount { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int TryEat(int eatAmount, bool exploded)
    {
        if (Amount < eatAmount) eatAmount = Amount;
        Amount -= eatAmount;

        if(NotifyResourceConsumed != null) NotifyResourceConsumed(eatAmount, exploded);

        return eatAmount;
    }

    public delegate void ResourceConsumptionHandler(int consumedAmount, bool exploded);
    public event ResourceConsumptionHandler NotifyResourceConsumed;
}
