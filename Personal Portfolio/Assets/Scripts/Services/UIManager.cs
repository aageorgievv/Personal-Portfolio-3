using UnityEngine;

public class UIManager : Service
{
    private int totalScore;
    public override void InitializeService()
    {
        
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
    }
}
