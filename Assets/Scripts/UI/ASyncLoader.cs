using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    ParticleUI loadingParticle;

    [Header("Progress Bar")]
    [SerializeField] private Image loadingBar;

    public GameObject LoadingScene
    { get; private set; }
    public Image LoadingBar
    { get => loadingBar; set => loadingBar = value; }

    /// <summary>
    /// Get from scene manager...?
    /// </summary>   


    public void LoadLevelBtn(string sceneToLoad)
    {
        GameManager.instance.MenuActive.SetActive(false);
        loadingScreen.SetActive(true);
        
        StartCoroutine(LoadLevelASync(sceneToLoad));
    }

    IEnumerator LoadLevelASync(string sceneToLoad)
    {
        if (!loadingScreen.activeSelf)
        {            
            loadingScreen.SetActive(true);
        }
        
        if (loadingParticle == null)
        {
            loadingParticle = GetComponentInChildren<ParticleUI>();
        }
        loadingParticle.PlayParticles();
        Debug.Log("Particle active ? " + loadingScreen.activeSelf);
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return null;
        }

        loadingParticle.StopParticles();        
        if (loadingScreen.activeSelf)
        { 
            loadingScreen.SetActive(false); 
        }
        Debug.Log("Loading Complete!");
    }
}
