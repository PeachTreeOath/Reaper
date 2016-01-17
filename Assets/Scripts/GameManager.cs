using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public float spawnSpeed = 3;
	public float moveSpeed = 3;
	public int boardSize = 7;
	// ONLY use odd numbered board sizes, game is only meant for sizes 5,7,9!
	private BoardSquare[,] board;
	private float lastSpawnTime;
	private GameObject boardParent;

	// Use this for initialization
	void Awake ()
	{
		board = new BoardSquare[boardSize + 2, boardSize + 2]; // Add 2 lanes for the outside walkway
		for (int i = 0; i < boardSize + 2; i++) {
			for (int j = 0; j < boardSize + 2; j++) {
				board [i, j] = new BoardSquare ();
			}
		}
	}

	void Start ()
	{
		CreateBG ();
		if (boardSize > 5) {
			GameObject.Find ("Main Camera").GetComponent<Camera>().orthographicSize += boardSize - 5;
		}
		boardParent = GameObject.Find ("BoardObjects");
		lastSpawnTime = Time.time;
		//TODO delete
		for (int i = 0; i < boardSize * 2; i++) {
			SpawnBlock ();
		}
		for (int i = 0; i < 3; i++) {
			SpawnBotPlayer ();

		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (lastSpawnTime + spawnSpeed < Time.time) {
			//	SpawnBlock ();
			lastSpawnTime = Time.time;
		}
	}

	private void CreateBG ()
	{
		GameObject bgPrefab = Resources.Load<GameObject> ("Prefabs/BGSquare");
		GameObject parent = GameObject.Find ("Background");
		float origCurrX = ((boardSize + 2) / 2) * -1.5f;
		float currX = origCurrX;
		float currY = origCurrX;
		for (int i = 0; i < boardSize + 2; i++) {
			currY = origCurrX;
			for (int j = 0; j < boardSize + 2; j++) {
				GameObject bg = ((GameObject)Instantiate (bgPrefab, Vector2.zero, Quaternion.identity));
				bg.transform.position = new Vector2 (currX, currY);
				bg.transform.parent = parent.transform;
				currY += 1.5f;
			}
			currX += 1.5f;
		}
	}

	public bool CheckForBlock (int row, int col) //TODO is this necessary?
	{
		if (board [row, col].block != null) {
			return true;
		}

		return false;
	}

	//row and col are starting position, direction is the direction of movement
	public bool PullBlock (int direction, int destRow, int destCol)
	{
		int startRow = destRow; 
		int startCol = destCol; 

		//1234 is LRUD
		if (direction == 1) {
			startCol = destCol + 1;	
		} else if (direction == 2) {
			startCol = destCol - 1;	
		} else if (direction == 3) {
			startRow = destRow - 1;
		} else if (direction == 4) {
			startRow = destRow + 1;
		} else
			return true; // direction keys aren't getting held down. 

		if (startRow == 0 || destRow >= (boardSize + 1) || destCol == 0 || destCol >= (boardSize + 1)) {
			return false;
		}

		Block block = board [startRow, startCol].block; 
		if (block == null)
			return true;
		else if (board [destRow, destCol].player != null)
			return false; //player already there
		else if (board [destRow, destCol].block != null)
			return false; //block already there
		
		block.Move (destRow, destCol);
		board [destRow, destCol].block = block;
		board [startRow, startCol].block = null;

		if (board [startRow, startCol].player != null) {//player on top of block, move player too
			board [startRow, startCol].player.Move (destRow, destCol); 
			SetPlayerPosition (destRow, destCol, board [startRow, startCol].player); 
			VacatePlayerPosition (startRow, startCol);
		}
		return true;
	}

	public bool PushBlock (int direction, int row, int col)
	{
		Block block = board [row, col].block;
		if (block == null) {
			return true;
		}

		int destRow = row;
		int destCol = col;

		if (direction == 1) {
			destCol = col - 1;	
		} else if (direction == 2) {
			destCol = col + 1;	
		} else if (direction == 3) {
			destRow = row + 1;
		} else if (direction == 4) {
			destRow = row - 1;
		}
		if (destRow == 0 || destRow >= (boardSize + 1) || destCol == 0 || destCol >= (boardSize + 1)) {
			return false;
		}
		if (PushBlock (direction, destRow, destCol)) {
			block.Move (destRow, destCol);
			board [destRow, destCol].block = block;
			board [row, col].block = null;

			if (board [row, col].player != null) {
				//there's a player on the block, move him too
				board[row,col].player.Move (destRow, destCol); 
				SetPlayerPosition (destRow, destCol, board[row,col].player); 
				VacatePlayerPosition (row, col);
			}

			return true;
		}
		return false;
	}

	public void CheckForMatches ()
	{
		if (board == null) { // Use to stop early polling before awakes are done
			return;
		}

		for (int i = 1; i < boardSize + 1; i++) {
			for (int j = 1; j < boardSize + 1; j++) {	
				MatchH (i, j, 5);
				MatchV (i, j, 5);
				MatchH (i, j, 4);
				MatchV (i, j, 4);
				MatchH (i, j, 3);
				MatchV (i, j, 3);
			}
		}

		DeleteMatches ();
	}

	private void DeleteMatches ()
	{
		for (int i = 1; i < boardSize + 1; i++) {
			for (int j = 1; j < boardSize + 1; j++) {	
				Block block = board [i, j].block;
				if (block != null) {
					if (block.toDelete) {
						GameObject.Destroy (block.gameObject);
					}
				}
			}
		}
	}

	private void MatchH (int row, int col, int length)
	{
		for (int i = 1; i < length; i++) {
			if (col + i > boardSize) {
				return;
			}
			Block newBlock = board [row, col + i].block;
			if (board [row, col].block == null || newBlock == null) {
				return;
			}
			if ((board [row, col].block.color != newBlock.color) && (board[row,col].block.shape != newBlock.shape)) {
				return;
			}
		}

		for (int i = 0; i < length; i++) {
			Block newBlock = board [row, col + i].block;
			newBlock.toDelete = true;
		}
	}

	private void MatchV (int row, int col, int length)
	{
		for (int i = 1; i < length; i++) {
			if (row + i > boardSize) {
				return;
			}
			Block newBlock = board [row + i, col].block;
			if (board [row, col].block == null || newBlock == null) {
				return;
			}
			if ((board [row, col].block.color != newBlock.color) && (board[row,col].block.shape != newBlock.shape)) {
				return;
			}
		}

		for (int i = 0; i < length; i++) {
			Block newBlock = board [row + i, col].block;
			newBlock.toDelete = true;
		}
	}

	private void SpawnBlock ()
	{
		while (true) { //TODO better exit stmt
			int row = Random.Range (1, boardSize);
			int col = Random.Range (1, boardSize);

			if (board [row, col].block == null) {
				GameObject blockObj = Resources.Load<GameObject> ("Prefabs/Block");
				Block block = ((GameObject)Instantiate (blockObj, Vector2.zero, Quaternion.identity)).GetComponent<Block> ();
				block.transform.parent = boardParent.transform;
				block.SetBoardPosition (row, col);
				PlaceBlock (block, row, col);
				break;
			}
		}
	}

	private void SpawnBotPlayer ()
	{
		while (true) {
			int row = Random.Range (0, boardSize + 1);
			int col = Random.Range (0, boardSize + 1);
			if (board [row, col].player == null) {
				GameObject playerObj = Resources.Load<GameObject> ("Prefabs/Player"); 
				Player player = ((GameObject)Instantiate (playerObj, Vector2.zero, Quaternion.identity)).GetComponent<Player> ();
				player.transform.parent = boardParent.transform;
				player.SetBoardPosition (row, col); 	
				board [row, col].player = player;
				player.isBot = true; 
				break;
			}
		}
	}

	public void PlaceBlock (Block block, int row, int col)
	{
		board [row, col].block = block; //Hi, it's me!
	}

	public void SetPlayerPosition (int row, int col, Player player)
	{
		board [row, col].player = player; 
	}

	public void VacatePlayerPosition (int row, int col)
	{
		board [row, col].player = null; 
	}

	public Player GetPlayerInPosition (int row, int col)
	{
		return board [row, col].player; 
	}
}
				