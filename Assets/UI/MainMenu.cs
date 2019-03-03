using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // from build queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);

        // file/ build settings/ add scenes to queue
    }

    public void QuitGame()
    {
        Debug.Log("meesa give up");
        Application.Quit();
    }
}
