using UnityEngine;
using System.Collections;

public class PreviewBlock : MonoBehaviour
{

	public int color;
	public int shape;

	private float expireTime;
	private float currTime;
	private GameManager mgr;
	private bool checkExpire;

	// Use this for initialization
	void Start ()
	{
		mgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		currTime += Time.deltaTime;

		if (checkExpire && currTime > expireTime) {
			mgr.ExpirePreview ();
		}
	}

	public void SetExpire(float time)
	{
		checkExpire = true;
		expireTime = time;
	}

	// Keep in sync with Block.cs
	public void SetBlockProperties (int inColor, int inShape)
	{
		SpriteRenderer blockSprite = GetComponent<SpriteRenderer> ();
		if (inColor == 0) {
			int newColor = Random.Range (1, 7);
			color = newColor;
		} else {
			color = inColor;
		}
		if (inShape == 0) {
			int newShape = Random.Range (1, 6); 
			shape = newShape;
		} else {
			shape = inShape;
		}

		if (color == 1) {
			blockSprite.sprite = Resources.Load<Sprite> ("Images/wildcard_sprite");
		} else {
			blockSprite.sprite = Resources.Load<Sprite> ("Images/block_sprite");
		}

		switch (color) {
		case 1:
			shape = 1; // random shape that doesn't get used
			return; //no colors or fruits on a wildcard
		case 2:
			blockSprite.material = Resources.Load<Material> ("Images/BlueMat");
			break;
		case 3:
			blockSprite.material = Resources.Load<Material> ("Images/GreenMat");
			break;
		case 4:
			blockSprite.material = Resources.Load<Material> ("Images/YellowMat");
			break;
		case 5:
			blockSprite.material = Resources.Load<Material> ("Images/RedMat");
			break;
		case 6:
			blockSprite.material = Resources.Load<Material> ("Images/PurpleMat");
			break;
		case 7:
			blockSprite.material = Resources.Load<Material> ("Images/GreyMat");
			break;
		case 8:
			blockSprite.material = Resources.Load<Material> ("Images/DarkBlueMat");
			break;
		case 9:
			blockSprite.material = Resources.Load<Material> ("Images/OrangeMat");
			break;
		}

		FruitShape fruitShape = GetComponentInChildren<FruitShape> ();
		SpriteRenderer fruitSprite = fruitShape.GetComponent<SpriteRenderer> ();

		switch (shape) {
		case 1:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitApple");
			break;
		case 2:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitBanana");
			break;
		case 3:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitCherries");
			break;
		case 4:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitGrapes");
			break;
		case 5:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitLemon");
			break;
		case 6:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitPear");
			break;
		case 7:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitPineapple");
			break;
		case 8:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitStrawberry");
			break;
		}
	}
}
