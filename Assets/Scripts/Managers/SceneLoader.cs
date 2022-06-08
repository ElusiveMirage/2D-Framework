using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : GenericSingleton<SceneLoader>
{
    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject transition;
    [SerializeField] private Image progressBar;

    //private bool loading;
    private float target;
    private string levelName;
    private AsyncOperation sceneLoad;

    private void Update()
    {
        if(sceneLoad != null)
        {
            target = sceneLoad.progress;
        }

        if(target == 0.9f)
        {
            target = 1f;
        }
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, Time.deltaTime);
    }

    public void LoadScene(string sceneName)
    {
        levelName = sceneName;
        StartCoroutine(LoadSceneCoroutine());
        //SceneManager.LoadSceneAsync(levelName);
    }

    public IEnumerator LoadSceneCoroutine() //for webgl builds
    {
        target = 0f;
        progressBar.fillAmount = 0f;

        sceneLoad = SceneManager.LoadSceneAsync(levelName);

        sceneLoad.allowSceneActivation = false;

        transition.SetActive(true);
        loadingScreen.SetActive(true);

        if (transition != null && transition.GetComponent<ScreenTransition>())
        {
            transition.GetComponent<ScreenTransition>().FadeInOut(1f);
        }

        target = sceneLoad.progress;
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, Time.deltaTime);

        yield return new WaitForSeconds(2);

        sceneLoad.allowSceneActivation = true;

        if (transition != null && transition.GetComponent<ScreenTransition>())
        {
            transition.GetComponent<ScreenTransition>().FadeInOut(1f);
        }

        loadingScreen.SetActive(false);

        // Wait until the asynchronous scene fully loads
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
    }

    //public IEnumerator LoadSceneCoroutine()
    //{
    //    target = 0f;
    //    progressBar.fillAmount = 0f;
    //    loading = true;

    //    var scene = SceneManager.LoadSceneAsync(levelName);

    //    scene.allowSceneActivation = false;

    //    transition.SetActive(true);
    //    loadingScreen.SetActive(true);

    //    if (transition != null && transition.GetComponent<ScreenTransition>())
    //    {
    //        transition.GetComponent<ScreenTransition>().FadeInOut(1f);
    //    }

    //    do
    //    {
    //        target = scene.progress;

    //    } while (scene.progress < 0.9f);

    //    target = 1f;      

    //    yield return new WaitForSeconds(2);

    //    scene.allowSceneActivation = true;

    //    if (transition != null && transition.GetComponent<ScreenTransition>())
    //    {
    //        transition.GetComponent<ScreenTransition>().FadeInOut(1f);
    //    }

    //    target = 0f;
    //    loading = false;
    //    loadingScreen.SetActive(false);

    //    // Wait until the asynchronous scene fully loads
    //    while (!scene.isDone)
    //    {
    //        yield return null;
    //    }
    //}

    //public async void LoadScene(string sceneName) //for when building only for pc
    //{
    //    target = 0f;
    //    progressBar.fillAmount = 0f;
    //    loading = true;

    //    var scene = SceneManager.LoadSceneAsync(sceneName);

    //    scene.allowSceneActivation = false;

    //    transition.SetActive(true);
    //    loadingScreen.SetActive(true);

    //    if (transition != null && transition.GetComponent<ScreenTransition>())
    //    {
    //        transition.GetComponent<ScreenTransition>().FadeInOut(1f);
    //    }

    //    do
    //    {
    //        await System.Threading.Tasks.Task.Delay(100);
    //        target = scene.progress;

    //    } while (scene.progress < 0.9f);

    //    target = 1f;

    //    await System.Threading.Tasks.Task.Delay(2000);

    //    scene.allowSceneActivation = true;

    //    if (transition != null && transition.GetComponent<ScreenTransition>())
    //    {
    //        transition.GetComponent<ScreenTransition>().FadeInOut(1f);
    //    }

    //    target = 0f;
    //    loading = false;
    //    loadingScreen.SetActive(false);
    //}
}
