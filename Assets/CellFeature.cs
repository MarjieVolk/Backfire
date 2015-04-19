using UnityEngine;
using System.Collections;
using System;

public class CellFeature : MonoBehaviour {
    public Sprite Sprite;
    public int StartingAmount;

    public Cell Cell;
    public int Amount { get; private set; }

    void Awake() // TODO if any subclass impl Awake() this breaks; bad!  Fix!
    {
        Amount = StartingAmount;
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
