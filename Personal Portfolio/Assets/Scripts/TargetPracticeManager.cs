using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TargetPracticeManager : MonoBehaviour
{
    public float Speed => speed;
    public float TargetRiseTime => targetRiseTime;

    [SerializeField] private List<Target> targets;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float targetRiseTime = 5f;

    private float hitTime = -1f;
    private bool isTargetActive = false;

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
        if(!isTargetActive && Time.time > hitTime + targetRiseTime)
        {
            int randomIndex = Random.Range(0, targets.Count);
            Target randomTarget = targets[randomIndex];
            hitTime = 0f;
            isTargetActive = true;
            randomTarget.TargetRise();
        }
    }

    public void StartCountDown()
    {
        hitTime = Time.time;
        isTargetActive = false;
    }
}
