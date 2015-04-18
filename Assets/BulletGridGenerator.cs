using UnityEngine;
using System.Collections;

public class BulletGridGenerator : MonoBehaviour {

    public GameObject CellPrefab;

	// Use this for initialization
	void Start () {
        Vector2 center = gameObject.transform.position;
        float realWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        float realHeight = GetComponent<SpriteRenderer>().bounds.size.y;

        Texture2D levelDescriptor = GetComponent<SpriteRenderer>().sprite.texture;
        float cellsWidth = levelDescriptor.width;
        float cellsHeight = levelDescriptor.height;

        Vector2 botLeft = center - new Vector2(realWidth / 2, realHeight / 2);
        float cellWidth = realWidth / cellsWidth;
        float cellHeight = realHeight / cellsHeight;

        for (int x = 0; x < cellsWidth; x++)
        {
            for (int y = 0; y < cellsHeight; y++)
            {
                Vector2 cellCenter = botLeft + new Vector2(cellWidth * x, cellHeight * y);

                GameObject cell = (GameObject)Instantiate(CellPrefab, cellCenter, Quaternion.identity);
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
