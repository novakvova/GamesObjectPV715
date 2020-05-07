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
        SlingShot.isFire = GameObject.Find("cb_Toggle").GetComponent<Toggle>().isOn;
        if (!SlingShot.isFire)
        {
            SlingShot.firstName = GameObject.Find("FirstName").GetComponent<InputField>().text;
            SlingShot.secondName = GameObject.Find("SecondName").GetComponent<InputField>().text;
        }
        else
        {
            SlingShot.firstName = GameObject.Find("SecondName").GetComponent<InputField>().text;
            SlingShot.secondName = GameObject.Find("FirstName").GetComponent<InputField>().text;
        }
        SceneManager.LoadScene("_Scene_0");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
