using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

	public float moveSpeed = 3;
	public int boardSize = 7;
	// ONLY use odd numbered board sizes, game is only meant for sizes 5,7,9!
	private BoardSquare[,] board;
	private BoardSquare[,] spawningBoard;
	private float lastSpawnTime;
	private GameObject boardParent;
	private GameObject previewBlockRes;
	private GameObject spawnBlockRes;
	private GameObject matchRes;
	private GameObject playerRes;
	private PreviewBlock previewBlockL;
	private PreviewBlock previewBlockR;
	private PreviewBlock nextBlock;
	private SpawnBlock spawnBlock;
	private Text stageText;
	private Text scoreText;
	private int stage;
	private int score;
	public int difficulty;
	private float defaultSpawnSpeed = 2f;
	private float spawnSpeed;
	private GlobalObject globalObj;

	private int numBlocks = 0;
	private int maxBlocks = 0;

	// Use this for initialization
	void Awake ()
	{
		GameObject global = GameObject.Find ("GlobalObject"); 
		if (global != null) {
			globalObj = global.GetComponent<GlobalObject> ();
		}
		if (globalObj != null) {
			switch (globalObj.boardSize) {
			case 0:
				boardSize = 5;
				break;
			case 1:
				boardSize = 7;
				break;
			case 2:
				boardSize = 9;
				break;
			}
		}

		board = new BoardSquare[boardSize + 2, boardSize + 2]; // Add 2 lanes for the outside walkway
		for (int i = 0; i < boardSize + 2; i++) {
			for (int j = 0; j < boardSize + 2; j++) {
				board [i, j] = new BoardSquare ();

			}
		}
		maxBlocks = boardSize * boardSize; 
	}

	void Start ()
	{
		playerRes = Resources.Load<GameObject> ("Prefabs/Player");
		previewBlockRes = Resources.Load<GameObject> ("Prefabs/PreviewBlock");
		spawnBlockRes = Resources.Load<GameObject> ("Prefabs/SpawnBlock");
		matchRes = Resources.Load<GameObject> ("Prefabs/Match");

		stageText = GameObject.Find ("StageText").GetComponent<Text> ();
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text> ();

		CreateBG ();
		if (boardSize == 7) {
			GameObject.Find ("Main Camera").GetComponent<Camera> ().orthographicSize = 6.75f;
		} else if (boardSize == 9) {
			GameObject.Find ("Main Camera").GetComponent<Camera> ().orthographicSize = 8.25f;
		}
		boardParent = GameObject.Find ("BoardObjects");
		lastSpawnTime = Time.time;

		//for (int i = 0; i < 3; i++) {
			//SpawnBotPlayer (i+1);
		//}

		if (globalObj != null) {
			//Set players
			if (globalObj.numPlayers > 0) {
				Player player = ((GameObject)Instantiate(playerRes,Vector2.zero,Quaternion.identity)).GetComponent<Player>();
				player.numPlayer = 0;
				player.playerJoyName = globalObj.p1JoyMap;
				player.destRow = 0;
				player.destCol = 0;
			}
			if (globalObj.numPlayers > 1) {
				Player player = ((GameObject)Instantiate(playerRes,Vector2.zero,Quaternion.identity)).GetComponent<Player>();
				player.numPlayer = 1;
				player.playerJoyName = globalObj.p2JoyMap;
				player.destRow = 0;
				player.destCol = boardSize+1;
			}
			if (globalObj.numPlayers > 3) {
				Player player = ((GameObject)Instantiate(playerRes,Vector2.zero,Quaternion.identity)).GetComponent<Player>();
				player.numPlayer = 2;
				player.playerJoyName = globalObj.p3JoyMap;
				player.destRow = boardSize+1;
				player.destCol = 0;
			}
			if (globalObj.numPlayers > 4) {
				Player player = ((GameObject)Instantiate(playerRes,Vector2.zero,Quaternion.identity)).GetComponent<Player>();
				player.numPlayer = 3;
				player.playerJoyName = globalObj.p4JoyMap;
				player.destRow = boardSize+1;
				player.destCol = boardSize+1;
			}

			//Set difficulty
			if (globalObj.difficulty == 1) {
				defaultSpawnSpeed = 1.5f;
			} else if (globalObj.difficulty == 2) {
				defaultSpawnSpeed = 1f;
			}
		} else {
			Player player = ((GameObject)Instantiate(playerRes,Vector2.zero,Quaternion.identity)).GetComponent<Player>();
			player.numPlayer = 0;
			player.playerJoyName = "_p1";
			player.destRow = 0;
			player.destCol = 0;
		}
		NewStage ();
		CreateNextBlock ();
	}

	// Update is called once per frame
	void Update ()
	{
		// If block is moved over spawning block, move spawn block to different area
		if (spawnBlock != null) {
			if (GetBlockInPosition (spawnBlock.mRow, spawnBlock.mCol)) {
				RetrySpawn ();
			}
		}
	}

	private void CreateBG ()
	{
		GameObject sidePanelL = Resources.Load<GameObject> ("Prefabs/SidePanelL");
		GameObject sidePanelR = Resources.Load<GameObject> ("Prefabs/SidePanelR");
		GameObject bgPrefab = Resources.Load<GameObject> ("Prefabs/BGSquare");
		GameObject bgSidePrefab = Resources.Load<GameObject> ("Prefabs/BGSideSquare");
		GameObject grassTiles = Resources.Load<GameObject> ("Prefabs/GrassTiles");
		GameObject parent = GameObject.Find ("Background");

		if (boardSize == 5) {
			GameObject panelLObj = ((GameObject)Instantiate (sidePanelL, new Vector2 (-7.25f, 0), Quaternion.identity));
			panelLObj.transform.parent = parent.transform;
			GameObject panelRObj = ((GameObject)Instantiate (sidePanelR, new Vector2 (7.25f, 0), Quaternion.identity));
			panelRObj.transform.parent = parent.transform;
		} else if (boardSize == 7) {
			GameObject panelLObj = ((GameObject)Instantiate (sidePanelL, new Vector2 (-8.75f, 0), Quaternion.identity));
			panelLObj.transform.parent = parent.transform;
			GameObject panelRObj = ((GameObject)Instantiate (sidePanelR, new Vector2 (8.75f, 0), Quaternion.identity));
			panelRObj.transform.parent = parent.transform;
		} else if (boardSize == 9) {
			GameObject panelLObj = ((GameObject)Instantiate (sidePanelL, new Vector2 (-10.25f, 0), Quaternion.identity));
			panelLObj.transform.parent = parent.transform;
			GameObject panelRObj = ((GameObject)Instantiate (sidePanelR, new Vector2 (10.25f, 0), Quaternion.identity));
			panelRObj.transform.parent = parent.transform;
		}

		float origCurrX = ((boardSize + 2) / 2) * -1.5f;
		float currX = origCurrX;
		float currY = origCurrX;
		for (int i = 0; i < boardSize + 2; i++) {
			currY = origCurrX;
			for (int j = 0; j < boardSize + 2; j++) {
				GameObject pf;
				if (i == 0 || j == 0 || i == boardSize + 1 || j == boardSize + 1) {
					pf = bgSidePrefab;
				} else {
					pf = bgPrefab;
				}
				GameObject bg = ((GameObject)Instantiate (pf, Vector2.zero, Quaternion.identity));
				bg.transform.position = new Vector2 (currX, currY);
				bg.transform.parent = parent.transform;

				if (((i % 2) == 0) && ((j % 2) == 0)) {//every 2x2 squares, make a grass tile 

					GameObject gt = ((GameObject)Instantiate (grassTiles, Vector2.zero, Quaternion.identity));
					gt.transform.position = new Vector2 (currX, currY);
					gt.transform.parent = parent.transform; 
				}
				currY += 1.5f;
			}
			currX += 1.5f;
		}

		/*
		//create preview block squares
		GameObject pb1 = ((GameObject)Instantiate (bgPrefab, Vector2.zero, Quaternion.identity)); 
		pb1.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquare");
		pb1.transform.position = new Vector2 (origCurrX - 3F, -1F * origCurrX);
		pb1.transform.parent = parent.transform;

		GameObject pb2 = ((GameObject)Instantiate (bgPrefab, Vector2.zero, Quaternion.identity)); 
		pb2.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Images/bgSquare");
		pb2.transform.position = new Vector2 (currX + 1.5F, -1F * origCurrX);
		pb2.transform.parent = parent.transform;
		*/
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

	public bool PushBlock (int direction, int row, int col, bool executePush)
	{
		Block block = board [row, col].block;
		if (block == null) {
			return true;
		}
		if (block.moving) {
			return false;
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

		if (PushBlock (direction, destRow, destCol, executePush)) {
			if (executePush) {
				block.Move (destRow, destCol);
				board [destRow, destCol].block = block;
				board [row, col].block = null;

				if (board [row, col].player != null) {
					//there's a player on the block, move him too
					board [row, col].player.Move (destRow, destCol); 
					SetPlayerPosition (destRow, destCol, board [row, col].player); 
					VacatePlayerPosition (row, col);
				}
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
						Instantiate (matchRes, block.transform.position, Quaternion.identity);
						GameObject.Destroy (block.gameObject);
						numBlocks = numBlocks - 1;
						board [i, j].block = null; 
					}
				}
			}
		}
	}

	private void MatchH (int row, int col, int length)
	{
		bool matchColor = true;
		bool matchShape = true;
		int matchLength = 1; 

		Block firstBlock = board [row, col].block; 
		if (firstBlock == null)
			return;
		
		for (int i = 1; i < length; i++) {
			if (firstBlock.color == 1) {
				firstBlock = board [row, col + i].block;
				if (firstBlock == null)
					return;
				if (firstBlock.hMatched) {
					return;
				}
				matchLength += 1; 
			} else {
				break;
			}
		}

		for (int i = matchLength; i < length; i++) {
			if (col + i > boardSize) {
				return;
			}
			Block newBlock = board [row, col + i].block;

			if (firstBlock == null || newBlock == null || firstBlock.color == 0 || newBlock.color == 0) {
				return;
			}
			if (firstBlock.hMatched) {
				return;
			}

			if ((firstBlock.color != newBlock.color) && (newBlock.color != 1)) //not wild card
				matchColor = false; 
			if ((firstBlock.shape != newBlock.shape) && (newBlock.color != 1))
				matchShape = false;
		}

		if (matchShape && matchColor) {
			if (length == 5) {
				AddScore (20000);
			} else if (length == 4) {
				AddScore (7000);
			} else if (length == 3) {
				AddScore (2500);
			}
		} else if (matchShape || matchColor) {
			if (length == 5) {
				AddScore (1000);
			} else if (length == 4) {
				AddScore (500);
			} else if (length == 3) {
				AddScore (100);
			}
		}
		if (matchShape || matchColor) {
			if (length == 5) {
				AddScore (1000);
			} else if (length == 4) {
				AddScore (500);
			} else if (length == 3) {
				AddScore (100);
			}
			for (int i = 0; i < length; i++) {
				Block newBlock = board [row, col + i].block;
				newBlock.toDelete = true;
				newBlock.hMatched = true;
			}
		}
	}

	private void MatchV (int row, int col, int length)
	{
		bool matchColor = true;
		bool matchShape = true;
		int matchLength = 1; 

		Block firstBlock = board [row, col].block; 
		if (firstBlock == null)
			return;
			
		for (int i = 1; i < length; i++) {
			
			if (firstBlock.color == 1) {
				firstBlock = board [row + i, col].block; 
				if (firstBlock == null)
					return;
				if (firstBlock.vMatched) {
					return;
				}
				matchLength += 1; 
			} else {
				break;
			}
		}
		for (int i = matchLength; i < length; i++) {
			if (row + i > boardSize) {
				return;
			}
			Block newBlock = board [row + i, col].block;
			if (firstBlock == null || newBlock == null || firstBlock.color == 0 || newBlock.color == 0) {
				return;
			}
			if (firstBlock.vMatched) {
				return;
			}

			if ((firstBlock.color != newBlock.color) && (newBlock.color != 1)) //not wild card
				matchColor = false; 
			if ((firstBlock.shape != newBlock.shape) && (newBlock.color != 1))
				matchShape = false;
		}

		if (matchShape && matchColor) {
			if (length == 5) {
				AddScore (20000);
			} else if (length == 4) {
				AddScore (7000);
			} else if (length == 3) {
				AddScore (2500);
			}
		} else if (matchShape || matchColor) {
			if (length == 5) {
				AddScore (1000);
			} else if (length == 4) {
				AddScore (500);
			} else if (length == 3) {
				AddScore (100);
			}
		}
		if (matchShape || matchColor) {
			for (int i = 0; i < length; i++) {
				Block newBlock = board [row + i, col].block;
				newBlock.toDelete = true;
				newBlock.vMatched = true;
			}
		}

	}

	private void AddScore (int addScore)
	{
		score += addScore;
		int newLevel = score / 700;
		if (newLevel != stage) {
			stage = newLevel;
			NewStage ();
		}
		scoreText.text = "" + score;
	}

	private void NewStage ()
	{
		int speedDifficulty = stage / 10;
		spawnSpeed = defaultSpawnSpeed - ((stage % 10) * 0.1f);
		spawnSpeed -= speedDifficulty * 0.2f;
		if (speedDifficulty > 2) {
			speedDifficulty = 2;
		}
		difficulty = speedDifficulty;
		stageText.text = "" + (stage + 1);
	}

	public void SpawnBlock ()
	{
		while (numBlocks < maxBlocks) {

			if (board [spawnBlock.mRow, spawnBlock.mCol].block == null) {
				GameObject blockObj = Resources.Load<GameObject> ("Prefabs/Block");
				Block block = ((GameObject)Instantiate (blockObj, Vector2.zero, Quaternion.identity)).GetComponent<Block> ();
				block.transform.parent = boardParent.transform;
				block.SetBoardPosition (spawnBlock.mRow, spawnBlock.mCol);
				block.SetBlockProperties (spawnBlock.color, spawnBlock.shape);
				PlaceBlock (block, spawnBlock.mRow, spawnBlock.mCol);
				numBlocks++;
				CheckForMatches ();
				Destroy (spawnBlock.gameObject);
				CreateNextBlock ();
				break;
			}
		}
		if (numBlocks >= maxBlocks) {
			Application.LoadLevel (3);
		}
	}

	private void SpawnSpawner ()
	{
		while (numBlocks < maxBlocks) {
			int row = Random.Range (1, boardSize + 1);
			int col = Random.Range (1, boardSize + 1);

			if (board [row, col].block == null) {
				spawnBlock = ((GameObject)Instantiate (spawnBlockRes, Vector2.zero, Quaternion.identity)).GetComponent<SpawnBlock> ();
				spawnBlock.SetBlockProperties (nextBlock.color, nextBlock.shape);
				spawnBlock.SetBoardPosition (row, col);
				break;
			}
		}
	}

	private void RetrySpawn ()
	{
		while (numBlocks < maxBlocks) {
			int row = Random.Range (1, boardSize + 1);
			int col = Random.Range (1, boardSize + 1);

			if (board [row, col].block == null) {
				spawnBlock.SetBoardPosition (row, col);
				spawnBlock.Retry ();
				break;
			}
		}
	}

	private Vector2 previewPosBlock5L = new Vector2 (-6.655f, -0.35f);
	private Vector2 previewPosBlock5R = new Vector2 (6.645f, -0.35f);
	private Vector2 previewPosBlock7L = new Vector2 (-8.165f, -0.35f);
	private Vector2 previewPosBlock7R = new Vector2 (8.145f, -0.35f);
	private Vector2 previewPosBlock9L = new Vector2 (-9.65f, -0.35f);
	private Vector2 previewPosBlock9R = new Vector2 (9.65f, -0.35f);

	public void CreateNextBlock ()
	{
		if (boardSize == 5) {
			previewBlockL = ((GameObject)Instantiate (previewBlockRes, previewPosBlock5L, Quaternion.identity)).GetComponent<PreviewBlock> ();
			previewBlockR = ((GameObject)Instantiate (previewBlockRes, previewPosBlock5R, Quaternion.identity)).GetComponent<PreviewBlock> ();
		} else if (boardSize == 7) {
			previewBlockL = ((GameObject)Instantiate (previewBlockRes, previewPosBlock7L, Quaternion.identity)).GetComponent<PreviewBlock> ();
			previewBlockR = ((GameObject)Instantiate (previewBlockRes, previewPosBlock7R, Quaternion.identity)).GetComponent<PreviewBlock> ();
		} else if (boardSize == 9) {
			previewBlockL = ((GameObject)Instantiate (previewBlockRes, previewPosBlock9L, Quaternion.identity)).GetComponent<PreviewBlock> ();
			previewBlockR = ((GameObject)Instantiate (previewBlockRes, previewPosBlock9R, Quaternion.identity)).GetComponent<PreviewBlock> ();
		}

		int newColor = Random.Range (2, 8 + difficulty);
		int wildCardRoll = Random.Range (0, 100);
		if (wildCardRoll < 4) {
			newColor = 1;
		}
		int newShape = Random.Range (1, 7 + difficulty); 
		previewBlockL.SetBlockProperties (newColor, newShape);
		previewBlockR.SetBlockProperties (newColor, newShape);

		nextBlock = previewBlockL;
		nextBlock.SetExpire (spawnSpeed);
	}

	public void ExpirePreview ()
	{
		SpawnSpawner ();
		Destroy (previewBlockL.gameObject);
		Destroy (previewBlockR.gameObject);
	}

	private void SpawnBotPlayer (int numPlayer)
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
				player.numPlayer = numPlayer;
				break;
			}
		}
	}

	public void PlaceBlock (Block block, int row, int col)
	{
		board [row, col].block = block;
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

	public Block GetBlockInPosition (int row, int col)
	{
		return board [row, col].block; 
	}
}
				