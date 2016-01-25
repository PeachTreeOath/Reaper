using UnityEngine;
using System.Collections;

public class Warp : MonoBehaviour
{

	public float phase1Duration;
	public float phase2Duration;

	private float currTime;
	private SpriteRenderer sprite;
	private GameManager mgr;
	// Use this for initialization
	void Start ()
	{
		mgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		currTime += Time.deltaTime;

		if (currTime > phase1Duration) {
			float elapsedPercent = (currTime - phase1Duration) / (phase2Duration - phase1Duration);
			sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 1-elapsedPercent);
		}
		if (currTime > phase2Duration) {
			GetComponentInParent<Block> ().finishedSpawning = true;
			mgr.CheckForMatches ();
			mgr.CreateNextBlock ();
			GameObject.Destroy (gameObject);
		}
	}
}
