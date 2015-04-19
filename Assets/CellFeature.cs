using UnityEngine;
using System.Collections;
using System;

public class CellFeature : MonoBehaviour {
    public Sprite Sprite;
    public int StartingAmount;

    protected int _amount;
    public virtual int Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
