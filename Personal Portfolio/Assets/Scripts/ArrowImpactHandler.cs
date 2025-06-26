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

        Hitzone zone = collision.collider.GetComponent<Hitzone>();
        if(zone != null)
        {
            Target target = zone.GetComponentInParent<Target>();
            if(target != null)
            {
                target.RegisterHit(zone);
            }
        }

        hasHit = true;
        arrowLauncher.StopFlight();
        DespawnAfterDelay();
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
