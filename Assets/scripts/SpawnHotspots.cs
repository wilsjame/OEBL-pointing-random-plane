using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Threading;


public class SpawnHotspots : MonoBehaviour {

	/* Parent gameObject to hold generated hotspot collection */
	GameObject parentObject;

	/* Prefabs */
	public Transform static_point;
	public Transform trigger_point;
	public Transform trial_counter;

	/* Encapsulated trial counter coordinates */ 
	public struct CoOrds
	{
		public float x, y, z;
		public string plane;

		/* Constructor to initiliaze x, y, and z coordinates */
		public CoOrds(float x_coOrd, float y_coOrd, float z_coOrd, string p)
		{
			x = x_coOrd;
			y = y_coOrd;
			z = z_coOrd;
			plane = p;
		}
	}

	List<List<CoOrds>> coOrds_collection = new List<List<CoOrds>> ();	/* Entire point collection */
	List<CoOrds> coOrds_collection_1 = new List<CoOrds> (); /* z = 0.0 frame points */
	List<CoOrds> coOrds_collection_2 = new List<CoOrds> (); /* z = 0.3 frame points */
	List<CoOrds> coOrds_collection_3 = new List<CoOrds> (); /* z = 0.6 frame points */
	List<CoOrds> counter_collection = new List<CoOrds> (); 	/* Trial counter coordinates */ 
	public int[] order = {0, 1, 2};				/* Plane spawn order */
	public int itr = 0;					/* Keep track of list iterations */
	public int plane = 0;					/* Keep track of completed planes */
	public int trial = 0;					/* Keep track of completed trials */

	public string fileName = "pointing_random_plane_task_time_";
	public string path;

	public Stopwatch stopwatch = new Stopwatch();
	/*
	public System.TimeSpan trial_time;
	public System.TimeSpan plane_1_time;
	public System.TimeSpan plane_2_time;
	public System.TimeSpan plane_3_time;
	*/
	
	/* Use this for initialization */
	void Start () {

		// Create unique out file 
		fileName = fileName + System.DateTime.Now + ".txt";
		fileName = fileName.Replace("/","-");
		fileName = fileName.Replace(":",";");
		path = Path.Combine(Application.persistentDataPath, fileName);
		UnityEngine.Debug.Log(fileName);
		UnityEngine.Debug.Log(Application.persistentDataPath);	

		/* Generate */
		initializeCoordinates (ref order, ref coOrds_collection, ref coOrds_collection_1, ref coOrds_collection_2, ref coOrds_collection_3);

		/* Call function once on startup to create initial hotspot */
		HotSpotTriggerInstantiate ();
	}

	/* Generate circular arrays of static points */ 
	public void initializeCoordinates (ref int[] order, ref List<List<CoOrds>> coOrds_collection, ref List<CoOrds> coOrds_collection_1, ref List<CoOrds> coOrds_collection_2, ref List<CoOrds> coOrds_collection_3)
	{
		int i;
		int temp;
		CoOrds temp_vector;
		int random_placeholder;
		int numberOfObjects = 18;
		float radius = .5f;

		/* z = 0 frame */
		for (i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			CoOrds pos = new CoOrds(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0, "back");
			coOrds_collection_1.Add(pos);
		}

		/* Shuffle */
		for (i = 0; i < numberOfObjects; i++) {
			random_placeholder = i + Random.Range (0, numberOfObjects - i);

			/* Swap */
			temp_vector = coOrds_collection_1[i];
			coOrds_collection_1[i] = coOrds_collection_1[random_placeholder];
			coOrds_collection_1[random_placeholder] = temp_vector;
		}

		/* z = 0.3 frame */
		for (i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			CoOrds pos = new CoOrds(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0.3f, "middle");
			coOrds_collection_2.Add(pos);
		}

		/* Shuffle */
		for (i = 0; i < numberOfObjects; i++) {
			random_placeholder = i + Random.Range (0, numberOfObjects - i);

			/* Swap */
			temp_vector = coOrds_collection_2[i];
			coOrds_collection_2[i] = coOrds_collection_2[random_placeholder];
			coOrds_collection_2[random_placeholder] = temp_vector;
		}

		/* z = 0.6 frame */
		for (i = 0; i < numberOfObjects; i++) {
			float angle = i * Mathf.PI * 2 / numberOfObjects;
			CoOrds pos = new CoOrds(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0.6f, "front");
			coOrds_collection_3.Add(pos);
		}

		/* Shuffle */
		for (i = 0; i < numberOfObjects; i++) {
			random_placeholder = i + Random.Range (0, numberOfObjects - i);

			/* Swap */
			temp_vector = coOrds_collection_3[i];
			coOrds_collection_3[i] = coOrds_collection_3[random_placeholder];
			coOrds_collection_3[random_placeholder] = temp_vector;
		}

		/* Add planes to entire collection */
		coOrds_collection.Add(coOrds_collection_1);
		coOrds_collection.Add(coOrds_collection_2);
		coOrds_collection.Add(coOrds_collection_3);

		/* Shuffle plane order */ 
		//UnityEngine.Debug.Log("Plane order before shuffle: " + order[0] + order[1] + order[2]);
		
		for (i = 0; i < 3; i++) {
			random_placeholder = i + Random.Range (0, 3 - i);

			/* Swap */
			temp = order[i];
			order[i] = order[random_placeholder];
			order[random_placeholder] = temp;
		}
		
		//UnityEngine.Debug.Log("Plane order after shuffle: " + order[0] + order[1] + order[2]);

		/* Trial counters */ 
		CoOrds counter_1 = new CoOrds (0.71f, 0.5f, 0.0f, null);
		counter_collection.Add (counter_1);
		CoOrds counter_2 = new CoOrds (0.81f, 0.5f, 0.0f, null);
		counter_collection.Add (counter_2);
		CoOrds counter_3 = new CoOrds (0.91f, 0.5f, 0.0f, null);
		counter_collection.Add (counter_3);	

		/* Spawn initial static points */ 
		for (i = 0; i < numberOfObjects; i++) {
			temp_vector = coOrds_collection[order[trial]] [i];
			Transform static_pt = Instantiate(static_point, new Vector3 (temp_vector.x, temp_vector.y, temp_vector.z), Quaternion.identity, this.transform); // Make this gameObject the parent

			switch (temp_vector.plane) {
				case "front":
					static_pt.GetComponent<StaticSpot>().plane = "front";
					break;
				case "middle":
					static_pt.GetComponent<StaticSpot>().plane = "middle";
					break;
				case "back":
					static_pt.GetComponent<StaticSpot>().plane = "back";
					break;
			}

		}

	}

	/* Destroy finished plane and spawn a new one */ 
	public void newPlane ()
	{
		CoOrds coords_temp = new CoOrds ();
		itr = 0;

		/* Destroy completed plane */ 
		GameObject[] completed = GameObject.FindGameObjectsWithTag ("static_sphere");

		for (var i = 0; i < completed.Length; i++) {
			Destroy(completed[i]);
		}

		/* Spawn new plane's static points */
		for (int i = 0; i < coOrds_collection[order[plane]].Count; i++) {
			coords_temp = coOrds_collection[order[plane]] [i];
			Transform static_pt = Instantiate (static_point, new Vector3 (coords_temp.x, coords_temp.y, coords_temp.z), Quaternion.identity, this.transform); // Make this gameObject the parent

		switch (coords_temp.plane) {
			case "front":
				static_pt.GetComponent<StaticSpot>().plane = "front";
				break;
			case "middle":
				static_pt.GetComponent<StaticSpot>().plane = "middle";
				break;
			case "back":
				static_pt.GetComponent<StaticSpot>().plane = "back";
				break;
		}

			static_pt.localPosition = new Vector3 (coords_temp.x, coords_temp.y, coords_temp.z); // Spawn position relative to parent
		}

		/* Spawn new plane's intial trigger point */
		coords_temp = coOrds_collection[order[plane]] [itr];
		Transform trigger = Instantiate (trigger_point, new Vector3 (coords_temp.x, coords_temp.y, coords_temp.z), Quaternion.identity, this.transform); // Make this gameObject the parent
		trigger.localPosition = new Vector3 (coords_temp.x, coords_temp.y, coords_temp.z); // Spawn position relative to parent
		itr++;
	}

	/* Spawn trigger points until 3 trials are completed */
	public void HotSpotTriggerInstantiate ()
	{
		CoOrds coords_temp = new CoOrds ();

		/* Check if user has tapped first point */
		if (itr == 1) {

			// Begin trial timing
			stopwatch.Start();
		}
		
		/* Begin spawning */
		if (plane < 3 && itr != coOrds_collection[order[plane]].Count) {

			/* Spawn the trigger point */ 
			coords_temp = coOrds_collection[order[plane]] [itr];
			Transform trigger = Instantiate (trigger_point, new Vector3 (coords_temp.x, coords_temp.y, coords_temp.z), Quaternion.identity, this.transform); // Make this gameObject the parent
			trigger.localPosition = new Vector3 (coords_temp.x, coords_temp.y, coords_temp.z); // Spawn position relative to parent
			itr++;
		}

		/* Spawn new plane */
		else if (++plane < 3) {

			// Stop timing
			System.TimeSpan ts = stopwatch.Elapsed;
			stopwatch.Stop();
			UnityEngine.Debug.Log("Plane " + plane + " : " + ts + " " + GameObject.Find("static_point(Clone)").GetComponent<StaticSpot>().plane);
			stopwatch.Reset();

			// Write time to file
			File.AppendAllText(@path, "Plane " + plane + " : ");
			File.AppendAllText(@path, ts.ToString() + " " + GameObject.Find("static_point(Clone)").GetComponent<StaticSpot>().plane);
			File.AppendAllText(@path, "\r\n");

			newPlane();
		}

		/* Reset planes and spawn trial counter */
		else if (trial < 3) {

			// Stop timing
			System.TimeSpan ts = stopwatch.Elapsed;
			stopwatch.Stop();
			UnityEngine.Debug.Log("Plane " + plane + " : " + ts + " " + GameObject.Find("static_point(Clone)").GetComponent<StaticSpot>().plane);
			stopwatch.Reset();

			// Write time to file
			File.AppendAllText(@path, "Plane " + plane + " : ");
			File.AppendAllText(@path, ts.ToString() + " " + GameObject.Find("static_point(Clone)").GetComponent<StaticSpot>().plane);
			File.AppendAllText(@path, "\r\n");

			// Spawn trial counter
			trial++;
			UnityEngine.Debug.Log("Trial " + trial + " completed!");
			coords_temp = counter_collection [trial - 1];
			Instantiate (trial_counter, new Vector3 (coords_temp.x, coords_temp.y, coords_temp.z), Quaternion.identity);

			// Reset trial
			if (trial < 3) {
				//TODO shuffle planes before new trial
				plane = 0;
				newPlane();
			}

		}

	}

}

