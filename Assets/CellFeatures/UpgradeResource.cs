using UnityEngine;
using System.Collections;

public class UpgradeResource : CellFeature
{
    // Use this for initialization
    void Start()
    {
        NotifyResourceConsumed += ResourceConsumedHandler;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResourceConsumedHandler(int resourcesConsumed, bool exploded)
    {
        if (!exploded)
        {
            FindObjectOfType<Resources>().addUpgradeResource(resourcesConsumed);
        }
    }
}
