using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Generator {

	public enum Directions {Up, Down, Left, Right};

	public Cell[,] maze;
	public float RandomTowers=0.3f;
	private Pos2 startingCell;
	private int Height;
	private int Width;

	private bool startingSet = false;

	public void setStartingCell(Pos2 pos){
		startingCell = pos;
		startingSet = true;
	}

	public void newMaze(int height, int width){
		maze = new Cell[height, width];
		Height = height;
		Width = width;
		initiate ();
	}

	public void generateMaze(){
		ArrayList walls = new ArrayList ();
		reset ();

		if(!startingSet)
			startingCell = selectStartingCell (Height, Width);
		maze [startingCell.x, startingCell.y].visited = true;
		maze [startingCell.x, startingCell.y].type = Cell.Types.Center;

		walls = addWalls (startingCell.x, startingCell.y, walls);

		while (walls.Count != 0) {
			Pos3 wall = (Pos3)walls[Mathf.FloorToInt(Random.Range(0,walls.Count))];
			if((wall.z == (int)Directions.Up && wall.x == 0) ||
			   (wall.z == (int)Directions.Down && wall.x == Height-1) ||
			   (wall.z == (int)Directions.Left && wall.y == 0) ||
			   (wall.z == (int)Directions.Right && wall.y == Width-1) ||
			   (wall.z == (int)Directions.Right && maze[wall.x, wall.y+1].visited) ||
			   (wall.z == (int)Directions.Left && maze[wall.x, wall.y-1].visited) ||
			   (wall.z == (int)Directions.Up && maze[wall.x-1, wall.y].visited) ||
			   (wall.z == (int)Directions.Down && maze[wall.x+1, wall.y].visited))
				walls.Remove(wall);
			else{
				if(wall.z == (int)Directions.Right){
					maze[wall.x, wall.y+1].visited = true;
					maze[wall.x, wall.y+1].left = true;
					maze[wall.x, wall.y].right = true;
					walls = addWalls (wall.x, wall.y+1, walls);
				}else if(wall.z == (int)Directions.Left){
					maze[wall.x, wall.y-1].visited = true;
					maze[wall.x, wall.y-1].right = true;
					maze[wall.x, wall.y].left = true;
					walls = addWalls (wall.x, wall.y-1, walls);
				}else if(wall.z == (int)Directions.Up){
					maze[wall.x-1, wall.y].visited = true;
					maze[wall.x-1, wall.y].down = true;
					maze[wall.x, wall.y].up = true;
					walls = addWalls (wall.x-1, wall.y, walls);
				}else if(wall.z == (int)Directions.Down){
					maze[wall.x+1, wall.y].visited = true;
					maze[wall.x+1, wall.y].up = true;
					maze[wall.x, wall.y].down = true;
					walls = addWalls (wall.x+1, wall.y, walls);
				}
				walls.Remove(wall);
			}
		}
	}
	
	void cleanVisited(){
		for (int i=0; i<Height; i++)
			for (int j=0; j<Width; j++)
				maze [i, j].visited = false;
	}
	
	void initiate(){
		for (int i=0; i<Height; i++)
			for (int j=0; j<Width; j++)
				maze [i, j] = new Cell();
	}
	
	void reset(){
		for (int i=0; i<Height; i++) {
			for (int j=0; j<Width; j++){
				maze [i, j].up = false;
				maze [i, j].down = false;
				maze [i, j].right = false;
				maze [i, j].left = false;
			}
		}
	}

	public void traverse(int height, int width, Directions dir, int numPath){
		cleanVisited ();

		Stack S = new Stack ();
		Dictionary<string, Pos3> parents = new Dictionary<string, Pos3> ();
		S.Push (new Pos3 (height, width, (int)dir));
		while (S.Count != 0) {
			Pos3 v = (Pos3)S.Pop();
			if(!maze[v.x, v.y].visited){
				if(v.x == startingCell.x && v.y == startingCell.y){
					break;
				}
				maze[v.x, v.y].visited = true;
				if(maze[v.x, v.y].up && v.z != (int)Directions.Up){
					S.Push(new Pos3 (v.x-1, v.y, (int)Directions.Down));
					parents[(v.x-1)+":"+v.y] = new Pos3 (v.x, v.y, (int)Directions.Up);
				}
				if(maze[v.x, v.y].down && v.z != (int)Directions.Down){
					S.Push(new Pos3 (v.x+1, v.y, (int)Directions.Up));
					parents[(v.x+1)+":"+v.y] = new Pos3 (v.x, v.y, (int)Directions.Down);
				}
				if(maze[v.x, v.y].left && v.z != (int)Directions.Left){
					S.Push(new Pos3 (v.x, v.y-1, (int)Directions.Right));
					parents[v.x+":"+(v.y-1)] = new Pos3 (v.x, v.y, (int)Directions.Left);
				}
				if(maze[v.x, v.y].right && v.z != (int)Directions.Right){
					S.Push(new Pos3 (v.x, v.y+1, (int)Directions.Left));
					parents[v.x+":"+(v.y+1)] = new Pos3 (v.x, v.y, (int)Directions.Right);
				}
			}
		}

		Pos3 curr = new Pos3(startingCell.x, startingCell.y, 0);
		while (curr != null) {
			maze[curr.x, curr.y].road++;
			if(!parents.ContainsKey(curr.x+":"+curr.y))
				break;
			curr=parents[curr.x+":"+curr.y];
			maze[curr.x, curr.y].type=Cell.Types.Road;
			Debug.Log("D:"+maze[curr.x, curr.y].road + ":x:"+curr.x+":y:"+curr.y);
			maze[curr.x, curr.y].dir[maze[curr.x, curr.y].road]=(Directions)curr.z;
			maze[curr.x, curr.y].paths[maze[curr.x, curr.y].road] = numPath;
		}

		maze[height, width].type = Cell.Types.Respawn;
	}

	public void generateTowers(){
		for (int i=1; i<Height-1; i++) {
			for (int j=1; j<Width-1; j++) {
				if(maze[i,j].road == 0 && (maze[i-1,j].road != 0 || maze[i,j-1].road != 0 || maze[i+1,j].road != 0 || maze[i,j+1].road != 0 || maze[i-1,j-1].road != 0 ||
				                            maze[i+1,j+1].road != 0 || maze[i+1,j-1].road != 0 || maze[i-1,j+1].road != 0) && Random.value<RandomTowers)
					maze[i,j].type=Cell.Types.Tower;
			}
		}
	}
	
	public string print(){
		string retorno = "";
		for (int i=0; i<Height; i++) {
			for (int j=0; j<Width; j++) {
				if(maze[i,j].road == 0)
					retorno=retorno+"x";
				else
					retorno=retorno+maze[i,j].dir;
			}
			retorno=retorno+"\n";
		}
		return retorno;
	}
	
	public string printMaze(){
		string retorno = "";
		for (int j=0; j<Width; j++) {
			for (int i=0; i<Height; i++) {
				retorno=retorno+maze[i,j].up+":"+maze[i,j].down+":"+maze[i,j].right+":"+maze[i,j].left+"   ";
			}
			retorno=retorno+"\n";
		}
		return retorno;
	}
	
	ArrayList addWalls(int height, int width, ArrayList walls){
		walls.Add (new Pos3 (height, width, (int)Directions.Up));
		walls.Add (new Pos3 (height, width, (int)Directions.Down));
		walls.Add (new Pos3 (height, width, (int)Directions.Left));
		walls.Add (new Pos3 (height, width, (int)Directions.Right));
		return walls;
	}

	public Pos2 selectStartingCell(int height, int width){
		return new Pos2(Mathf.FloorToInt(Random.Range(0, (height*1.0f)/2)+(height*1.0f)/4), Mathf.FloorToInt(Random.Range(0, (width*1.0f)/2)+(width*1.0f)/4));
	}
}
