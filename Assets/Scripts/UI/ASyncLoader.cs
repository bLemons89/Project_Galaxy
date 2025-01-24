using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject menuActive;

    [Header("Progress Bar")]
    [SerializeField] private Image loadingBar;

    /// <summary>
    /// Get from scene manager...?
    /// </summary>

    public void LoadLevelBtn(string sceneToLoad)
    {
        menuActive.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelASync(sceneToLoad));

    }

    IEnumerator LoadLevelASync(string sceneToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return null;
        }
    }

}
