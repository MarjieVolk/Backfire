using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimestepManager : MonoBehaviour {

    public float timestepLengthSeconds = 1.5f;

    private float previousTimestepSeconds = 0;
    private List<TimestepListener> listeners = new List<TimestepListener>();
    private List<TimestepListener> toAdd = new List<TimestepListener>();
    private List<TimestepListener> toRemove = new List<TimestepListener>();
    private List<TimestepListener> finalizers = new List<TimestepListener>();
	
	// Update is called once per frame
	void Update () {
        if (Time.time - previousTimestepSeconds >= timestepLengthSeconds) {
            previousTimestepSeconds = Time.time;
            foreach (TimestepListener listener in listeners) {
                listener.notifyTimestep();
            }
            foreach (TimestepListener finalizer in finalizers) {
                finalizer.notifyTimestep();
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

    public interface TimestepListener {
        void notifyTimestep();
    }
}
