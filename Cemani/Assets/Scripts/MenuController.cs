using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
  public string Game;
  public string Options;
  public string Credits;
  public void CloseGame()
  {
    Application.Quit();

  }
  public void LoadScene()
  {
    SceneManager.LoadScene(Game, LoadSceneMode.Single);//Start Game
  }
  public void LoadOpt()
  {
    SceneManager.LoadScene(Options, LoadSceneMode.Single);//Buka Options
  }
  public void LoadCred()
  {
    SceneManager.LoadScene(Credits, LoadSceneMode.Single);//Buka Credits
  }
}
  