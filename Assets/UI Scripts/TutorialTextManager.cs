using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class TutorialTextManager : MonoBehaviour {
    
    public float pitEncounteredTime = 0;
    public bool bombExploded = false;
    public float unclickableTimeSeconds = 1;
    public AudioClip voiceSound;
    public Sprite jumpImage, collisionImage;

    private List<TutorialMessage> messages;
    private float timePopupStarted;

    private Resources resources;
    private TimestepManager timestepManager;

    void Start() {
        resources = GameObject.FindObjectOfType<Resources>();
        timestepManager = GameObject.FindObjectOfType<TimestepManager>();

        transform.GetChild(0).gameObject.SetActive(false);
        messages = new List<TutorialMessage>();
        gameObject.AddComponent<AudioSource>();

        if (Application.loadedLevelName.Contains("OneBulletPrototype")) {
            // Level 1 messages
            TutorialMessage message = new TutorialMessage();
            message.trigger = () => {
                return true;
            };
            message.message = "Friends. We find ourselves on a metal planet, flying through space.  Leadership is needed.  We have formed the Nanocouncil. This planet is made of <placement resource>, which can be converted into additional nanobots. Begin <placement resource> assimilation.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getJumpResourceAmount() >= LevelWinDetector.jumpResourceForJump;
            };
            message.message = "Joyous accomplishment. We have assimilated <jump resource> in sufficiency to facilitate propulsion system fabrication. Begin interplanetary transit procedure.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getJumpResourceAmount() > 0;
            };
            message.message = "Friends. We have encountered a foreign substance. <jump resource> may be useful for nanobot acceleration. With enough <jump resource>, the Nanocouncil may be capable of propulsion system fabrication. Begin <jump resource> assimilation.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getPlacementResourceAmount() >= 10;
            };
            message.message = "Friends. The Nanocouncil has calculated that this planet’s resources cannot sustain infinite expansion. Favorably we have detected a nearby planet with more plentiful resources. No methods of transit to alien planet known at this time. Further exploration required.";
            message.image = jumpImage;
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return pitEncounteredTime > 0 && (Time.time - pitEncounteredTime > 2);
            };
            message.message = "Unfavorable! We have lost a fellow friend to a hole on the planetary surface.  Planetary integrity decreasing.  Caution encouraged.";
            messages.Add(message);
        }

        if (Application.loadedLevelName.Contains("Level 2")) {
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
            message.message = "#12A7()?; .*\\1ave encountered a miraculous substance.  <upgrade resource> may be used for extreme nanobot enhancement.  Begin <upgrade resource> assimilation.  Atypical rapidity encouraged.";
            messages.Add(message);
        }

        if (Application.loadedLevelName.Contains("Level 3")) {
            // Level 3 messages
            TutorialMessage message = new TutorialMessage();
            message.trigger = () => {
                return true;
            };
            message.message = "Friends.  This planet is populated with a substance identified as danger inducing.  Approach <detonating terrain> with caution.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return bombExploded;
            };
            message.message = "Let us take a microsecond to regret the untimely combustion of our fellow friends.";
            messages.Add(message);

            message = new TutorialMessage();
            message.trigger = () => {
                return resources.getJumpResourceAmount() > 10;
            };
            message.message = "Most unfavorable of verifications.  The Nanocouncil has calculated that our planet is on a collision course with an unknown body of unfathomable proportions.  Continuation of backwards trajectory is imperative.  Atypical rapidity advised.";
            message.image = collisionImage;
            messages.Add(message);
        }
    }

    void Update() {
        if (!transform.GetChild(0).gameObject.activeSelf) {
            // Not active - check for more messages
            foreach (TutorialMessage message in messages) {
                if (message.trigger()) {
                    displayMessage(message.message, message.image);
                    messages.Remove(message);
                    break;
                }
            }
        }
    }

    public void displayMessage(string text, Sprite image) {
        GameObject panel = transform.GetChild(0).gameObject;
        panel.SetActive(true);
        panel.GetComponentInChildren<Text>().text = text;
        panel.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = image;
        timePopupStarted = Time.time;
        timestepManager.setPaused(true);
        if (SoundManager.instance != null) {
            SoundManager.instance.PlaySingle(gameObject.GetComponent<AudioSource>(), voiceSound);
        }
        GameObject.FindObjectOfType<PlacementMenuHandler>().stopDragging();
    }

    public void onClick() {
        if (Time.time - timePopupStarted > unclickableTimeSeconds) {
            transform.GetChild(0).gameObject.SetActive(false);
            timestepManager.setPaused(false);
        }
    }

    private class TutorialMessage {
        public string message;
        public Sprite image = null;
        public Func<Boolean> trigger;
    }
}
