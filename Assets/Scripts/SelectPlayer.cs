using UnityEngine;
using System.Collections;

public class SelectPlayer : MonoBehaviour {

	public enum SwapIndex
	{
		Outline = 39,
		LightSkin = 66,
		LightSkinOff = 46,
		// 1 pixel mistake
		DarkSkin = 51,
		BeakLight = 101,
		BeakDark = 89,
		Rune = 161,
	}

	public int numPlayer;

	private Texture2D mColorSwapTex;
	private Color[] mSpriteColors;

	public void InitColorSwapTex ()
	{
		Texture2D colorSwapTex = new Texture2D (256, 1, TextureFormat.RGBA32, false, false);
		colorSwapTex.filterMode = FilterMode.Point;
		for (int i = 0; i < colorSwapTex.width; i++) {
			colorSwapTex.SetPixel (i, 0, new Color (0f, 0f, 0f, 0f));
		}
		colorSwapTex.Apply ();
		GetComponent<SpriteRenderer> ().material.SetTexture ("_SwapTex", colorSwapTex);
		mSpriteColors = new Color[colorSwapTex.width];
		mColorSwapTex = colorSwapTex;
	}

	public void SwapColor (SwapIndex index, Color32 color32)
	{
		Color color = (Color)color32;
		mSpriteColors [(int)index] = color;
		mColorSwapTex.SetPixel ((int)index, 0, color);

		//For fixing mistakes with the sprite
		if (index == SwapIndex.LightSkin) {
			mSpriteColors [(int)SwapIndex.DarkSkin] = color;
			mColorSwapTex.SetPixel ((int)SwapIndex.DarkSkin, 0, color);
			mSpriteColors [(int)SwapIndex.LightSkinOff] = color;
			mColorSwapTex.SetPixel ((int)SwapIndex.LightSkinOff, 0, color);
		}
	}

	public void ChangeColor (int playerNum)
	{
		switch (playerNum) {
		case 0:
			SwapColor (SwapIndex.LightSkin, new Color32 (66, 33, 0, 255));
			break;
		case 1:
			SwapColor (SwapIndex.Outline, new Color32 (0, 12, 40, 255));
			SwapColor (SwapIndex.LightSkin, new Color32 (0, 58, 180, 255));
			SwapColor (SwapIndex.Rune, new Color32 (0, 100, 64, 255));
			break;
		case 2:
			SwapColor (SwapIndex.Outline, new Color32 (38, 0, 0, 255));
			SwapColor (SwapIndex.LightSkin, new Color32 (148, 0, 11, 255));
			SwapColor (SwapIndex.Rune, new Color32 (175, 0, 161, 255));
			break;
		case 3:
			SwapColor (SwapIndex.Outline, new Color32 (38, 38, 0, 255));
			SwapColor (SwapIndex.LightSkin, new Color32 (160, 160, 0, 255));
			SwapColor (SwapIndex.Rune, new Color32 (18, 0, 159, 255));
			break;
		}
		mColorSwapTex.Apply ();
	}

	// Use this for initialization
	void Start () {
		InitColorSwapTex ();
		ChangeColor (numPlayer);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
