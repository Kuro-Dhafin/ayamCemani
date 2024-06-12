using UnityEngine;
using UnityEngine.SceneManagement;

public class POZE : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    public string MainMenu;
    bool isGamePaused = false;

    private Bullet[] bullets;
    private PlayerMove playerMove;

    void Start()
    {
        PauseMenu.SetActive(false);
        bullets = FindObjectsOfType<Bullet>();
        playerMove = FindObjectOfType<PlayerMove>();  // Find the PlayerMove script in the scene
    }

    public void pause()
    {
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                resume();
                Debug.Log("Game should NOT be paused rn");
            }
            else
            {
                pause();
                Debug.Log("Game should be paused rn");
            }
        }
    }
}
