using UnityEngine;
using System.Collections;

public class Walls : MonoBehaviour {

	[HideInInspector]
	public Vector2 posOffset = new Vector2(20f, 10f);

	float wallWidth = 0.5f;
	GameObject[] WallEdges = new GameObject[4];
	Vector2[] posMults = new Vector2[4]{new Vector2(-1f,1f), new Vector2(1f,1f), new Vector2(1f,-1f), new Vector2(-1f,-1f)};
	Vector2[] colSizeMults = new Vector2[4]{new Vector2(0f,2f), new Vector2(2f,0f), new Vector2(0f,2f), new Vector2(2f,0f)};
	Vector2[] colOffset = new Vector2[4]{new Vector2(0f,-1f), new Vector2(-1f,0f), new Vector2(0f,1f), new Vector2(1f,0f)};


	public PhysicsMaterial2D wallPhyMat;
	public Material wallMat;
	Color wallColor;
	// Use this for initialization

	public void CreateWalls(Vector2 pOff, Color wColor){

		posOffset = pOff;
		wallColor = wColor;

		for (int i = 0; i < 4; i++) {

			GameObject wEdge = new GameObject ();
			wEdge.transform.position = GetVectorMult(posOffset, posMults[i]);
			wEdge.tag = "charDestoyer";

			LineRenderer lr = wEdge.AddComponent <LineRenderer>() as LineRenderer;
			BoxCollider2D bc = wEdge.AddComponent <BoxCollider2D>() as BoxCollider2D;

			if (i > 0)
				SetComponentParameters (lr, bc, i);
				
			WallEdges [i] = wEdge;
		}

		SetComponentParameters (WallEdges[0].GetComponent<LineRenderer>(), WallEdges[0].GetComponent<BoxCollider2D>(), 0);
	}
	
	void SetComponentParameters(LineRenderer lr, BoxCollider2D bc, int idx){

		lr.SetPosition (0, GetVectorMult(posOffset, posMults[idx]));
		int adx = idx != 0 ? idx - 1 : 3;

		lr.SetPosition (1, WallEdges[adx].transform.position);
		lr.SetWidth (wallWidth, wallWidth);
		lr.material = wallMat;
		lr.receiveShadows = false;
		lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		lr.useLightProbes = false;
		lr.SetColors (wallColor, wallColor);

		bc.size = FixWallWidth(GetVectorMult(posOffset,colSizeMults [idx]));
		bc.offset = GetVectorMult(posOffset, colOffset [idx]);
		bc.sharedMaterial = wallPhyMat;
	}

	private static Vector2 GetVectorMult(Vector2 v1, Vector2 v2){
		
		return new Vector2 (v1.x * v2.x, v1.y * v2.y);
	}

	private Vector2 FixWallWidth(Vector2 tW){

		Vector2 fW = tW;

		if (fW.x == 0)
			fW = new Vector2 (wallWidth, fW.y);
		else if (fW.y == 0)
			fW = new Vector2 (fW.x, wallWidth);
		return fW;
	}
}
