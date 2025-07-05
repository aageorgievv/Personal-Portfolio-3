using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    private InputActionProperty toggleButton;
    void Start()
    {

    }

    void Update()
    {

    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        menu.SetActive(!menu.activeSelf);
    }
}
