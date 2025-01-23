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
using JetBrains.Annotations;

public class ButtonFunctions : MonoBehaviour
{
    SceneManagerScript sceneManager;

    [Header("===== MENUS =====")]
    // Background Screen //
    [SerializeField] GameObject backgroundScreen;
    private CanvasGroup backgroundGroup;

    // Pause Menu //
    [SerializeField] GameObject pauseMenu;
    private CanvasGroup pauseGroup;
    // title
    [SerializeField] GameObject pauseTitle;
    private TextMeshProUGUI titleText;
    // buttons
    [SerializeField] GameObject pauseButtons;
    private CanvasGroup pauseButtonsGroup;

    // Settings Menu //
    [SerializeField] GameObject settingsMenu;
    private CanvasGroup settingsGroup;

    // Controls Menu //
    [SerializeField] GameObject controlsMenu;
    private CanvasGroup controlsGroup;

    // Inventory Menu //
    [SerializeField] GameObject inventoryMenu;
    private CanvasGroup inventoryGroup;

    // Save Menu //
    [SerializeField] GameObject saveMenu;
    private CanvasGroup saveGroup;

    // Credit Menu //
    [SerializeField] GameObject creditScreen;
    private CanvasGroup creditGroup;

    // For Win //
    [SerializeField] GameObject winScreen;

    // For Lose //
    [SerializeField] GameObject loseScreen;

    // Flags //
    //private bool settingsButton;

    // For Settings //
    [SerializeField] private bool enableFlickering;
    [SerializeField] TextMeshProUGUI musicVolumeText;
    [SerializeField] TextMeshProUGUI sfxVolumeText;
    public UnityEngine.UI.Slider _musicSlider, _sfxSlider, _playerSFXSlider;


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
    public GameObject ControlsMenu
    { get => controlsMenu; set => controlsMenu = value; }
    public GameObject InventoryMenu
    { get => inventoryMenu; set => inventoryMenu = value; }
    public GameObject SaveMenu
    { get => saveMenu; set => saveMenu = value; }
    public GameObject CreditScreen
    { get => creditScreen; set => creditScreen = value; }

    public void ButtonsInitialize()
    {
        //find and set UI canvas groups
        backgroundGroup = backgroundScreen.GetComponent<CanvasGroup>();

        pauseGroup = pauseMenu.GetComponent<CanvasGroup>();
        titleText = pauseTitle.GetComponent<TextMeshProUGUI>();
        pauseButtonsGroup = pauseButtons.GetComponent<CanvasGroup>();

        settingsGroup = settingsMenu.GetComponent<CanvasGroup>();

        controlsGroup = controlsMenu.GetComponent<CanvasGroup>();

        inventoryGroup = inventoryMenu.GetComponent<CanvasGroup>();

        saveGroup = saveMenu.GetComponent<CanvasGroup>();

        creditGroup = creditScreen.GetComponent<CanvasGroup>();

        _musicSlider = _musicSlider.GetComponent<UnityEngine.UI.Slider>();
        _sfxSlider = _musicSlider.GetComponent<UnityEngine.UI.Slider>();

        // for animate in
        backgroundGroup.alpha = 0f;
        backgroundScreen.transform.localScale = Vector3.zero;

        enableFlickering = true;
        //settingsButton = false;

    }

    // Pause Buttons //
    public void Resume()
    {
        GameManager.instance.StateUnPause();
    }

    public void Restart()
    {       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.PlayerScript.Respawn();
        GameManager.instance.StateUnPause();        
    }

    public void SaveGame()
    {
        //Navigate to Save/Load Screen
        GameManager.instance.MenuActive.SetActive(false);
        GameManager.instance.MenuActive = saveMenu;
        GameManager.instance.MenuActive.SetActive(true);
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
        GameManager.instance.MenuActive = SettingsMenu;
        SettingsMenu.SetActive(true);
        PauseMenu.SetActive(false);
        backgroundGroup.alpha = 1f;
        RectTransform settingsTransform = SettingsMenu.GetComponent<RectTransform>();

        settingsTransform.DOAnchorPos(new Vector3(0, 0, 0), 0.25f)
                         .SetEase(Ease.InOutQuad)
                         .SetUpdate(true);
        
    }

    // Settings Buttons //
    public void ControlsButton()
    {
        GameManager.instance.MenuActive.SetActive(false);
        GameManager.instance.MenuActive = controlsMenu;
        GameManager.instance.MenuActive.SetActive(true);

        //controlsMenu.SetActive(true);
        //controlsPos.DOAnchorPos(new Vector2(0, 0), 0.25f)
        //           .SetEase(Ease.OutQuad)
        //           .SetUpdate(true);
    }
    public void BackButton()
    {
        GameManager.instance.MenuActive.SetActive(false);
        BackgroundGroupOpen();

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
        GameManager.instance.MenuActive = PauseMenu;

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


    // Settings //
    public void ToggleFlicker()
    {
        enableFlickering = !enableFlickering;
    }
    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }
    public void TogglePlayerSFX()
    {
        AudioManager.instance.TogglePlayerSFX();
    }
    public void MuteAllSFX()
    {
        AudioManager.instance.MuteAllSFX();
    }
    public void UnMuteAllSFX()
    {
        AudioManager.instance.UnMuteAllSFX();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(_musicSlider.value);

        musicVolumeText.text = (_musicSlider.value * 100).ToString("F0");
    }
    public void SFXAllVolume()
    {
        AudioManager.instance.SFXAllVolume(_sfxSlider.value);

        sfxVolumeText.text = (_sfxSlider.value * 100).ToString("F0");
    }

}
