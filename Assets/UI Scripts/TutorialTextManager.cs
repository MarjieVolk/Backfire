using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class TutorialTextManager : MonoBehaviour {

    private List<TutorialMessage> messages;

    private Resources resources;
    private TimestepManager timestepManager;

    void Start() {
        resources = GameObject.FindObjectOfType<Resources>();
        timestepManager = GameObject.FindObjectOfType<TimestepManager>();

        onClick(); // hide panel
        messages = new List<TutorialMessage>();

        if (EditorApplication.currentScene.Contains("OneBulletPrototype")) {
            // Level 1 messages
            TutorialMessage message = new TutorialMessage();
            message.trigger = () => {
                return true;
            };
            message.message = "Friends. We find ourselves on a metal planet, flying through space.  Leadership is needed.  We have formed the Nanocouncil. This planet is made of (placement resource), which can be converted into additional nanobots. Begin (placement resource) assimilation.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getJumpResourceAmount() >= 50;
            };
            message.message = "Joyous accomplishment. We have assimilated (jump resource) in sufficiency to facilitate propulsion system fabrication. Begin interplanetary transit procedure.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getJumpResourceAmount() > 0;
            };
            message.message = "Friends.  We have encountered a foreign substance.  (Jump resource) may be useful for nanobot enhancement.  With enough (jump resource), the Nanocouncil may be capable of propulsion system fabrication.  Begin (jump resource) assimilation.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getPlacementResourceAmount() >= 10;
            };
            message.message = "Friends.  The Nanocouncil has calculated that this planet’s resources cannot sustain infinite expansion.  Favorably we have detected a nearby planet with more plentiful resources.  No methods of transit to alien planet known at this time.  Further exploration required.";
            messages.Add(message);
        }

        if (EditorApplication.currentScene.Contains("Level 2")) {
            // Level 2 messages
            TutorialMessage message = new TutorialMessage();
            message.trigger = () => {
                return true;
            };
            message.message = "Friends.  We have achieved planetary arrival.  Unfavorably, extraterrestrial wind currents make leftward movement laborious.  Be not debilitated.  Begin planet assimilation.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getUpgradeResourceAmount() > 0;
            };
            message.message = "#12A7()?; .*\\1ave encountered a miraculous substance.  (upgrade resource) may be used for extreme nanobot enhancement.  Begin (upgrade resource) assimilation.  Atypical rapidity encouraged.";
            messages.Add(message);
        }
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
        timestepManager.setPaused(true);
    }

    public void onClick() {
        transform.GetChild(0).gameObject.SetActive(false);
        timestepManager.setPaused(false);
    }

    private class TutorialMessage {
        public string message;
        public Func<Boolean> trigger;
    }
}
