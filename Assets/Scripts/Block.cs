using UnityEngine;
using System.Collections;

public class Block : BoardObject
{

	public int color;
	public int shape; 
	public bool toDelete;

	// Use this for initialization
	void Start ()
	{
		boardType = BoardType.BLOCK;
		SetBlockProperties ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update ();
	}

	private void SetBlockProperties ()
	{
		SpriteRenderer blockSprite = GetComponent<SpriteRenderer> ();
		int newColor = Random.Range (1, 6);

		if (newColor == 1) {
			blockSprite.sprite = Resources.Load<Sprite> ("Images/wildcard_sprite");
		} else {
			blockSprite.sprite = Resources.Load<Sprite> ("Images/block_sprite");
		}
		color = newColor;

		switch (newColor) {
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
			blockSprite.material = Resources.Load<Material> ("Images/GreyMat");
			break;
		case 7:
			blockSprite.material = Resources.Load<Material> ("Images/PinkMat");
			break;
		}

		int newShape = Random.Range (1, 7); 

	//	sprite.sprite = Resources.Load<Sprite> ("Images/block_sprite");

		FruitShape fruitShape =  GetComponentInChildren<FruitShape> ();
		SpriteRenderer fruitSprite = fruitShape.GetComponent<SpriteRenderer> ();

		switch (newShape) {
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
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitPineapple");
			break;
		case 7:
			fruitSprite.sprite = Resources.Load<Sprite> ("Images/fruitStrawberry");
			break;
		}
		//Helen, don't be an idiot.  Leave this here. 
		shape = newShape; 
	}

	protected override void SetDestToPos ()
	{
		base.SetDestToPos ();
		mgr.CheckForMatches ();
	}
}
