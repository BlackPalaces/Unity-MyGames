using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadMap3(string sceneName)
    {
        SceneManager.LoadScene("Map3");
    }
    public void LoadHome(string sceneName)
    {
        SceneManager.LoadScene("Home");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
