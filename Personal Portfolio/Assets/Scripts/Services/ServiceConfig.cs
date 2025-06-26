using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "scriptableObjects/ServiceConfig")]
public class ServiceConfig : ScriptableObject
{
    public void SetUpServices()
    {
        UIManager uiManager = new();
        Services.RegisterService(uiManager);
    }
}
