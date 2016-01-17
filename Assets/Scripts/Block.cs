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
		mgr.PlaceBlock (this, destRow, destCol);
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update ();
	}

	private void SetBlockProperties ()
	{
		SpriteRenderer blockSprite = GetComponent<SpriteRenderer> ();
		//int newColor = Random.Range (0, 6);
		int newColor = Random.Range (0, 4);
		int newShape = Random.Range (0, 6); 

	//	sprite.sprite = Resources.Load<Sprite> ("Images/block_sprite");


		SpriteRenderer fruitShape =  GetComponentInChildren<SpriteRenderer> ();

		switch (newShape) {
		case 0:
			fruitShape.sprite = Resources.Load<Sprite> ("Images/fruitApple");
			break;
		case 1:
			fruitShape.sprite = Resources.Load<Sprite> ("Images/fruitBanana");
			break;
		case 2:
			fruitShape.sprite = Resources.Load<Sprite> ("Images/fruitCherry");
			break;
		case 3:
			fruitShape.sprite = Resources.Load<Sprite> ("Images/fruitGrapes");
			break;
		case 4:
			fruitShape.sprite = Resources.Load<Sprite> ("Images/fruitLemon");
			break;
		case 5:
			fruitShape.sprite = Resources.Load<Sprite> ("Images/fruitPineapple");
			break;
		case 6:
			fruitShape.sprite = Resources.Load<Sprite> ("Images/fruitStrawberry");
			break;
		}
		//Helen, don't be an idiot.  Leave this here. 
		shape = newShape; 
		switch (newColor) {

		case 0:
			blockSprite.material = Resources.Load<Material> ("Images/RedMat");
			break;
		case 1:
			blockSprite.material = Resources.Load<Material> ("Images/BlueMat");
			break;
		case 2:
			blockSprite.material = Resources.Load<Material> ("Images/GreenMat");
			break;
		case 3:
			blockSprite.material = Resources.Load<Material> ("Images/YellowMat");
			break;
		case 4:
			blockSprite.material = Resources.Load<Material> ("Images/GreyMat");
			break;
		case 5:
			blockSprite.material = Resources.Load<Material> ("Images/PinkMat");
			break;
		}
		color = newColor;
	}

	protected override void SetDestToPos ()
	{
		base.SetDestToPos ();
		mgr.CheckForMatches ();
	}
}
