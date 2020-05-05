using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public void StartGame()
    {
        SlingShot.firstName = GameObject.Find("FirstName").GetComponent<InputField>().text;
        SlingShot.secondName = GameObject.Find("SecondName").GetComponent<InputField>().text;
        SceneManager.LoadScene("_Scene_0");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
