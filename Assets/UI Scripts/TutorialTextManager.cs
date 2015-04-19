using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class TutorialTextManager : MonoBehaviour {

    private List<TutorialMessage> messages;

    private Resources resources;

    void Start() {
        resources = GameObject.FindObjectOfType<Resources>();

        onClick(); // hide panel
        messages = new List<TutorialMessage>();

        TutorialMessage message = new TutorialMessage();
        message.trigger = () => {
            return EditorApplication.currentScene.Contains("OneBulletPrototype");
        };
        message.message = "Friends. We find ourselves on a metal planet, flying through space.  Leadership is needed.  We have formed the Nanocouncil. This planet is made of (placement resource), which can be converted into additional nanobots. Begin (placement resource) assimilation.";
        messages.Add(message);

        message = new TutorialMessage();
        message.trigger = () => {
            return resources.getJumpResourceAmount() >= 50;
        };
        message.message = "Joyous accomplishment. We have assimilated (jump resource) in sufficiency to facilitate propulsion system fabrication. Begin interplanetary transit procedure";
        messages.Add(message);

        message = new TutorialMessage();
        message.trigger = () => {
            return resources.getJumpResourceAmount() > 0;
        };
        message.message = "Friends.  We have encountered a foreign substance.  (Jump resource) may be useful for nanobot enhancement.  With enough (jump resource), the Nanocouncil may be capable of propulsion system fabrication.  Begin (jump resource) assimilation";
        messages.Add(message);
    }

    void Update() {
        foreach (TutorialMessage message in messages) {
            if (message.trigger()) {
                displayText(message.message);
                messages.Remove(message);
                break;
            }
        }
    }

    public void displayText(string text) {
        GameObject panel = transform.GetChild(0).gameObject;
        panel.SetActive(true);
        panel.GetComponentInChildren<Text>().text = text;
    }

    public void onClick() {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private class TutorialMessage {
        public string message;
        public Func<Boolean> trigger;
    }
}
