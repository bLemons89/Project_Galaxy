using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEditor;

public class ButtonFunctions : MonoBehaviour
{
    //public RectTransform settingsPos, controlPos;

    // Pause Menu Buttons //
    public void Resume()
    {
        GameManager.instance.StateUnPause();
    }

    //Restart
    //Respawn the player at last checkpoint...how to handle items?


    //Settings
    public void SettingsButton()
    {
        CanvasGroup settingsGroup = GameManager.instance.SettingsGroup;
        settingsGroup.gameObject.SetActive(true);
        //settingsGroup.alpha = 0f;
        //settingsGroup.transform
        //           .DOScale(Vector3.one, 0.5f)
        //           .SetEase(Ease.OutBack)
        //           .SetUpdate(true)
        //           .OnStart(() =>
        //           {
        //               settingsGroup.DOFade(1f, 0.25f)
        //               .SetUpdate(true);
        //           });
    }

    //Save Game
    //Navigate to Save/Load Screen

    //Main Menu
    //Navigate to Main Menu Scene

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Settings Menu Buttons //
    public void SoundButton()
    {
        //controlPos.DOAnchorPos(new Vector2(1989, 0), 0.25f);
        //soundPos.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }
    public void ControlsButton()
    {
        //soundPos.DOAnchorPos(new Vector2(-1989, 0), 0.25f);
        //controlPos.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void BackgroundGroupOpen()
    {
        // zoom and fade in
        GameManager.instance.BackgroundScreen.transform.DOScale(Vector3.one, 0.5f)
                   .SetEase(Ease.OutBack)
                   .SetUpdate(true);
        GameManager.instance.BackgroundScreenGroup.DOFade(0.98f, 0.25f)
        .SetUpdate(true);

    }
    public void BackgroundGroupClose()
    {
        // Kill any previous animations to prevent conflicts
        DOTween.Kill(GameManager.instance.BackgroundScreen); 

        GameManager.instance.BackgroundScreen.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad).SetUpdate(true);
        GameManager.instance.BackgroundScreenGroup.DOFade(0f, 0.25f).SetUpdate(true).OnKill(() =>
        {
            // Only deactivate the background after the animations are finished
            GameManager.instance.BackgroundScreen.SetActive(false);
        });
    }
}
