using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    public void RegisterHit(Hitzone zone)
    {
        uiManager.AddScore(zone.points);
    }
}
