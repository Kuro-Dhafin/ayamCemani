using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOver; // Assign the GameOverPanel in the Inspector
    private Bullet[] bullets;
    private PlayerMove playerMove;
    public string MainMenu;
    private POZE poze;

    void Start()
    {
        GameOver.SetActive(false); // Initially hide the game over panel
        bullets = FindObjectsOfType<Bullet>();
        playerMove = FindObjectOfType<PlayerMove>();
    }

    public void ShowGameOver()
    {
        GameOver.SetActive(true);
        Time.timeScale = 0;

        // Pause bullets
        foreach (Bullet bullet in bullets)
        {
            bullet.SetPauseState(true);
        }

        // Pause player movement and shooting
        playerMove.SetPause(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1; // Reload the current scene
    }

     public bool IsGameOverActive()
    {
        return GameOver.activeSelf;
    }
}
