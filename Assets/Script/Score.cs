using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Score{
	public int maxScores = 10;

	public ScoreData data;

	public bool eval(int Score){
		foreach (int s in data.score)
			if (Score > s)
				return true;
		if (data.score.Length < maxScores)
			return true;
		return false;
	}

	public void newScore(string Name, int Score, int Time){
		int i;
		for (i = 0 ; i < data.name.Length ; i++)
			if (Score > data.score [i])
				break;
		if (i == data.name.Length && data.name.Length < maxScores) {
			data.name [data.name.Length] = Name;
			data.score [data.name.Length] = Score;
			data.time [data.name.Length] = Time;
		} else {
			for(int j = data.name.Length-1 ; j > i ; j--){
				data.name[j] = data.name[j-1];
				data.score[j] = data.score[j-1];
				data.time[j] = data.time[j-1];
			}
			data.name [i] = Name;
			data.score [i] = Score;
			data.time [i] = Time;
		}
	}
	
	public void Save(){
		if (data == null)
			return;
		
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/scores.dat", FileMode.OpenOrCreate);
		
		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load(){

		if (!File.Exists (Application.persistentDataPath + "/scores.dat")) {
			data = new ScoreData(maxScores);
			return;
		}
		
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/scores.dat", FileMode.Open);
		
		data = (ScoreData)bf.Deserialize (file);
		file.Close ();
	}

	public string Debug(){
		string text = "";
		for (int i = 0; i < data.name.Length; i++)
			text += data.name [i] + ":" + data.score [i] + ":" + data.time [i] + "\n";

		return text;
	}
}

[Serializable]
public class ScoreData{
	public string[] name;
	public int[] score;
	public int[] time;

	public ScoreData(int size){
		name = new string[size];
		score = new int[size];
		time = new int[size];
	}
}