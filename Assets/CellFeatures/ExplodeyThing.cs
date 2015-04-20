using UnityEngine;
using System.Collections;

public class ExplodeyThing : CellFeature
{
    private BulletGridGenerator _grid;

    public Sprite DangerZoneSprite;
    public int DangerZoneThreshold;

    public int TerrainDamage;
    public int TerrainSplashDamage;
    public int radius;

    public AudioClip armSound;
    public AudioClip explodeSound;

    private bool armSoundTriggered = false;

	// Use this for initialization
	void Start () {
        _grid = FindObjectOfType<BulletGridGenerator>();
        NotifyResourceConsumed += ResourceConsumedHandler;
        gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (SoundManager.instance != null && _grid.GameGrid[Cell.GridPosition.X][Cell.GridPosition.Y].Nanobot != null && !armSoundTriggered) {
            armSoundTriggered = true;
            SoundManager.instance.PlaySingle(GetComponent<AudioSource>(), armSound);
        }
	}

    void ResourceConsumedHandler(int resourcesConsumed, bool exploded)
    {
        if (resourcesConsumed == 0) return;
        if (Amount == 0)
        {
            explode();
        }

        if (Amount < DangerZoneThreshold)
        {
            Sprite = DangerZoneSprite;
        }
    }

    void explode()
    {
        if (SoundManager.instance != null) {
            SoundManager.instance.PlaySingle(GetComponent<AudioSource>(), explodeSound);
        }
        GridPosition cellPosition = Cell.GridPosition;
        explodeNanobots(cellPosition);
        explodeTerrain(cellPosition);
    }

    void explodeNanobots(GridPosition position) {
        for (int x = -radius; x <= radius; x++) {
            for (int y = -radius; y <= radius; y++) {
                GridPosition offset = new GridPosition(x, y);
                GridPosition adjacent = _grid.applyDelta(position, offset);
                if (adjacent != null) {
                    _grid.DestroyNanobotAt(adjacent);
                }
            }
        }
    }

    void explodeTerrain(GridPosition position)
    {
        for (int x = -radius; x <= radius; x++) {
            for (int y = -radius; y <= radius; y++) {
                GridPosition offset = new GridPosition(x, y);
                GridPosition adjacent = _grid.applyDelta(position, offset);
                if (adjacent != null) {
                    _grid.getCellAt(adjacent).Cell.GetComponent<Cell>().Eat(TerrainSplashDamage, true);
                }
            }
        }
    }
}
