using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void RegisterHit(Hitzone zone)
    {
        Services.Get<UIManager>().AddScore(zone.points);
    }
}
