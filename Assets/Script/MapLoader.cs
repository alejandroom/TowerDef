using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour {

	public GameObject Center;
	public GameObject Respawn;
	public GameObject Road;
	public GameObject Tower;
	public GameObject Trash;

	public int Height=30;
	public int Width=30;
	public int numEntrances = 4;

	public string type;

	void Start(){
		Object.DontDestroyOnLoad (gameObject);
	}
	
	public void launch(){
		Generator g = new Generator ();
		g.newMaze (Height, Width);
		if( type == "Square" || type == ""){
			g.setStartingCell(g.selectStartingCell(Height, Width));
			for (int i=0; i<numEntrances; i++) {
				g.generateMaze ();
				if(i%4==0)
					g.traverse (0, Random.Range (0, Width-1), Generator.Directions.Up, i);
				if(i%4==1)
					g.traverse (Height-1, Random.Range (0, Width-1), Generator.Directions.Down, i);
				if(i%4==2)
					g.traverse (Random.Range (0, Height-1), 0, Generator.Directions.Left, i);
				if(i%4==3)
					g.traverse (Random.Range (0, Height-1), Width-1, Generator.Directions.Right, i);
			}
		}else if( type == "Rectangular"){
			g.setStartingCell(new Pos2(0, Random.Range (0, Width-1)));
			for (int i=0; i<numEntrances; i++){
				g.generateMaze ();
				g.traverse (Height-1, Random.Range (0, Width-1), Generator.Directions.Down, i);
			}
		}
		g.generateTowers ();
		loadMap (g.maze, Height, Width);
		GameObject.FindObjectOfType<Light>().gameObject.transform.position = new Vector3(Height/2, 8, Width/2);
	}
	
	void loadMap(Cell[,] maze, int height, int width){
		Quaternion up = Quaternion.LookRotation (Vector3.back); 
		Quaternion down = Quaternion.LookRotation (Vector3.forward); 
		Quaternion left = Quaternion.LookRotation (Vector3.left); 
		Quaternion right = Quaternion.LookRotation (Vector3.right); 

		for (int j=0; j<width; j++) {
			for (int i=0; i<height; i++) {
				Vector3 pos = new Vector3(j, 0, i);
				if(maze[i,j].road == 0){
					if(maze[i,j].type == Cell.Types.Tower)
						Instantiate(Tower, pos, up);
					else
						Instantiate(Trash, pos, up);
				}else if(maze[i,j].type==Cell.Types.Respawn){
					if(maze[i,j].dir[0]==Generator.Directions.Down){
						GameObject clone = Instantiate(Respawn, pos, down) as GameObject;
						UnitRespawn resp = clone.GetComponentInChildren<UnitRespawn>();
						resp.path = maze[i,j].paths[0];
					}else if(maze[i,j].dir[0]==Generator.Directions.Up){
						GameObject clone = Instantiate(Respawn, pos, up) as GameObject;
						UnitRespawn resp = clone.GetComponentInChildren<UnitRespawn>();
						resp.path = maze[i,j].paths[0];
					}else if(maze[i,j].dir[0]==Generator.Directions.Right){
						GameObject clone = Instantiate(Respawn, pos, right) as GameObject;
						UnitRespawn resp = clone.GetComponentInChildren<UnitRespawn>();
						resp.path = maze[i,j].paths[0];
					}else if(maze[i,j].dir[0]==Generator.Directions.Left){
						GameObject clone = Instantiate(Respawn, pos, left) as GameObject;
						UnitRespawn resp = clone.GetComponentInChildren<UnitRespawn>();
						resp.path = maze[i,j].paths[0];
					}
				}else if(maze[i,j].type==Cell.Types.Center){
					Instantiate(Center, pos, up);
				}else if(maze[i,j].type==Cell.Types.Road){
					for(int k = 0 ; k < maze[i,j].road ; k ++){
						if(maze[i,j].dir[k]==Generator.Directions.Down){
							GameObject clone = Instantiate(Road, pos, down) as GameObject;
							Arrow arrow = clone.GetComponentInChildren<Arrow>();
							arrow.path = maze[i,j].paths[k];
						}else if(maze[i,j].dir[k]==Generator.Directions.Up){
							GameObject clone = Instantiate(Road, pos, up) as GameObject;
							Arrow arrow = clone.GetComponentInChildren<Arrow>();
							arrow.path = maze[i,j].paths[k];
						}else if(maze[i,j].dir[k]==Generator.Directions.Right){
							GameObject clone = Instantiate(Road, pos, right) as GameObject;
							Arrow arrow = clone.GetComponentInChildren<Arrow>();
							arrow.path = maze[i,j].paths[k];
						}else if(maze[i,j].dir[k]==Generator.Directions.Left){
							GameObject clone = Instantiate(Road, pos, left) as GameObject;
							Arrow arrow = clone.GetComponentInChildren<Arrow>();
							arrow.path = maze[i,j].paths[k];
						}
					}
				}
			}
		}
	}
}
