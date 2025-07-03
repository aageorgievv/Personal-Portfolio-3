using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject movePoint;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 5f;
    [SerializeField] private bool canMove = false;


    private Animator animator;

    private bool isHit = false;

    private float hitTime = -1;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        startPosition = transform.position;
        endPosition = movePoint.transform.position;

        StartCoroutine(Move());
    }

    private void Update()
    {
        TargetRise();
    }
    public void RegisterHit(Hitzone zone)
    {
        if (!isHit)
        {
            uiManager.AddScore(zone.points);
            animator.SetTrigger("TargetDrop");
            isHit = true;
            canMove = false;
            hitTime = Time.time;
        }
    }

    public void TargetRise()
    {
        if(isHit && Time.time > hitTime + resetTime)
        {
            animator.SetTrigger("TargetRise");
            isHit = false;
            canMove = true;
            hitTime = -1;
        }
    }

    private IEnumerator Move()
    {
        if(canMove)
        {

            while (Vector3.Distance(transform.position, endPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
                yield return null;
            }

            while (Vector3.Distance(transform.position, startPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
                yield return null;
            }

            StartCoroutine(Move());
        }
    }
}
