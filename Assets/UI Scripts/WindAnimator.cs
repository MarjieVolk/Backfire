using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WindAnimator : MonoBehaviour {

    public float chanceToGenerate;
    public Sprite[] sprites;

    private System.Random gen;
    private List<Wind> winds;

    void Start() {
        gen = new System.Random();
        winds = new List<Wind>();
    }

	void Update () {
	    if (gen.NextDouble() < chanceToGenerate) {
            GameObject obj = new GameObject();
            obj.AddComponent<SpriteRenderer>().sprite = sprites[gen.Next(sprites.Length)];

            float y = (float) gen.NextDouble() * 10f - 5f;
            obj.transform.position = new Vector3(-20, y, 200);

            Wind wind = new Wind();
            wind.obj = obj;
            wind.speed = (float) gen.NextDouble() / 30f;
            winds.Add(wind);
        }

        List<Wind> toRemove = new List<Wind>();
        foreach (Wind wind in winds) {
            float newX = wind.obj.transform.position.x + wind.speed;
            if (newX > 20) {
                toRemove.Add(wind);
            } else {
                wind.obj.transform.position = new Vector3(newX, wind.obj.transform.position.y, wind.obj.transform.position.z);
            }
        }

        foreach (Wind wind in toRemove) {
            winds.Remove(wind);
            GameObject.Destroy(wind.obj);
        }
	}

    private class Wind {
        public GameObject obj;
        public float speed;
    }
}
