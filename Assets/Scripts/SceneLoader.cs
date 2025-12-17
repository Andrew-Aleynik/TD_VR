using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("New Scene");
    }

    public void LoadTops()
    {
        SceneManager.LoadScene("Tops");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
