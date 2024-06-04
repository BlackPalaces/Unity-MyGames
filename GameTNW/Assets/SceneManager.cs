using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public string currentScene;
    public void LoadMap()
    { 
    }
    public void LoadMap3(string sceneName)
    {
        SceneManager.LoadScene("Map3");
    }
    public void LoadMap2(string sceneName)
    {
        SceneManager.LoadScene("Map2");
    }
    public void LoadMap1(string sceneName)
    {
        SceneManager.LoadScene("Map1");
    }
    public void LoadFinal(string sceneName)
    {
        SceneManager.LoadScene("MapFinal");
    }
    public void LoadEdHome(string sceneName)
    {
        SceneManager.LoadScene("EdHome");
    }
    public void Home(string sceneName)
    {
        SceneManager.LoadScene("Home");
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
