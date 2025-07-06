using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class TargetPracticeManager : MonoBehaviour
{
    public float Speed => speed;
    public float TargetRiseTime => targetRiseTime;
    public bool SessionStarted => sessionStarted;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private List<Target> targets;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float targetRiseTime = 5f;

    private float hitTime = 0f;

    private bool isTargetActive = false;
    private bool sessionStarted = false;

    private int targetHit = 0;
    private int totalTargets = 10;
    private void Start()
    {
        foreach (Target target in targets)
        {
            target.Initialize(this);
        }
    }

    private void Update()
    {
        TargetActivation();
    }

    private void TargetActivation()
    {
        if (sessionStarted && targetHit >= totalTargets)
        {
            EndTargetPractice();
        }

        if(sessionStarted && !isTargetActive && Time.time > hitTime + targetRiseTime)
        {
            int randomIndex = Random.Range(0, targets.Count);
            Target randomTarget = targets[randomIndex];
            hitTime = 0f;
            isTargetActive = true;
            randomTarget.TargetRise();
        }
    }

    public void RegisterTargetHit(Hitzone zone)
    {
        targetHit++;
        uiManager.UpdateTargetHitUI(targetHit, totalTargets);
        StartCountDown();
    }

    public void StartCountDown()
    {
        hitTime = Time.time;
        isTargetActive = false;
    }

    public void StartTargetPractice()
    {
        uiManager.ResetScore();
        targetHit = 0;
        hitTime = 0;
        isTargetActive = false;
        sessionStarted = true;
        uiManager.EnableRecordText(false);
        uiManager.EnableTotalTargetsText(true);
        uiManager.UpdateTargetHitUI(targetHit, totalTargets);
    }
    public void EndTargetPractice()
    {
        uiManager.EnableTotalTargetsText(false);
        uiManager.UpdateRecordText();
        uiManager.EnableRecordText(true);
        sessionStarted = false;
    }
}
