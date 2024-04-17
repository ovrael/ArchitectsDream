using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void Awake()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManager != null)
        {
            DestroyImmediate(gameManager);
        }
        Time.timeScale = 1.0f;
    }

    public void StartGame()
    {
        animator.Play("Load Game");
        StartCoroutine(LoadGameAsync());
    }

    IEnumerator LoadGameAsync()
    {
        yield return new WaitForSecondsRealtime(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainHub");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


    public void CloseGame()
    {
        Application.Quit();
    }
}