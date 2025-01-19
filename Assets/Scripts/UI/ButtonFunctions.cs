using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor;
using TMPro;

public class ButtonFunctions : GameManager
{
    [Header("===== OVERLAYS =====")]
    private CanvasGroup backgroundGroup;
    private CanvasGroup pauseGroup;
    private CanvasGroup pauseButtonsGroup;
    private CanvasGroup settingsGroup;

    [SerializeField] GameObject backgroundScreen;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject pauseButtons;
    
    [SerializeField] RectTransform settingsPos, controlsPos;

    private bool settingsButton;

    SceneManagerScript sceneManager;

    // Getters and Setters //
    public GameObject BackgroundScreen
    { get => backgroundScreen; set => backgroundScreen = value; }
    public GameObject PauseMenu
    { get => pauseMenu; set => pauseMenu = value; }
    public GameObject SettingsMenu
    { get => settingsMenu; set => settingsMenu = value; }
    public GameObject PauseButtons
    { get => pauseButtons; set => pauseButtons = value; }

    public CanvasGroup BackgroundGroup
    { get => backgroundGroup; set => backgroundGroup = value; }
    public CanvasGroup PauseGroup
    { get => pauseGroup; set => pauseGroup = value; }
    public CanvasGroup SettingsGroup
    { get => settingsGroup; set => settingsGroup = value; }
    public CanvasGroup PauseButtonsGroup
    { get => pauseButtonsGroup; set => pauseButtonsGroup = value; }

    public void ButtonsInitialize()
    {
        //find and set UI canvas groups
        backgroundGroup = backgroundScreen.GetComponent<CanvasGroup>();
        pauseGroup = pauseMenu.GetComponent<CanvasGroup>();
        pauseButtonsGroup = pauseMenu.GetComponent<CanvasGroup>();
        settingsGroup = settingsMenu.GetComponent<CanvasGroup>();
 
        // for animate in
        backgroundGroup.alpha = 0f;
        backgroundScreen.transform.localScale = Vector3.zero;

        settingsButton = false;
    }
    
    // Pause Buttons //
    public void Resume()
    {
        StateUnPause();
    }

    public void Restart() 
    {
        sceneManager.ResetScene();
        //GameManager.instance.PlayerScript.Respawn();
    }
    
    public void SaveGame() 
    {
        //Navigate to Save/Load Screen
    }

    public void MainMenu() 
    {
        //Navigate to Main Menu Scene
    }

    public void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }

    public void SettingsButton()
    {
        if (controlsMenu.activeSelf)
        {
            controlsPos.DOAnchorPos(new Vector2(0, 2024), 0.25f)
                       .SetEase(Ease.OutQuad)
                       .SetUpdate(true);
        }

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        settingsPos.DOAnchorPos(new Vector2(0, 0), 0.25f)
                   .SetEase(Ease.InQuad)
                   .SetUpdate(true);
    }

    // Settings Buttons //
    public void ControlsButton()
    {
        controlsMenu.SetActive(true);
        controlsPos.DOAnchorPos(new Vector2(0, 0), 0.25f)
                   .SetEase(Ease.OutQuad)
                   .SetUpdate(true);
    }
    public void BackButton()
    {
        settingsPos.DOAnchorPos(new Vector2(0, -1100), 0.25f)
                   .SetEase(Ease.OutQuad)
                   .SetUpdate(true)
                   .OnComplete(() =>
                   {
                       settingsMenu.SetActive(false);
                   });     
        pauseMenu.SetActive(true);
    }

    // Screens //
    public void BackgroundGroupOpen()
    {
        if (DOTween.IsTweening(backgroundScreen.transform)) return;

        backgroundScreen.SetActive(true);

        backgroundScreen.transform.DOScale(Vector3.one, 0.25f)
                        .SetEase(Ease.InOutQuad)
                        .SetUpdate(true);
        backgroundGroup.DOFade(0.98f, 0.25f)
                       .SetUpdate(true);

        pauseMenu.SetActive(true);
        pauseMenu.transform.DOScale(Vector3.one, 0.5f)
                           .SetEase(Ease.InOutBack)
                           .SetUpdate(true);
        pauseGroup.DOFade(1f, .5f)
                  .SetUpdate(true);

        pauseButtons.SetActive(true);
        
    }
    public void BackgroundGroupClose()
    {
        if (DOTween.IsTweening(backgroundScreen.transform)) return;

        pauseMenu.transform.DOScale(Vector3.zero, 0.5f)
                           .SetEase(Ease.InOutBack)
                           .SetUpdate(true);
        pauseGroup.DOFade(1f, .5f)
                  .SetUpdate(true)
                  .OnComplete(() =>
                  {
                      pauseMenu.SetActive(false);
                  });

        backgroundScreen.transform
                     .DOScale(Vector3.zero, 0.5f)
                     .SetEase(Ease.InOutQuad)
                     .SetUpdate(true);
        
        backgroundGroup.DOFade(0f, 0.5f)
                       .SetUpdate(true)
                       .OnComplete(() => 
                       { 
                         backgroundScreen.SetActive(false); 
                       });
    }    
}
