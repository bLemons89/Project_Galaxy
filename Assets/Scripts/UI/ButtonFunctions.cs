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

public class ButtonFunctions : MonoBehaviour
{
    SceneManagerScript sceneManager;

    [Header("===== MENUS =====")]
    [SerializeField] GameObject backgroundScreen;
    private CanvasGroup backgroundGroup;

    [SerializeField] GameObject pauseMenu;
    private CanvasGroup pauseGroup;
    [SerializeField] GameObject pauseTitle;
    private TextMeshProUGUI titleText;
    [SerializeField] GameObject pauseButtons;
    private CanvasGroup pauseButtonsGroup;

    [SerializeField] GameObject settingsMenu;
    private CanvasGroup settingsGroup;

    [SerializeField] GameObject controlsMenu;
    private CanvasGroup controlsGroup;

    // Flags //
    private bool settingsButton;

    // For Settings //
    [SerializeField] private bool enableFlickering;

    // Getters and Setters //
    public GameObject BackgroundScreen
    { get => backgroundScreen; set => backgroundScreen = value; }
    public GameObject PauseMenu
    { get => pauseMenu; set => pauseMenu = value; }
    public GameObject PauseTitle
    { get => pauseTitle; set => pauseTitle = value; }
    public GameObject PauseButtons
    { get => pauseButtons; set => pauseButtons = value; }
    public GameObject SettingsMenu
    { get => settingsMenu; set => settingsMenu = value; }

    public void ButtonsInitialize()
    {
        //find and set UI canvas groups
        backgroundGroup = backgroundScreen.GetComponent<CanvasGroup>();
        pauseGroup = pauseMenu.GetComponent<CanvasGroup>();
        titleText = pauseTitle.GetComponent<TextMeshProUGUI>();
        pauseButtonsGroup = pauseButtons.GetComponent<CanvasGroup>();
        settingsGroup = settingsMenu.GetComponent<CanvasGroup>();

        // for animate in
        backgroundGroup.alpha = 0f;
        backgroundScreen.transform.localScale = Vector3.zero;

        enableFlickering = true;
        settingsButton = false;

    }

    // Pause Buttons //
    public void Resume()
    {
        GameManager.instance.StateUnPause();
    }

    public void Restart()
    {
        //sceneManager.ResetScene();
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
        //if (controlsMenu.activeSelf)
        //{
        //    controlsPos.DOAnchorPos(new Vector2(0, 2024), 0.25f)
        //               .SetEase(Ease.OutQuad)
        //               .SetUpdate(true);
        //}

        //pauseMenu.SetActive(false);
        //settingsMenu.SetActive(true);

        //settingsPos.DOAnchorPos(new Vector2(0, 0), 0.25f)
        //           .SetEase(Ease.InQuad)
        //           .SetUpdate(true);
    }

    // Settings Buttons //
    public void ControlsButton()
    {
        //controlsMenu.SetActive(true);
        //controlsPos.DOAnchorPos(new Vector2(0, 0), 0.25f)
        //           .SetEase(Ease.OutQuad)
        //           .SetUpdate(true);
    }
    public void BackButton()
    {
        //settingsPos.DOAnchorPos(new Vector2(0, -1100), 0.25f)
        //           .SetEase(Ease.OutQuad)
        //           .SetUpdate(true)
        //           .OnComplete(() =>
        //           {
        //               settingsMenu.SetActive(false);
        //           });     
        //pauseMenu.SetActive(true);
    }

    // Screens //
    public void BackgroundGroupOpen()
    {
        DOTween.KillAll();
        //Turn on
        backgroundScreen.SetActive(true);
        backgroundGroup.alpha = 0f;
        backgroundScreen.transform.localScale = Vector3.zero;

        //Scale/Fade
        backgroundScreen.transform.DOScale(Vector3.one, 0.25f)
                        .SetEase(Ease.InOutQuad)
                        .SetUpdate(true);
        backgroundGroup.DOFade(0.98f, 0.25f)
                       .SetUpdate(true);
        //Turn on
        pauseMenu.SetActive(true);
        pauseGroup.alpha = 0f;
        pauseMenu.transform.localScale = Vector3.zero;

        //Scale/Fade
        pauseMenu.transform.DOScale(Vector3.one, 0.5f)
                           .SetEase(Ease.InOutBack)
                           .SetUpdate(true);
        pauseGroup.DOFade(1f, .5f)
                  .SetUpdate(true);
        //Turn on
        pauseTitle.SetActive(true);
        titleText.alpha = 0f;
        pauseTitle.transform.localScale = Vector3.zero;

        //Scale/Fade
        pauseTitle.transform.DOScale(Vector3.one, 0.25f)
                  .SetEase(Ease.InOutQuad)
                  .SetUpdate(true);

        titleText.DOFade(1f, 1f)
                 .SetEase(Ease.InOutElastic)
                 .SetUpdate(true);

        titleText.DOFade(0.75f, 0.15f)
                 .SetEase(Ease.InOutExpo)
                 .SetLoops(-1, LoopType.Restart)
                 .SetDelay(0.5f)
                 .SetUpdate(true);

        float randomDelay = Random.Range(0.5f, 1.5f);
        titleText.DOFade(1f, Random.Range(0.1f, 0.5f))
                 .SetEase(Ease.InOutElastic)
                 .SetDelay(randomDelay)
                 .SetUpdate(true);

        //Animate in
        float totalDelay = .5f;
        float delayBetween = 0.15f;
        for (int i = 0; i < pauseButtonsGroup.transform.childCount; i++)
        {
            GameObject button = pauseButtons.transform.GetChild(i).gameObject;

            button.SetActive(true);
            button.transform.localScale = Vector3.zero;

            button.transform.DOScale(Vector3.one, 0.15f)
                            .SetEase(Ease.InOutQuad)
                            .SetDelay(totalDelay)
                            .SetUpdate(true);

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (enableFlickering)
            {
                buttonText.DOFade(0.75f, 0.15f)
                 .SetEase(Ease.InOutExpo)
                 .SetLoops(-1, LoopType.Restart)
                 .SetDelay(0.5f)
                 .SetUpdate(true);

                buttonText.DOFade(1f, Random.Range(0.1f, 0.5f))
                          .SetEase(Ease.InOutElastic)
                          .SetDelay(randomDelay)
                          .SetUpdate(true);
            }
            totalDelay += delayBetween;
        }
    }
    public void BackgroundGroupClose()
    {
        DOTween.KillAll();

        for (int i = 0; i < pauseButtonsGroup.transform.childCount; i++)
        {
            GameObject button = pauseButtons.transform.GetChild(i).gameObject;

            button.transform.DOScale(Vector3.zero, 0.15f)
                            .SetEase(Ease.InOutQuad)
                            .SetDelay(0.15f)
                            .SetUpdate(true);

            button.SetActive(false);
        }

        //Scale/Fade
        pauseTitle.transform.DOScale(Vector3.zero, 0.25f)
                  .SetEase(Ease.InOutQuad)
                  .SetUpdate(true);
        //Turn off
        pauseTitle.SetActive(false);

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
