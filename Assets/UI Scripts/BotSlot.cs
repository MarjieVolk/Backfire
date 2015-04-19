using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotSlot : MonoBehaviour {

    public Nanobot nanobot;

    private PlacementMenuHandler handler;
    private Resources resources;

	// Use this for initialization
	void Start () {
        handler = GameObject.FindObjectOfType<PlacementMenuHandler>();
        resources = GameObject.FindObjectOfType<Resources>();

        if (nanobot == null) {
            // Init empty disabled button
            Destroy(transform.FindChild("Image").gameObject);
            foreach (Text text in transform.GetComponentsInChildren<Text>()) {
                text.text = "";
            }
            GetComponent<Button>().interactable = false;
        } else {
            // Init functioning button
            transform.FindChild("Image").GetComponent<Image>().sprite = nanobot.GetComponent<SpriteRenderer>().sprite;
            foreach (Text text in transform.GetComponentsInChildren<Text>()) {
                if (text.gameObject.name.Equals("ID")) {
                    text.text = nanobot.id;
                } else if (text.gameObject.name.Equals("Price")) {
                    text.text = "" + nanobot.price;
                }
            }
        }
    }

    void Update() {
        int placementResource = resources.getPlacementResourceAmount();

        if (nanobot != null) {
            GetComponent<Button>().interactable = nanobot.price <= placementResource;
        }
    }

    public void onClick() {
        handler.clickNanobot(nanobot);
    }
}
