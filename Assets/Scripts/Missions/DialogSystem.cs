using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem instance { get; set; }

    public TextMeshProUGUI questText;

    public Button firstOption;
    public Button secondOption;
    public Canvas dialogUI;

    public bool isOpen;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else instance = this;
    }

    public void OpenDialogUI()
    {
        dialogUI.gameObject.SetActive(true);
        isOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDialogUI()
    {
        dialogUI.gameObject.SetActive(false);
        isOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}