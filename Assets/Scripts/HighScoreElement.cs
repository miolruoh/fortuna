using System;

// Class for new highscore
[Serializable]
public class HighScoreElement{
    public string playerName;
    public int score;

    public HighScoreElement(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}
