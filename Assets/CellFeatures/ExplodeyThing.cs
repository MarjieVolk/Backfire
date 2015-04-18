using UnityEngine;
using System.Collections;

public class ExplodeyThing : CellFeature
{
    public Sprite DangerZoneSprite;
    public int DangerZoneThreshold;
    public override int Amount
    {
        set
        {
            _amount = value;

            if (_amount == 0)
            {
                explode();
            }

            if (_amount < DangerZoneThreshold)
            {
                Sprite = DangerZoneSprite;
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void explode()
    {
        Cell cell = GetComponent<Cell>();
        // TODO get cell position
        // TODO explode things here (and adjacent?), maybe damage terrain, certainly kill some nanobots
    }
}
