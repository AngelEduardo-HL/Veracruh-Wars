using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByIndex(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void ReloadCurrent()
    {
        int idx = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(idx);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
        Application.Quit();
    }
}
