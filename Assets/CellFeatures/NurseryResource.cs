using UnityEngine;
using System.Collections;

public class NurseryResource : CellFeature {

    public override int Amount
    {
        set
        {
            // add _amount - value to the player's resource store
            _amount = value;
            Debug.Log("child thingy");
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
