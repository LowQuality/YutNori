using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
public class ButtonManager : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("CharacterSelector");
    }
    
    public void Retry()
    {
        SceneAsyncLoadManager.SetSceneName("Main");
        SceneManager.LoadScene("Loading");
    }
    
    public void Replay()
    {
        SceneAsyncLoadManager.SetSceneName("Replay");
        SceneManager.LoadScene("Loading");
    }

    public void Helper()
    {
        SceneManager.LoadScene("Helper");
    }
}
}