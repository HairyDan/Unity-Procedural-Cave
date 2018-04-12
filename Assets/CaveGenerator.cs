using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveGenerator : MonoBehaviour {

	bool[,] tileMap;
	public int height, width, fillPercent;

	void Start () {
		GenerateTileMap ();
	}

	void GenerateTileMap(){
		tileMap = new bool[width, height];

		System.Random rng = new System.Random ();

		//randomly populate the tile map
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				int tileResult = rng.Next(0,100);
				if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
					tileMap [x, y] = true;
				} else if (tileResult >= fillPercent) {
					tileMap [x, y] = false;
				} else {
					tileMap [x, y] = true;
				}
			}
		}

		//smooth the map using CA-style smoothing multiple times
		for (int i = 0; i < 3; i++) {
			SmoothMap ();
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (tileMap, 1);
	}

	void SmoothMap(){
		for (int x = 1; x < width -1; x++) {
			for (int y = 1; y < height -1; y++) {
				int numberOfSurroundingWalls = GetSurroundingWallCount (x, y);
				if (numberOfSurroundingWalls > 4) {
					tileMap [x, y] = true;
				} else if (numberOfSurroundingWalls < 4) {
					tileMap [x, y] = false;
				}
			}
		}
	}

	int GetSurroundingWallCount(int xLocation, int yLocation){
		int numberOfSurroundingWalls = 0;
		for (int i = xLocation - 1; i <= xLocation + 1; i++) {
			for (int j = yLocation - 1; j <= yLocation + 1; j++) {
				if (i >= 0 && i < width && j >= 0 && j < height) {
					if (i != xLocation || j != yLocation) {
						if (tileMap [i, j]) {
							numberOfSurroundingWalls++;
						}
					}
				}
			}
		}
		return numberOfSurroundingWalls;
	}

	void OnDrawGizmos(){
		/*if (tileMap != null) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (tileMap [x, y]) {
						Gizmos.color = Color.black;
					} else {
						Gizmos.color = Color.white;
					}
					Vector3 tilePos = new Vector3 (-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
					Gizmos.DrawCube (tilePos, Vector3.one);
				}
			}
		}*/
	}
		
}
