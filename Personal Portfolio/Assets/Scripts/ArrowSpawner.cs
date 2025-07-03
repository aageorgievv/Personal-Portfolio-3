using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject notchPoint;
    [SerializeField] private float spawnDelay = 0.3f;

    private XRGrabInteractable bow;
    private XRPullInteractable pullInteractable;
    private bool isArrowNotched = false;
    private GameObject currentArrow = null;

    void Start()
    {
        bow = GetComponent<XRGrabInteractable>();
        pullInteractable = GetComponentInChildren<XRPullInteractable>();

        if(pullInteractable != null )
        {
            pullInteractable.OnPullReleased += NotchEmpty;
        }
    }

    private void OnDestroy()
    {
        if(pullInteractable != null )
        {
            pullInteractable.OnPullReleased -= NotchEmpty;
        }
    }

    private void Update()
    {
        if(bow.isSelected && !isArrowNotched)
        {
            isArrowNotched = true;
            StartCoroutine(DelayedSpawn());
        }

        if(!bow.isSelected && currentArrow != null)
        {
            Destroy(currentArrow);
            NotchEmpty(1f);
        }
    }

    private void NotchEmpty(float value)
    {
        isArrowNotched = false;
        currentArrow = null;
    }

    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);

        currentArrow = Instantiate(arrowPrefab, notchPoint.transform);

        ArrowLauncher launcher = currentArrow.GetComponent<ArrowLauncher>();
        if(launcher != null && pullInteractable != null)
        {
            launcher.Initialize(pullInteractable);
        }
    }
}
