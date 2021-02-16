using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject controlMenu;

    public void Play()
    {
        SceneManager.LoadScene("Stadium");
    }

    public void Controls()
    {
        controlMenu.SetActive(true);
        startMenu.SetActive(false);
    }

    public void BackToStart()
    {
        controlMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
