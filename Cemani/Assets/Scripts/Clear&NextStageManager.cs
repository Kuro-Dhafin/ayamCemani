using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public List<GameObject> enemies;
    public string nextStageName;
    public GameObject stageClearUI;  // Referensi ke UI Stage Clear

    void Update()
    {
        CheckEnemies();
    }

    void CheckEnemies()
    {
        enemies.RemoveAll(item => item == null);

        if (enemies.Count == 0)
        {
            ShowStageClearUI();
        }
    }

    void ShowStageClearUI()
    {
        stageClearUI.SetActive(true);  // Menampilkan UI Stage Clear
    }

    public void LoadNextStage()  // button Next Stage
    {
        SceneManager.LoadScene(nextStageName);
    }
}
