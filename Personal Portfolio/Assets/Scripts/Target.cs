using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<int> OnTargetHit;
    public void RegisterHit(Hitzone zone)
    {
        OnTargetHit?.Invoke(zone.points); // for a ScoreManager/UI Manager
    }
}
