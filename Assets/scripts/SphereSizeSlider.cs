using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Examples.InteractiveElements;

public class SphereSizeSlider : MonoBehaviour {
	
	//GameObject SpawnHotspots;
	GameObject trigger_sphere;
	float sliderValue;
	
	/* Update is called once per frame (Required because newly spawned trigger points need to be scaled) */
	void Update () {
		
		//Debug.Log("trial from slider: " + GameObject.Find("SpawnHotSpots").GetComponent<SpawnHotspots>().trial);
		// Change the static and trigger sphere size according to the slider value 
		if (GameObject.Find("SpawnHotSpots").GetComponent<SpawnHotspots>().trial < 3) {

			// Fill the static_spheres list with all the static spheres in the scene
			GameObject[] static_sphere_array;
			static_sphere_array = GameObject.FindGameObjectsWithTag("static_sphere");
			
			// Change all the static sphere sizes according to the slider value
			foreach(GameObject static_sphere in static_sphere_array)
			{
				static_sphere.transform.localScale = new Vector3(sliderValue + 0.035f, sliderValue + 0.035f, sliderValue + 0.035f);
			}

			// Change the trigger sphere size according to the slider value 
			trigger_sphere = GameObject.FindGameObjectWithTag("trigger_sphere");
			trigger_sphere.transform.localScale = new Vector3(sliderValue + 0.036f, sliderValue + 0.036f, sliderValue + 0.036f); // Make the trigger slightly larger than the static points
		}

	}

	// Get slider value, called by slider's event update
	public void getSlider()
	{
	
		// Get the slider's current value 
		GameObject slider = GameObject.Find("Sphere_Size_Slider"); // Grab sphere size slider from scene
		SliderGestureControl sliderScript = slider.GetComponent<SliderGestureControl>(); // Grab script off of slider
		sliderValue = sliderScript.GetSliderValue ();
	}
	
}
