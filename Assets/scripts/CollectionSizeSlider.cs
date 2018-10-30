using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Examples.InteractiveElements;

public class CollectionSizeSlider : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Get slider value, called by slider's event update
	public void getSlider()
	{

		// Get the slider's current value 
		GameObject slider = GameObject.Find("Collection_Size_Slider"); // Grab collection size slider from scene
		
		SliderGestureControl sliderScript = slider.GetComponent<SliderGestureControl>(); // Grab script off of slider
		float sliderValue = sliderScript.GetSliderValue ();

		// Change the sphere collection size according to the slider value
		GameObject sphere_collection = GameObject.Find("SpawnHotSpots"); // Grab the collection from scene
		sphere_collection.transform.localScale = new Vector3(sliderValue, sliderValue, sliderValue);
	}
		
}
