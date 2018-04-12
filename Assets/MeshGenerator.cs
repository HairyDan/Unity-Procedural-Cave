using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour {

	public SquareGrid squareGrid;

	public void GenerateMesh(bool[,] tileMap, float squareSize){
		squareGrid = new SquareGrid (tileMap, squareSize);
	}

	void OnDrawGizmos(){
		if (squareGrid != null) {
			for (int x = 0; x< squareGrid.squares.GetLength(0); x++){
				for (int y = 0; y< squareGrid.squares.GetLength(1); y++){
					Gizmos.color = (squareGrid.squares [x, y].topLeft.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].topLeft.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].topRight.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].topRight.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].bottomRight.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].bottomRight.position, Vector3.one * .4f);

					Gizmos.color = (squareGrid.squares [x, y].bottomLeft.active) ? Color.black : Color.white;
					Gizmos.DrawCube (squareGrid.squares [x, y].bottomLeft.position, Vector3.one * .4f);

					Gizmos.color = Color.grey;
					Gizmos.DrawCube (squareGrid.squares [x, y].centreTop.position, Vector3.one * .2f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centreRight.position, Vector3.one * .2f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centreBottom.position, Vector3.one * .2f);
					Gizmos.DrawCube (squareGrid.squares [x, y].centreLeft.position, Vector3.one * .2f);
				}
			}

		}
	}

	public class SquareGrid {
		public Square[,] squares;

		public SquareGrid(bool[,] tileMap, float squareSize){
			int nodeCountX = tileMap.GetLength(0);
			int nodeCountY = tileMap.GetLength(1);
			Debug.Log("ncx: " + nodeCountX + "ncy: " + nodeCountY);
			float mapWidth = nodeCountX* squareSize;
			float mapHeight = nodeCountY * squareSize;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX,nodeCountY];

			for (int x = 0; x< nodeCountX; x++){
				for (int y = 0; y< nodeCountY; y++){
					Vector3 pos = new Vector3(-mapWidth/2 + x*squareSize + squareSize/2, 0, -mapHeight/2 + y *squareSize + squareSize/2);
					controlNodes[x,y] = new ControlNode(pos, tileMap[x,y], squareSize);
				}
			}

			squares = new Square[nodeCountX-1,nodeCountY-1];
			for (int x = 0; x< nodeCountX-1; x++){
				for (int y = 0; y< nodeCountY-1; y++){
					squares[x,y] = new Square(controlNodes[x,y+1], controlNodes[x+1,y+1], controlNodes[x+1,y], controlNodes[x,y]);
				}
			}
		}
	}

	public class Square{
		public ControlNode topLeft, topRight, bottomRight, bottomLeft;
		public Node centreTop, centreRight, centreBottom, centreLeft;

		public Square (ControlNode tl, ControlNode tr, ControlNode br, ControlNode bl){
			topLeft = tl;
			topRight = tr;
			bottomRight = br;
			bottomLeft = bl;

			centreTop = topLeft.right;
			centreRight = bottomRight.above;
			centreBottom = bottomLeft.right;
			centreLeft = bottomLeft.above;
		}
	}

	public class Node {
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 pos){
			position = pos;
		}
	}

	public class ControlNode : Node {
		public bool active;

		public Node above, right;

		public ControlNode(Vector3 pos, bool isActive, float squareSize) : base (pos){
			active = isActive;
			above = new Node(position + Vector3.forward * squareSize/2f);
			right = new Node(position + Vector3.right * squareSize/2f);

		}
	}
}
