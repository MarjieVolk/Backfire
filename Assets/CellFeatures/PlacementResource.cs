using UnityEngine;
using System.Collections;

public class PlacementResource : CellFeature {

    public override int Amount
    {
        set
        {
            _amount = value;
            FindObjectOfType<Resources>().addPlacementResource(_amount - value);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
