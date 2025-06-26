
using UnityEngine;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject wristUI;
    [SerializeField] XRNode handNode = XRNode.LeftHand;
    [SerializeField] private float angleThreshHold = 45f;

    private bool uiVisible = false;

    private int totalScore;

    private void Update()
    {
        CheckWristUI();
    }

    private void CheckWristUI()
    {
        InputDevices.GetDeviceAtXRNode(handNode).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
        Vector3 eulerAngles = rotation.eulerAngles;
        float pitch = NormalizeAngle(eulerAngles.z);
        Debug.Log($"Pitch: {pitch}");

        if (Mathf.Abs(pitch) > angleThreshHold)
        {
            EnableWristUI();
        }
        else if (Mathf.Abs(pitch) <= angleThreshHold)
        {
            DisableWristUI();
        }
    }

    private float NormalizeAngle(float angle)
    {
        return (angle > 180) ? angle - 360 : angle;
    }

    private void EnableWristUI()
    {
        wristUI.SetActive(true);
    }

    private void DisableWristUI()
    {
        wristUI.SetActive(false);
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
    }

}



