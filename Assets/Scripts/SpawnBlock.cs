using UnityEngine;
using System.Collections;

public class SpawnBlock : MonoBehaviour
{

	public int color;
	public int shape;
	public int mRow;
	public int mCol;

	private float expireTime;
	private float currTime;
	private GameManager mgr;
	private Warp warp;
	private GameObject warpObj;

	// Use this for initialization
	void Start ()
	{
		warpObj = Resources.Load<GameObject> ("Prefabs/Warp");
		warp = ((GameObject)Instantiate (warpObj, transform.position, Quaternion.identity)).GetComponent<Warp> ();
		warp.transform.parent = transform;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void Retry()
	{
		Destroy (warp.gameObject);
		warp = ((GameObject)Instantiate (warpObj, transform.position, Quaternion.identity)).GetComponent<Warp> ();
		warp.transform.parent = transform;
	}

	public void Delete ()
	{
		mgr.SpawnBlock ();
	}

	public Vector2 GetBoardPosition (int r, int c)
	{
		float x_offset = ((mgr.boardSize + 2) / 2) * -1.5f; //int division deletes the remainder
		float y_offset = ((mgr.boardSize + 2) / 2) * 1.5f;

		float xPos = x_offset + c * 1.5f;
		float yPos = y_offset - r * 1.5f;
		return new Vector2 (xPos, yPos);
	}

	public void SetBoardPosition (int inRow, int inCol)
	{
		mgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		mRow = inRow;
		mCol = inCol;
		transform.position = GetBoardPosition (inRow, inCol);
	}

	// Keep in sync with Block.cs
	public void SetBlockProperties (int inColor, int inShape)
	{
		SpriteRenderer blockSprite = GetComponent<SpriteRenderer> ();
		if (inColor == 0) {
			int newColor = Random.Range (2, 8 + mgr.difficulty);
			int wildCardRoll = Random.Range (0, 100);
			if (wildCardRoll < 4) {
				newColor = 1;
			}
			color = newColor;
		} else {
			color = inColor;
		}
		if (inShape == 0) {
			int newShape = Random.Range (1, 7 + mgr.difficulty); 
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
