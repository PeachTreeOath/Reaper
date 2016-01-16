using UnityEngine;
using System.Collections;

public class BoardObject : MonoBehaviour
{

	public enum BoardType
	{
		BLOCK,
		PLAYER}
	;

	public BoardType boardType;
	public float speed;
	public int row;
	public int col;
	public int destRow;
	public int destCol;

	protected bool moving;
	protected float moveTime;
	protected GameManager mgr;

	void Awake ()
	{
		mgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		speed = mgr.moveSpeed;
		SetDestToPos ();
		transform.position = GetBoardPosition (row, col);
	}

	protected void Update ()
	{
		if (moving) {
			moveTime += Time.deltaTime * speed;
			transform.position = Vector2.Lerp (GetBoardPosition (row, col), GetBoardPosition (destRow, destCol), moveTime);
			if (moveTime >= 1) {
				moving = false;
				SetDestToPos ();
			}
		}
	}

	public void Move (int row, int col)
	{
		if (row == destRow && col == destCol)
			return; 
		destRow = row;
		destCol = col;
		moving = true;
		moveTime = 0;
	}

	public Vector2 GetBoardPosition (int row, int col)
	{

		float x_offset = ((mgr.boardSize + 2) / 2) * -1.5f; //int division deletes the remainder
		float y_offset = ((mgr.boardSize + 2) / 2) * 1.5f;

		float xPos = x_offset + col * 1.5f;
		float yPos = y_offset - row * 1.5f;
		return new Vector2 (xPos, yPos);
	}

	public void SetBoardPosition (int row, int col)
	{
		destRow = row;
		destCol = col;
		SetDestToPos ();
		transform.position = GetBoardPosition (row, col);
	}

	protected virtual void SetDestToPos ()
	{
		row = destRow;
		col = destCol;
	}

}
