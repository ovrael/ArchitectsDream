using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform environmentalObject;

    [SerializeField]
    private GameObject playerPrefab = null;

    [SerializeField]
    private Animator canvasAnimator;

    // Start is called before the first frame update
    private void Start()
    {
        SceneManager.sceneLoaded += LevelLoaded;
        LoadMissingReferences();
        canvasAnimator.gameObject.SetActive(true);

        canvasAnimator.Play("Level Open");
    }

    private void LoadMissingReferences()
    {
        if (playerPrefab == null)
            playerPrefab = GameObject.FindGameObjectWithTag("Player");

        if (environmentalObject == null)
            environmentalObject = GameManager.Instance.transform.Find("Environmental");
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void ChangeLevel(OuterRuneData outerRune, InnerRuneData innerRune)
    {
        Scene nextScene = SceneManager.GetSceneByName($"{outerRune.TargetLocation}");
        if (nextScene == null)
        {
            Debug.LogError($"Cannot load next level, {nextScene.name} scene does not exist!");
            return;
        }

        canvasAnimator.Play("Level Close");

        StartCoroutine(LoadYourAsyncScene(outerRune.TargetLocation));

        //SceneManager.LoadScene($"{outerRune.TargetLocation}");

        for (var i = environmentalObject.childCount - 1; i >= 0; i--)
        {
            Destroy(environmentalObject.GetChild(i).gameObject);
        }

        if (innerRune.EnvironmentalPrefab == null)
        {
            return;
        }

        GameObject environmentalModification = Instantiate(innerRune.EnvironmentalPrefab, environmentalObject);
        environmentalModification.SetActive(true);
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        yield return new WaitForSecondsRealtime(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private void LevelLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.transform.position = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, -8.0f);

        GameObject cameraInstance = GameObject.FindGameObjectWithTag("MainCamera");
        cameraInstance.GetComponent<CameraFollowsPlayer>().Init();

        GameManager.Instance.UnpauseGame();
        canvasAnimator.Play("Level Open");
    }
}
