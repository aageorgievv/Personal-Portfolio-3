using System.Collections;
using UnityEngine;

public class ArrowImpactHandler : MonoBehaviour
{
    [Header("Impact Settings")]
    [SerializeField] private float stickDuration = 3f;
    [SerializeField] private float minEmbedDepth = 0.05f;
    [SerializeField] private float maxEmbedDepth = 0.15f;
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private Transform tip;

    private ArrowLauncher arrowLauncher;
    private Rigidbody rb;
    private bool hasHit = false;

    private void Awake()
    {
        arrowLauncher = GetComponent<ArrowLauncher>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(hasHit || ((1 << collision.gameObject.layer) & ignoreLayers) != 0)
        {
            return;
        }

        hasHit = true;
        arrowLauncher.StopFlight();
        HandleStick(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        Hitzone zone = GetComponent<Hitzone>();
        if (zone != null)
        {
            Target target = zone.GetComponentInParent<Target>();
            if (target != null)
            {
                target.RegisterHit(zone);
            }
        }
    }

    private void HandleStick(Collision collision)
    {
        Vector3 arrowDirection = transform.forward;
        Vector3 arrowUpDirection = transform.up;
        ContactPoint contactPoint = collision.GetContact(0);

        float randomDepth = Random.Range(minEmbedDepth, maxEmbedDepth);
        Quaternion finalRotation = Quaternion.LookRotation(arrowDirection, arrowUpDirection);
        Vector3 centerOffset = tip.localPosition;
        Vector3 finalPosition = contactPoint.point - (finalRotation * centerOffset) + contactPoint.normal * -randomDepth;

        transform.SetPositionAndRotation(finalPosition, finalRotation);

        CreateStabJoint(collision, randomDepth);

        transform.SetParent(collision.transform, true);
        StartCoroutine(DespawnAfterDelay());
    }

    public ConfigurableJoint CreateStabJoint(Collision collision, float randomDepth)
    {
        var joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = collision.rigidbody;
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        var limit = joint.linearLimit;
        limit.limit = randomDepth;
        joint.linearLimit = limit;

        return joint;
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(stickDuration);
        Destroy(gameObject);
    }
}
