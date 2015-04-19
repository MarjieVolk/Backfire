using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour, TimestepManager.TimestepListener {

    BulletGridGenerator currentLevel;
    public NanobotSchematic schematic;

	// Use this for initialization
	void Start () {
        currentLevel = FindObjectOfType<BulletGridGenerator>();
        GameObject.FindObjectOfType<TimestepManager>().addListener(this);
	}

    void Awake() {
        schematic = new NanobotSchematic();
    }

    public void notifyTimestep() {
        Debug.Log(string.Format("Bot at {0}/{1} spawning.", GetComponent<GridPositionComponent>().position.X, GetComponent<GridPositionComponent>().position.Y));
        for (int x = 0; x < schematic.transformation.Length; x++) {
            for (int y = 0; y < schematic.transformation[x].Length; y++) {
                currentLevel.placeBot(gameObject, schematic.transformation[x][y], x, y);
            }
        }
        GameObject.FindObjectOfType<TimestepManager>().removeListener(this);
        Destroy(gameObject);
    }
}
