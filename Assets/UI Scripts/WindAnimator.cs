using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindAnimator : MonoBehaviour {

    public float chanceToGenerate;
    public float speed = .1f;
    public Sprite[] sprites;

    private List<Wind> winds;

    void Start() {
        winds = new List<Wind>();
    }

	void Update () {
	    if (Random.value < chanceToGenerate) {
            GameObject obj = new GameObject();
            obj.AddComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];

            float y = (float) Random.value * 10f - 5f;
            obj.transform.position = new Vector3(-20, y, 200);

            Wind wind = new Wind();
            wind.obj = obj;
            winds.Add(wind);
        }

        List<Wind> toRemove = new List<Wind>();
        foreach (Wind wind in winds) {
            float newX = wind.obj.transform.position.x + speed;
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
    }
}
