using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public Canvas main;
    public Canvas credits;
    public Canvas options;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Play()
    {
        SceneManager.LoadScene("CrystalMountain");
    }

    public void Options()
    {
        main.gameObject.SetActive(false);
        options.gameObject.SetActive(true);
    }

    public void BackOptions()
    {
        main.gameObject.SetActive(true);
        options.gameObject.SetActive(false);
    }

    public void Credits()
    {
        main.gameObject.SetActive(false);
        credits.gameObject.SetActive(true);
    }

    public void BackCredits()
    {
        main.gameObject.SetActive(true);
        credits.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
