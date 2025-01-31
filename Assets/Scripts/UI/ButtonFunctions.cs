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

    /* 
     * Tried to split the menus and the buttons into separate classes, but unity is being hard-headed     
     */
public class ButtonFunctions : MonoBehaviour
{
    //SceneManagerScript sceneManager;
    //playerScript PlayerScript;

    [Header("===== MENUS =====")]
    // Background Screen //
        [SerializeField] GameObject backgroundScreen;
        private CanvasGroup backgroundGroup;
        public GameObject BackgroundScreen
        { get => backgroundScreen; set => backgroundScreen = value; }

    // Pause Menu //
        [SerializeField] GameObject pauseMenu;
        private CanvasGroup pauseGroup;
        public GameObject PauseMenu
        { get => pauseMenu; set => pauseMenu = value; }

        [SerializeField] GameObject pauseTitle;
        private TextMeshProUGUI titleText;
        public GameObject PauseTitle
        { get => pauseTitle; set => pauseTitle = value; }
    

        [SerializeField] GameObject pauseButtons;
        public GameObject PauseButtons
        { get => pauseButtons; set => pauseButtons = value; }

    // Settings Menu //
        [SerializeField] GameObject settingsMenu;
        private CanvasGroup settingsGroup;
        public GameObject SettingsMenu
        { get => settingsMenu; set => settingsMenu = value; }

    // Controls Menu //
        [SerializeField] GameObject controlsMenu;
        private CanvasGroup controlsGroup;
        public GameObject ControlsMenu
        { get => controlsMenu; set => controlsMenu = value; }

    // Inventory Menu //
        [SerializeField] GameObject inventoryMenu;
        private CanvasGroup inventoryGroup;
        public GameObject InventoryMenu
        { get => inventoryMenu; set => inventoryMenu = value; }

    // Save Menu //
        [SerializeField] GameObject saveMenu;
        private CanvasGroup saveGroup;
        private TextMeshProUGUI timeDateStamp;
        public GameObject SaveMenu
        { get => saveMenu; set => saveMenu = value; }
        public TextMeshProUGUI TimeDateStamp
        { get => timeDateStamp; set => timeDateStamp = value; }

    // Radial Menu //
    //RadialMenu radialMenu;
    //[SerializeField] GameObject radialMenuObject;


    // Credit Menu //
    [SerializeField] GameObject creditScreen;
        private CanvasGroup creditGroup;
        public GameObject CreditScreen
        { get => creditScreen; set => creditScreen = value; }

    // For Win //
        [SerializeField] GameObject winScreen;

    // For Lose //
        [SerializeField] GameObject loseScreen;

    // Loading //
        [SerializeField] GameObject loadingScreen;

    // Flags //

    

    [Header("===== Settings =====")]
    [SerializeField] private bool enableFlickering;

    [SerializeField] TextMeshProUGUI musicVolumeText;
    [SerializeField] TextMeshProUGUI sfxVolumeText;

    public UnityEngine.UI.Slider _musicSlider, _sfxSlider, _playerSFXSlider;

    

    public void Start()
    {        
        //radialMenu = FindObjectOfType<RadialMenu>();
        // Background Screen
        backgroundGroup = backgroundScreen.GetComponent<CanvasGroup>();
        backgroundGroup.alpha = 0f;
        backgroundScreen.transform.localScale = Vector3.zero;

        // Pause
        pauseGroup = pauseMenu.GetComponent<CanvasGroup>();
        titleText = pauseTitle.GetComponent<TextMeshProUGUI>();
        
        // only if using fade
        settingsGroup = settingsMenu.GetComponent<CanvasGroup>();
        controlsGroup = controlsMenu.GetComponent<CanvasGroup>();
        inventoryGroup = inventoryMenu.GetComponent<CanvasGroup>();
        saveGroup = saveMenu.GetComponent<CanvasGroup>();
        creditGroup = creditScreen.GetComponent<CanvasGroup>();

        // Settings //
        _musicSlider = _musicSlider.GetComponent<UnityEngine.UI.Slider>();
        _sfxSlider = _musicSlider.GetComponent<UnityEngine.UI.Slider>();

        enableFlickering = true;
    }

   
    /* 
     * This is only for the pause menu settings button 
     * It is only pressed once, the Settings Menu button is different
     */
    public void SettingsButton()
    {
        if (GameManager.instance.MenuActive != null)
        {
            GameManager.instance.MenuActive.SetActive(false);
        }
        GameManager.instance.MenuActive = SettingsMenu;
        SettingsMenu.SetActive(true);
        PauseMenu.SetActive(false);
        backgroundGroup.alpha = 1f;
        RectTransform settingsTransform = SettingsMenu.GetComponent<RectTransform>();

        settingsTransform.DOAnchorPos(new Vector3(0, 0, 0), 0.25f)
                         .SetEase(Ease.InOutQuad);        
    }

    // Settings Menu Buttons //
    /* Placeholder on/off until animations added */
    public void SettingsMenuButton()
    {
        if(GameManager.instance.MenuActive != settingsMenu)
        {
            if (GameManager.instance.MenuActive != null)
            {
                GameManager.instance.MenuActive.SetActive(false);
            }
            GameManager.instance.MenuActive = settingsMenu;
            GameManager.instance.MenuActive.SetActive(true);
        }
    }
    public void ControlsMenuButton()
    {
        if(GameManager.instance.MenuActive != controlsMenu)
        { 
            GameManager.instance.MenuActive.SetActive(false);
            GameManager.instance.MenuActive = controlsMenu;
            GameManager.instance.MenuActive.SetActive(true);
        }
        //controlsMenu.SetActive(true);
        //controlsPos.DOAnchorPos(new Vector2(0, 0), 0.25f)
        //           .SetEase(Ease.OutQuad)
        //           .SetUpdate(true);
    }
    public void InventoryMenuButton()
    {
        if (GameManager.instance.MenuActive != inventoryMenu)
        {
            GameManager.instance.MenuActive.SetActive(false);
            GameManager.instance.MenuActive = inventoryMenu;
            GameManager.instance.MenuActive.SetActive(true);
        }
    }
    public IEnumerator SaveMenuButton()
    {
        if (GameManager.instance.MenuActive != saveMenu)
        {
            if(GameManager.instance.MenuActive != null)
            { 
                GameManager.instance.MenuActive.SetActive(false); 
            }
            GameManager.instance.MenuActive = saveMenu;
            GameManager.instance.MenuActive.SetActive(true);
        }

        backgroundGroup.alpha = 1f;
        RectTransform saveTransform = saveMenu.GetComponent<RectTransform>();

        Vector3 startPos = saveTransform.anchoredPosition;
        Vector3 endPos = new Vector3(0, 0, 0);
        float duration = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp the position over time
            saveTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);

            // Increase elapsed time by time passed
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure it ends exactly at the target position
        saveTransform.anchoredPosition = endPos;

        //saveTransform.DOAnchorPos(new Vector3(0, 0, 0), 0.25f)
        //             .SetEase(Ease.InOutQuad);

    }
    public void CloseButton()
    {
        CloseAllMenus();
    }
    public void CreditsButton()
    {
        if (GameManager.instance.MenuActive != creditScreen)
        {
            if (GameManager.instance.MenuActive != null)
            {
                GameManager.instance.MenuActive.SetActive(false);
            }
            GameManager.instance.MenuActive = creditScreen;
            GameManager.instance.MenuActive.SetActive(true);
        }
        backgroundGroup.alpha = 1f;
        RectTransform creditsTransform = creditScreen.GetComponent<RectTransform>();

        creditsTransform.DOAnchorPos(new Vector3(0, 0, 0), 0.25f)
                         .SetEase(Ease.InOutQuad);
    }
    
   
    // Screens //
    public void OpenPauseMenu()
    {
        GameManager.instance.MenuActive = PauseMenu;
        
        DOTween.KillAll();
        if (!backgroundScreen.activeSelf)
        {
            //Turn on
            backgroundScreen.SetActive(true);
            backgroundGroup.alpha = 0f;
            backgroundScreen.transform.localScale = Vector3.zero;
                
            //Scale/Fade
            backgroundScreen.transform.DOScale(Vector3.one, 0.25f)
                            .SetEase(Ease.InOutQuad);
            backgroundGroup.DOFade(0.98f, 0.25f);
            
        }
        else
        {
            backgroundGroup.alpha = 0.98f;
        }

        //Turn on
        pauseMenu.SetActive(true);
        pauseGroup.alpha = 0f;
        pauseMenu.transform.localScale = Vector3.zero;
        
        //Scale/Fade
        pauseMenu.transform.DOScale(Vector3.one, 0.5f)
                           .SetEase(Ease.InOutBack);
        pauseGroup.DOFade(1f, .5f);

        //Turn on
        pauseTitle.SetActive(true);
        titleText.alpha = 0f;
        pauseTitle.transform.localScale = Vector3.zero;

        //Scale/Fade
        pauseTitle.transform.DOScale(Vector3.one, 0.25f)
                    .SetEase(Ease.InOutQuad);

        titleText.DOFade(1f, 1f)
                    .SetEase(Ease.InOutElastic);

        titleText.DOFade(0.75f, 0.15f)
                    .SetEase(Ease.InOutExpo)
                    .SetLoops(-1, LoopType.Restart)
                    .SetDelay(0.5f);
        float randomDelay = Random.Range(0.5f, 1.5f);
        titleText.DOFade(1f, Random.Range(0.1f, 0.5f))
                 .SetEase(Ease.InOutElastic)
                 .SetDelay(randomDelay);
        
        ButtonGroupOpen(pauseButtons);
        
    }
    public void ClosePauseMenu()
    {
        DOTween.KillAll();

        if (GameManager.instance.MenuActive == pauseMenu)
        {
            ButtonGroupClose(pauseButtons);

            //Scale/Fade
            pauseTitle.transform.DOScale(Vector3.zero, 0.25f)
                      .SetEase(Ease.InOutQuad);
            //Turn off
            pauseTitle.SetActive(false);
            pauseMenu.transform.DOScale(Vector3.zero, 0.5f)
                               .SetEase(Ease.InOutBack);
            pauseGroup.DOFade(1f, .5f)
                      .OnComplete(() =>
                      {
                          pauseMenu.SetActive(false);
                          CloseBackgroundScreen();
                          GameManager.instance.MenuActive = null;
                      });
        }

        if (GameManager.instance.MenuActive == null)
        {
            CloseBackgroundScreen();
        }
       
    }
    public void CloseBackgroundScreen()
    {
        if (backgroundScreen.activeSelf)
        {
            backgroundScreen.transform
                     .DOScale(Vector3.zero, 0.5f)
                     .SetEase(Ease.InOutQuad);

            backgroundGroup.DOFade(0f, 0.5f)
                           .OnComplete(() =>
                           {
                               backgroundScreen.SetActive(false);
                           });
        }
    }
    public void ButtonGroupOpen(GameObject buttonParent)
    {
        //Animate in
        float totalDelay = .5f;
        float delayBetween = 0.15f;
        float randomDelay = Random.Range(0.5f, 1.5f);

        for (int i = 0; i < buttonParent.transform.childCount; i++)
        {
            GameObject button = buttonParent.transform.GetChild(i).gameObject;

            button.SetActive(true);
            button.transform.localScale = Vector3.zero;

            button.transform.DOScale(Vector3.one, 0.15f)
                            .SetEase(Ease.InOutQuad)
                            .SetDelay(totalDelay);

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            
            // Toggle Setting //
            if (enableFlickering)
            {
                buttonText.DOFade(0.75f, 0.15f)
                 .SetEase(Ease.InOutExpo)
                 .SetLoops(-1, LoopType.Restart)
                 .SetDelay(0.5f);

                buttonText.DOFade(1f, Random.Range(0.1f, 0.5f))
                          .SetEase(Ease.InOutElastic)
                          .SetDelay(randomDelay);
            }
            totalDelay += delayBetween;
        }
        
    }
    public void ButtonGroupClose(GameObject buttonParent)
    {
        for (int i = 0; i < buttonParent.transform.childCount; i++)
        {
            GameObject button = buttonParent.transform.GetChild(i).gameObject;

            button.transform.DOScale(Vector3.zero, 0.15f)
                            .SetEase(Ease.InOutQuad)
                            .SetDelay(0.15f);

            button.SetActive(false);
        }
    }
    public void WinScreen()
    {
     
    }
    public void LoseScreen()
    {
     
    }
    // Maybe not needed...
    public void LoadingScreen()
    {
        
    }
    public void SettingsMenuReset()
    {
        RectTransform settingsTransform = settingsMenu.GetComponent<RectTransform>();
        settingsTransform.DOAnchorPos(new Vector3(-1983, 0, 0), 0.25f).SetEase(Ease.InOutQuad);
        settingsMenu.SetActive(false);
    }
    public IEnumerator SaveMenuReset()
    {
        RectTransform saveTransform = saveMenu.GetComponent<RectTransform>();

        Vector3 startPos = saveTransform.anchoredPosition;
        Vector3 endPos = new Vector3(-1983, 0, 0);
        float duration = 0.25f;
        float elapsedTime = 0f;

        // Move the position over time
        while (elapsedTime < duration)
        {
            saveTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it ends exactly at the target position
        saveTransform.anchoredPosition = endPos;

        // Deactivate the save menu after the movement
        saveMenu.SetActive(false);
    }
    public void CreditsReset()
    {
        RectTransform creditsTransform = creditScreen.GetComponent<RectTransform>();
        creditsTransform.DOAnchorPos(new Vector3(-1983, 0, 0), 0.25f).SetEase(Ease.InOutQuad);
        creditScreen.SetActive(false);
    }
    public void CloseAllMenus()
    {
        DOTween.KillAll();
        if (settingsMenu.activeSelf)
        {
            SettingsMenuReset();
        }
        else if(saveMenu.activeSelf)
        {
            StartCoroutine(SaveMenuReset());
        }
        else if(creditScreen.activeSelf)
        {
            CreditsReset();
        }
        
        if(GameManager.instance.MenuActive == pauseMenu || PauseMenu.activeSelf)
        {
            ClosePauseMenu();
        }

        if(backgroundScreen.activeSelf)
        {
            CloseBackgroundScreen();
        }

        //reset variable
        GameManager.instance.MenuActive = null;
    }

    // Settings //
        // Toggle //
        public void ToggleFlicker()
        {
            enableFlickering = !enableFlickering;
        }
        public void ToggleMusic()
        {
            //AudioManager.instance.ToggleMusic();
        }
        public void TogglePlayerSFX()
        {
            //AudioManager.instance.TogglePlayerSFX();
        }
        public void MuteAllSFX()
        {
            //AudioManager.instance.MuteAllSFX();
        }
        public void UnMuteAllSFX()
        {
            //AudioManager.instance.UnMuteAllSFX();
        }
    
        // Slider //
        public void MusicVolume()
        {
            //AudioManager.instance.MusicVolume(_musicSlider.value);

            musicVolumeText.text = (_musicSlider.value * 100).ToString("F0");
        }
        //public void SFXAllVolume()
        //{
        //    AudioManager.instance.SFXAllVolume(_sfxSlider.value);

        //    sfxVolumeText.text = (_sfxSlider.value * 100).ToString("F0");
        //}
}
