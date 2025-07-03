using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TargetPracticeManager : MonoBehaviour
{
    public float Speed => speed;
    public float RespawnRate => respawnRate;

    [SerializeField] private List<Target> targets;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float respawnRate = 5f;
    [SerializeField] private float targetActivationRate = 2f;

    private float hitTime = -1;

    private void Start()
    {
        foreach (Target target in targets)
        {
            target.Initialize(this);
        }

        StartCountDown();
    }

    private void Update()
    {
        TargetActivation();
    }

    private void TargetActivation()
    {
        if(Time.time > hitTime + respawnRate)
        {
            int randomIndex = Random.Range(0, targets.Count);
            Target randomTarget = targets[randomIndex];

            randomTarget.TargetRise();
        }
    }

    public void StartCountDown()
    {
        hitTime = Time.time;
    }
}
