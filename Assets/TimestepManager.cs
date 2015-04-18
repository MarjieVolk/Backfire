using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimestepManager : MonoBehaviour {

    public float timestepLengthSeconds = 1.5f;

    private float previousTimestepSeconds = 0;
    private List<TimestepListener> listeners = new List<TimestepListener>();
	
	// Update is called once per frame
	void Update () {
        if (Time.time - previousTimestepSeconds >= timestepLengthSeconds) {
            previousTimestepSeconds = Time.time;
            foreach (TimestepListener listener in listeners) {
                listener.notifyTimestep();
            }
        }
	}

    public void addListener(TimestepListener listener) {
        listeners.Add(listener);
    }

    public void removeListener(TimestepListener listener) {
        listeners.Remove(listener);
    }

    public interface TimestepListener {
        void notifyTimestep();
    }
}
