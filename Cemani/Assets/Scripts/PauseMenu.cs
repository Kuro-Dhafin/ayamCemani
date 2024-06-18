using UnityEngine;
using UnityEngine.SceneManagement;

public class POZE : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    public string MainMenu;
    bool isGamePaused = false;

    private Bullet[] bullets;
    private PlayerMove playerMove;
    private GameOverMenu gameOverMenu; // Reference to the GameOverMenu script

    void Start()
    {
        PauseMenu.SetActive(false);
        bullets = FindObjectsOfType<Bullet>();
        playerMove = FindObjectOfType<PlayerMove>();  // Find the PlayerMove script in the scene
        gameOverMenu = FindObjectOfType<GameOverMenu>(); // Find the GameOverMenu script in the scene
    }

    public void pause()
    {
        if (gameOverMenu.IsGameOverActive()) // Check if game over screen is active
        {
            return;
        }

        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;

        // Pause bullets
        foreach (Bullet bullet in bullets)
        {
            bullet.SetPauseState(true);
        }

        // Pause player movement and shooting
        playerMove.SetPause(true);
    }

    public void quit()
    {
        SceneManager.LoadScene(MainMenu, LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;

        // Resume bullets
        foreach (Bullet bullet in bullets)
        {
            bullet.SetPauseState(false);
        }

        // Resume player movement and shooting
        playerMove.SetPause(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }
}
