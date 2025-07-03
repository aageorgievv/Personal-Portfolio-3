using System.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ArrowLauncher : MonoBehaviour
{
    [Header("Launch Settings)")]
    [SerializeField] private float speed = 10f;

    private Rigidbody rb;
    private bool inAir = false;
    private XRPullInteractable _pullInteractable;

    private void Awake()
    {
        InitializeComponents();
        SetPhysics(false);
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>();

        if(rb == null)
        {
            Debug.LogError($"RigidBody not found in {gameObject.name}");
        }
    }

    public void Initialize(XRPullInteractable pullInteractable)
    {
        _pullInteractable = pullInteractable;
        _pullInteractable.OnPullReleased += Release;
    }

    private void OnDestroy()
    {
        if( _pullInteractable != null )
        {
            _pullInteractable.OnPullReleased -= Release;
        }
    }

    private void Release(float value)
    {
        if(_pullInteractable != null)
        {
            _pullInteractable.OnPullReleased -= Release;
        }

        transform.parent = null;
        inAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * value * speed;
        rb.AddForce(force, ForceMode.Impulse);

        StartCoroutine(RotateWithVelocity());
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();

        while(inAir)
        {
            if(rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(rb.linearVelocity, transform.up);
            }
            yield return null;
        }
    }

    public void StopFlight()
    {
        inAir = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool state)
    {
        if(rb != null)
        {
            rb.useGravity = state;
            rb.isKinematic = !state;
        }
    }
}
