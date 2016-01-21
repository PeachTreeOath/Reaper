using UnityEngine;
using System.Collections;

public class GlobalObject : MonoBehaviour {

	public int boardSize; 

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
