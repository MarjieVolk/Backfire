using UnityEngine;
using System.Collections;

public class UpgradeResource : CellFeature
{

    public override int Amount
    {
        set
        {
            _amount = value;
            FindObjectOfType<Resources>().addUpgradeResource(_amount - value);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
