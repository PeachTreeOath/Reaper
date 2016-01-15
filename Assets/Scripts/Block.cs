using UnityEngine;
using System.Collections;

public class Block : BoardObject
{

	public int color;
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
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		//int newColor = Random.Range (0, 6);
		int newColor = Random.Range (0, 4);
		switch (newColor) {
		case 0:
			sprite.material = Resources.Load<Material> ("Images/RedMat");
			break;
		case 1:
			sprite.material = Resources.Load<Material> ("Images/BlueMat");
			break;
		case 2:
			sprite.material = Resources.Load<Material> ("Images/GreenMat");
			break;
		case 3:
			sprite.material = Resources.Load<Material> ("Images/YellowMat");
			break;
		/*case 4:
			sprite.material = Resources.Load<Material> ("Images/GreyMat");
			break;
		case 5:
			sprite.material = Resources.Load<Material> ("Images/PinkMat");
			break;*/
		}
		color = newColor;
	}

	protected override void SetDestToPos ()
	{
		base.SetDestToPos ();
		mgr.CheckForMatches ();
	}
}
