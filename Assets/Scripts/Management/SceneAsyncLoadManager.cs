using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Management
{
public class SceneAsyncLoadManager : MonoBehaviour
{
    private static string _sceneName;
    public Slider loadingProgressBar;
    
    public static void SetSceneName(string sceneName)
    {
        _sceneName = sceneName;
    }
    private void Start()
    {
        StartCoroutine(SceneLoader());
    }

    private IEnumerator SceneLoader()
    {
        yield return null;
        var asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;
            loadingProgressBar.value = loadingProgressBar.value switch
            {
                < 0.9f => Mathf.MoveTowards(loadingProgressBar.value, 0.9f, Time.deltaTime),
                >= 0.9f => Mathf.MoveTowards(loadingProgressBar.value, 1f, Time.deltaTime),
                _ => loadingProgressBar.value
            };

            if (loadingProgressBar.value >= 1f)
            {
                asyncLoad.allowSceneActivation = true;
            }
        }
    }
}
}