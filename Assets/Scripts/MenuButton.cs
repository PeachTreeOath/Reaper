using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void NextLevel(){
		string size = GameObject.Find ("BoardSizeText").GetComponent<Text>().text; 
		GameObject.Find ("GlobalObject").GetComponent<GlobalObject> ().boardSize = System.Int32.Parse (size);  
		Application.LoadLevel (1);
	}
}
