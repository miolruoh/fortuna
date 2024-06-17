using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    List<HighScoreElement> highScoreList = new List<HighScoreElement>();
    [SerializeField] readonly int maxCount = 10;
    public int MaxCount
    {
        get {return maxCount;}
    }
    [SerializeField] readonly string fileName = "highscores1.json";

    public delegate void OnHighScoreListChanged(List<HighScoreElement> list);
    public static event OnHighScoreListChanged onHighScoreListChanged;

    public void LoadHighScores() 
    {
        highScoreList = FileHandler.ReadFromJSON<HighScoreElement>(fileName);

        while(highScoreList.Count > maxCount) 
        {
            highScoreList.RemoveAt(maxCount);
        }

        if(onHighScoreListChanged != null) 
        {
            onHighScoreListChanged.Invoke(highScoreList);
        }
    }

    public int GetLastHighScore()
    {
        highScoreList = FileHandler.ReadFromJSON<HighScoreElement>(fileName);
        return highScoreList[GetHighScoreCount() -1].score;
    }

    public int GetHighScoreCount()
    {
        highScoreList = FileHandler.ReadFromJSON<HighScoreElement>(fileName);
        return highScoreList.Count;
    }

    private void SaveHighScore()
    {
        FileHandler.SaveToJSON<HighScoreElement>(highScoreList, fileName);
    }

    public void AddHighScoreIfPossible (HighScoreElement element) 
    {
        for(int i = 0; i < maxCount; i++) 
        {
            if(i >= highScoreList.Count || element.score > highScoreList[i].score) 
            {
                // add new high score
                highScoreList.Insert(i, element);

                while(highScoreList.Count > maxCount) 
                {
                    highScoreList.RemoveAt(maxCount);
                }
                SaveHighScore();

                if(onHighScoreListChanged != null) 
                {
                    onHighScoreListChanged.Invoke(highScoreList);
                }
                break;
            }
        }
    }



}
