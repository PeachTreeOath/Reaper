using UnityEngine;
using System.Collections;

public class Block : BoardObject
{

	// Use this for initialization
	void Start ()
	{
		boardType = BoardType.BLOCK;
		mgr.PlaceBlock (this, destRow, destCol);
	}
	
	// Update is called once per frame
	void Update ()
	{
		base.Update ();
	}

}
