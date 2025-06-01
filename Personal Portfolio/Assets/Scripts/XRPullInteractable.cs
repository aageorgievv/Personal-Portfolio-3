using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace UnityEngine.XR.Interaction.Toolkit.Interactables
{
    public class XRPullInteractable : XRBaseInteractable
    {
        public event Action<float> OnPullReleased;
        public event Action<float> OnPullUpdated;
        public event Action OnPullStarted;
        public event Action OnPullEnded;

        [Header("Pull Settings")]
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;
        [SerializeField] private GameObject notchPoint;

        public float pullAmount { get; private set; } = 0.0f;

        private LineRenderer lineRenderer;
        private IXRSelectInteractor pullingInteractor = null;

        protected override void Awake()
        {
            base.Awake();
            lineRenderer = GetComponent<LineRenderer>();
        }

        public void SetPullInteractor(SelectEnterEventArgs args)
        {
            pullingInteractor = args.interactorObject;
            OnPullStarted?.Invoke();
        }

        public void Release()
        {
            OnPullReleased?.Invoke(pullAmount);
            OnPullEnded?.Invoke();
            pullingInteractor = null;
            pullAmount = 0.0f;
            notchPoint.transform.localPosition = new Vector3(notchPoint.transform.localPosition.x, notchPoint.transform.localPosition.y, 0f);
            UpdateStringAndNotch();
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if(isSelected && pullingInteractor != null)
                {
                    Vector3 pullPosition = pullingInteractor.GetAttachTransform(this).position;
                    float previousPull = pullAmount;
                    pullAmount = CalculatePull(pullPosition);

                    if(previousPull != pullAmount)
                    {
                        OnPullUpdated?.Invoke(pullAmount);
                    }

                    UpdateStringAndNotch();
                    HandleHaptics();
                }
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            SetPullInteractor(args);
        }

        private float CalculatePull(Vector3 pullPosition)
        {
            Vector3 pullDirection = pullPosition - startPoint.position;
            Vector3 targetDirection = endPoint.position - startPoint.position;
            float maxLenght = targetDirection.magnitude;

            targetDirection.Normalize();
            float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLenght;
            return Mathf.Clamp(pullValue, 0.0f, 1.0f);
        }

        private void UpdateStringAndNotch()
        {
            Vector3 linePosition = Vector3.Lerp(startPoint.localPosition, endPoint.localPosition, pullAmount);
            notchPoint.transform.localPosition = linePosition;
            lineRenderer.SetPosition(1, linePosition);
        }

        private void HandleHaptics()
        {
            if(pullingInteractor != null && pullingInteractor is XRBaseInputInteractor controllerInteractor)
            {
                controllerInteractor.SendHapticImpulse(pullAmount, 0.1f);
            }
        }
    }
}
