using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Score :IComparable<Score>
{
    public string name;
    public int score;

    public Score(string newName,int newScore)
    {
        name = newName;
        score = newScore;
    }

    public int CompareTo(Score other)
    {
        if (other == null)
        {
            return 1;
        }
        return other.score-score;
    }
}
