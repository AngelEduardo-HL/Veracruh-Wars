using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int score = 0;
    [SerializeField] private int lives = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        lives = 3;
        score = 0;
    }

    public void PlayerKilled()
    {
        lives--;

        if (lives <= 0)
        {
            GameOver();
            return;
        }

        SceneManager sm = FindFirstObjectByType<SceneManager>();
        if (sm != null) sm.ReloadCurrent();
        else UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    public void GameOver()
    {
        SceneManager sm = FindFirstObjectByType<SceneManager>();
        if (sm != null) sm.LoadSceneByName("Menu");
        else Debug.Log("GAME OVER");
    }
}
