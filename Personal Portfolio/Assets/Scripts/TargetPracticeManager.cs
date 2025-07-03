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

    void Start()
    {

    }

    void Update()
    {

    }

    private void TargetActivation()
    {
        int randomIndex = Random.Range(0, targets.Count);
        Target randomTarget = targets[randomIndex];

        randomTarget.TargetRise();
    }
}
