using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
  public string Menu;
  public void LoadMenu()
  {
    SceneManager.LoadScene(Menu, LoadSceneMode.Single);//BaLIK Ke Menu
  }
}
  