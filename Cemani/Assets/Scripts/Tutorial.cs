using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI tutorialText;
    private int step = 0;

    void Start()
    {
        UpdateTutorialText();
    }

    void Update()
    {
        CheckInput();
    }

    void UpdateTutorialText()
    {
        switch (step)
        {
            case 0:
                tutorialText.text = "Gunakan WASD untuk bergerak.";
                break;
            case 1:
                tutorialText.text = "Tekan Klik Kiri untuk menembak.";
                break;
            case 2:
                tutorialText.text = "Tekan Space untuk roll dan menghindari serangan.";
                break;
            case 3:
                tutorialText.text = "Ambil item kesehatan untuk menambah darah, Maju dan Serang Musuh!";
                break;
            case 4:
                tutorialText.text = "Tutorial Selesai! Selamat bermain.";
                break;
        }
    }

    void CheckInput()
    {
        switch (step)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                {
                    step++;
                    UpdateTutorialText();
                }
                break;
            case 1:
                if (Input.GetMouseButtonDown(0))
                {
                    step++;
                    UpdateTutorialText();
                }
                break;
            case 2:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    step++;
                    UpdateTutorialText();
                }
                break;
            case 3:
                // Ini akan dihandle oleh OnTriggerEnter
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        if (other.gameObject.CompareTag("HealthItem") && step == 3)
        {
            Debug.Log("Health item collected, moving to next step.");
            step++;
            UpdateTutorialText();
            Destroy(other.gameObject);  // Hapus item kesehatan setelah diambil
        }
    }
}
