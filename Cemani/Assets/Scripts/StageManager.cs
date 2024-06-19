using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public float displayTime = 2f; // Time to display the "Stage Clear" message
    public GameObject stageClearTextPrefab; // Reference to the "Stage Clear" prefab
    private List<GameObject> enemies = new List<GameObject>();
    private bool stageCleared = false;
    private GameObject stageClearPrefabInstance; // To store the instantiated prefab instance

    void Start()
    {
        AddEnemiesWithTag("Enemy");
        AddEnemiesWithTag("EnemyWarga2");
    }

    void Update()
    {
        if (stageCleared)
            return;

        if (IsStageCleared())
        {
            ShowStageClear();
            StartCoroutine(StageClearRoutine());
        }
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    private void AddEnemiesWithTag(string tag)
    {
        if (TagHelper.TagExists(tag))
        {
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemyObjects)
            {
                enemies.Add(enemy);
            }
        }
        else
        {
            Debug.LogWarning($"Tag: {tag} is not defined.");
        }
    }

    private bool IsStageCleared()
    {
        return enemies.Count == 0;
    }

    public void ShowStageClear()
    {
        // Instantiate the "Stage Clear" prefab
        stageClearPrefabInstance = Instantiate(stageClearTextPrefab, transform);

        // Get the TextMeshPro component from the instantiated prefab
        TextMeshProUGUI stageClearTextComponent = stageClearPrefabInstance.GetComponentInChildren<TextMeshProUGUI>();

        // Activate the TextMeshPro component
        stageClearTextComponent.gameObject.SetActive(true);
    }

    private IEnumerator StageClearRoutine()
    {
        stageCleared = true;

        // Wait for the specified display time
        yield return new WaitForSeconds(displayTime);

        // Destroy the "Stage Clear" instance
        Destroy(stageClearPrefabInstance);

        // Load the next stage
        LoadNextStage();
    }

    private void LoadNextStage()
    {
        // Load the next scene, assuming scenes are named in the format "Stage1", "Stage2", etc.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
