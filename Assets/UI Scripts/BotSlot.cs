using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotSlot : MonoBehaviour {

    public Nanobot nanobot;

    private PlacementMenuHandler handler;
    private Resources resources;

    private string _id;

	// Use this for initialization
	void Start () {
        handler = GameObject.FindObjectOfType<PlacementMenuHandler>();
        resources = GameObject.FindObjectOfType<Resources>();

        if (nanobot == null) {
            // Init empty disabled button
            Destroy(transform.FindChild("Image").gameObject);
            Destroy(transform.FindChild("SchematicGrid").gameObject);
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
                    _id = nanobot.id;
                } else if (text.gameObject.name.Equals("Price")) {
                    text.text = "" + nanobot.price;
                }
            }

            Transform schematicGrid = transform.FindChild("SchematicGrid");
            for (int x = 0; x < 3; x++) {
                Nanobot[] schematicColumn = null;
                switch (x) {
                    case 0:
                        schematicColumn = nanobot.schematic.transformationColumn1;
                        break;
                    case 1:
                        schematicColumn = nanobot.schematic.transformationColumn2;
                        break;
                    case 2:
                        schematicColumn = nanobot.schematic.transformationColumn3;
                        break;
                    default:
                        break;
                }

                for (int y = 0; y < 3; y++) {
                    Text text = schematicGrid.FindChild("Text_" + x + y).gameObject.GetComponent<Text>();

                    text.text = schematicColumn[y] == null ? "" : schematicColumn[y].id;
                }
            }
        }
    }

    void Update() {
        int placementResource = resources.getPlacementResourceAmount();

        if (nanobot != null) {
            GetComponent<Button>().interactable = nanobot.price <= placementResource;
            if (GetComponent<Button>().interactable && Input.GetKeyDown(_id.ToLower()))
            {
                handler.clickNanobot(nanobot);
            }
        }
    }

    public void onClick() {
        handler.clickNanobot(nanobot);
    }
}
