using UnityEngine;
using System.Collections;

public class GlobalObject : MonoBehaviour {

	public int boardSize; 
	public int difficulty;
	public int numPlayers;
	public string p1JoyMap;
	public string p2JoyMap;
	public string p3JoyMap;
	public string p4JoyMap;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
