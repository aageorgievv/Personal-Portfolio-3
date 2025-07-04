using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject movePoint;
    [SerializeField] private bool canMove = false;


    private Animator animator;
    private TargetPracticeManager manager;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        if (movePoint != null)
        {
            endPosition = movePoint.transform.position;
        }

        StartCoroutine(Move());
    }

    private void Update()
    {

    }

    public void Initialize(TargetPracticeManager manager)
    {
        this.manager = manager;
    }

    public void RegisterHit(Hitzone zone)
    {
        animator.SetTrigger("TargetDrop");
        uiManager.AddScore(zone.points);
        canMove = false;
        manager.RegisterTargetHit(zone);
    }

    public void TargetRise()
    {
        animator.SetTrigger("TargetRise");
    }

    private IEnumerator Move()
    {
        if (canMove)
        {

            while (Vector3.Distance(transform.position, endPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, manager.Speed * Time.deltaTime);
                yield return null;
            }

            while (Vector3.Distance(transform.position, startPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, manager.Speed * Time.deltaTime);
                yield return null;
            }

            StartCoroutine(Move());
        }
    }
}
