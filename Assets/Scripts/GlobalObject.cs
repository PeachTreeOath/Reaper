using UnityEngine;
using System.Collections;

public class GlobalObject : MonoBehaviour
{

	//public static GlobalObject instance;

	public int boardSize;
	public int difficulty;
	public int numPlayers;
	public string p1JoyMap;
	public string p2JoyMap;
	public string p3JoyMap;
	public string p4JoyMap;
	public int highestStage;

	private AudioClip titleTheme;
	private AudioClip gameTheme;
	private AudioClip gameOverTheme;
	private AudioSource audio;
	/*
	// Use this for initialization
	void Awake ()
	{
		
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad (transform.gameObject);
	}
	*/
	void Start ()
	{
		DontDestroyOnLoad (transform.gameObject);

		audio = GetComponent<AudioSource> ();
		titleTheme = (AudioClip)Resources.Load ("Audio/titletheme");
		gameTheme = (AudioClip)Resources.Load ("Audio/gameTheme");
		gameOverTheme = (AudioClip)Resources.Load ("Audio/gameOverTheme");

		audio.clip = titleTheme;
		audio.Play ();
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	public void PlayMusic (int theme)
	{
		switch (theme) {
		case 0:
			audio.clip = titleTheme;
			audio.loop = true;
			break;
		case 1:
			audio.clip = gameTheme;
			audio.loop = true;
			break;
		case 2:
			audio.clip = gameOverTheme;
			audio.loop = false;
			break;
		}
		audio.Play ();
	}

}
