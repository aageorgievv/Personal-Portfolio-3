
using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject wristUI;
    [SerializeField] GameObject totalTargetsTextParent;
    [SerializeField] TMP_Text totalTargetsText;
    [SerializeField] GameObject recordTextParent;
    [SerializeField] TMP_Text recordText;
    [SerializeField] XRNode handNode = XRNode.LeftHand;
    [SerializeField] private float angleThreshHold = 45f;

    private int previousScore = 0;
    private int currentScore;

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
        currentScore += amount;
    }

    private void UpdateScoreUI()
    {
        TMP_Text pointsText = wristUI.GetComponentInChildren<TMP_Text>(true);
        pointsText.text = $"{currentScore}";
    }

    public void UpdateTargetHitUI(int current, int total)
    {
        totalTargetsText.text = $"{current}/{total}";
    }

    public void EnableTotalTargetsText(bool state)
    {
        totalTargetsTextParent.SetActive(state);
    }

    public void UpdateRecordText()
    {
        if(previousScore < currentScore)
        {
            recordText.text = $"{currentScore}";
            previousScore = currentScore;
        }
    }

    public void EnableRecordText(bool state)
    {
        recordTextParent.SetActive(state);
    }
    public void ResetScore()
    {
        currentScore = 0;
    }
}



