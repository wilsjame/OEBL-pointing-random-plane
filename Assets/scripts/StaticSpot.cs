/* Attach to static spheres to change colors according to their plane. */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticSpot : MonoBehaviour 
{
    private Material cachedMaterial;
    public string plane;

    private void Start()
    {

	/* Color hotspot according to plane. */
	cachedMaterial = GetComponent<Renderer>().material;

	switch (plane) {
		case "front":
			cachedMaterial.SetColor("_Color", Color.yellow);
			break;
		case "middle":
			cachedMaterial.SetColor("_Color", Color.green);
			break;
		case "back":
			cachedMaterial.SetColor("_Color", new Color(.12f, .56f, 1.0f, 1f));
			break;
		default:
			break;
	}

    }

}
