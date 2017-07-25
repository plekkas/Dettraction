using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelSetting{

	public Vector2 levelSize = new Vector2(20f, 10f);
	public int startAttrObjs = 20;
	public int[] numOfEnemies = new int[5] {3,2,1,0,0};
	public Vector2[] enemyPositions = new Vector2[6] {new Vector2(-13f,-8f), new Vector2(12f,7f), new Vector2(10f,-5f), new Vector2(-8f,-4f), new Vector2(8f,2f), new Vector2(-7f,1.7f)};
	public int[] numOfObstacles = new int[4] { 1, 1, 0, 0 };
	public Vector2[] obstaclePositions = new Vector2[2] {new Vector2(-10f,-3f), new Vector2(5f,5f)};

}

[System.Serializable]
public class ColorPalettes{

	public Color[] levelColors = new Color[5];//0: bg, 1: walls, 2: player, 3: enemies 4: attracts
}
