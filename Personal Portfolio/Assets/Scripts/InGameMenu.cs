using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    public InputActionReference openMenuAction;

    private void OnEnable()
    {
        openMenuAction.action.Enable();
        openMenuAction.action.performed += OnToggleMenu;
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        openMenuAction.action.Disable();
        openMenuAction.action.performed -= OnToggleMenu;
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
    private void OnToggleMenu(InputAction.CallbackContext context)
    {
        if (menu != null)
        {
            menu.SetActive(!menu.activeSelf);
        }
    }

    private void OnDeviceChange(InputDevice input, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                openMenuAction.action.Disable();
                openMenuAction.action.performed -= OnToggleMenu;
                break;
            case InputDeviceChange.Reconnected:
                openMenuAction.action.Enable();
                openMenuAction.action.performed += OnToggleMenu;
                break;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
