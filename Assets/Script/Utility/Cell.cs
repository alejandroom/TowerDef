using UnityEngine;
using System.Collections;

public class Cell {
	
	public enum Types {Road, Respawn, Center, Tower};

	public bool visited;

	public bool up;
	public bool down;
	public bool left;
	public bool right;

	public int road;

	public Types type;
	public Generator.Directions[] dir;
	public int[] paths;

	public Cell(){
		visited = false;
		up = false;
		down = false;
		left = false;
		right = false;
		road = 0;
		paths = new int[100];
		dir = new Generator.Directions[100];
	}
}
