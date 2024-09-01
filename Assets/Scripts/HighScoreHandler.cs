using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    List<HighScoreElement> highScoreList = new List<HighScoreElement>();
    [SerializeField] readonly int maxCount = 10;   // amount of highscores to add to the highscore list
    public int MaxCount
    {
        get {return maxCount;}
    }
    private readonly string fileName = MenuCommands.FileName;
    // Delegate to check if highscore list has changed
    public delegate void OnHighScoreListChanged(List<HighScoreElement> list);
    public static event OnHighScoreListChanged onHighScoreListChanged;
    
    // Load highscore list from file if it has changed and remove lowest scores from list if have to
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
    // Get lowest score(last place) in the list
    public int GetLastHighScore()
    {
        highScoreList = FileHandler.ReadFromJSON<HighScoreElement>(fileName);
        return highScoreList[GetHighScoreCount() -1].score;
    }
    // Get amount of highscores in the list
    public int GetHighScoreCount()
    {
        highScoreList = FileHandler.ReadFromJSON<HighScoreElement>(fileName);
        return highScoreList.Count;
    }
    // Save highscorelist to file
    private void SaveHighScore()
    {
        FileHandler.SaveToJSON<HighScoreElement>(highScoreList, fileName);
    }
    // Check if score is high enough to add to the list and add it to the right spot
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
