
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject wristUI;
    [SerializeField] XRNode handNode = XRNode.LeftHand;
    [SerializeField] private float angleThreshHold = 45f;

    private TMP_Text pointsText;
    private int totalScore;

    private void Awake()
    {
        { }
    }

    private void Update()
    {
        CheckWristUI();
    }

    //WristUI
    private void CheckWristUI()
    {
        InputDevices.GetDeviceAtXRNode(handNode).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
        Vector3 eulerAngles = rotation.eulerAngles;
        float pitch = NormalizeAngle(eulerAngles.z);

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
        UpdateScoreUI();
    }

    private void DisableWristUI()
    {
        wristUI.SetActive(false);

    }

    public void AddScore(int amount)
    {
        totalScore += amount;
    }

    private void UpdateScoreUI()
    {
        TMP_Text pointsText = wristUI.GetComponentInChildren<TMP_Text>(true);
        pointsText.text = $"{totalScore}";
    }
}



