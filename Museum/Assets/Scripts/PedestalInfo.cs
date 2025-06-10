using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;                     

public class MessageBoxTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject messagePanel;  
    [SerializeField] private TMP_Text bodyText;       

    [Header("Per-object message")]
    [TextArea(2, 5)]
    [SerializeField] private string message;

    private bool panelOpen;

    void Awake()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void OnMouseUpAsButton()           
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!panelOpen) Show();
    }

    void Update()
    {
        if (panelOpen &&
            Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            Hide();
        }
    }

    private void Show()
    {
        bodyText.text = message;       
        messagePanel.SetActive(true);
        panelOpen = true;
    }

    private void Hide()
    {
        messagePanel.SetActive(false);
        panelOpen = false;
    }
}
