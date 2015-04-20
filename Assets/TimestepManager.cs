using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimestepManager : MonoBehaviour {

    public float timestepLengthSeconds = 1.5f;
    public AudioClip walkSound;

    private float previousTimestepSeconds = 0;
    private bool paused = false;

    private BulletGridGenerator level;

    private List<TimestepListener> listeners = new List<TimestepListener>();
    private List<TimestepListener> toAdd = new List<TimestepListener>();
    private List<TimestepListener> toRemove = new List<TimestepListener>();
    private List<TimestepListener> finalizers = new List<TimestepListener>();

    void Start() {
        level = GameObject.FindObjectOfType<BulletGridGenerator>();
        gameObject.AddComponent<AudioSource>();
    }

	// Update is called once per frame
	void Update () {
        if (paused) {
            return;
        }


        if (Time.time - previousTimestepSeconds >= timestepLengthSeconds) {
            previousTimestepSeconds = Time.time;
            foreach (TimestepListener listener in listeners) {
                listener.notifyTimestep();
            }
            foreach (TimestepListener finalizer in finalizers) {
                finalizer.notifyTimestep();
            }

            if (SoundManager.instance != null && hasNanobot()) {
                SoundManager.instance.RandomizeSfx(GetComponent<AudioSource>(), walkSound);
            }
        }
	}

    void LateUpdate()
    {
        listeners.AddRange(toAdd);
        toAdd.Clear();

        listeners.RemoveAll((item) => toRemove.Contains(item));
        toRemove.Clear();
    }

    public void setPaused(bool paused) {
        this.paused = paused;
    }

    public void addListener(TimestepListener listener) {
        toAdd.Add(listener);
    }

    public void removeListener(TimestepListener listener) {
        toRemove.Add(listener);
    }

    public void addFinalizer(TimestepListener finalizer) {
        finalizers.Add(finalizer);
    }

    public void removeFinalizer(TimestepListener finalizer) {
        finalizers.Remove(finalizer);
    }

    private bool hasNanobot() {
        foreach (BulletGridGenerator.GameCell[] column in level.GameGrid) {
            foreach (BulletGridGenerator.GameCell cell in column) {
                if (cell.Nanobot != null) {
                    return true;
                }
            }
        }

        return false;
    }

    public interface TimestepListener {
        void notifyTimestep();
    }
}
