using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform environmentalObject;

    [SerializeField]
    private GameObject playerPrefab = null;

    // Start is called before the first frame update
    private void Start()
    {
        SceneManager.sceneLoaded += LevelLoaded;
        LoadMissingReferences();
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

        SceneManager.LoadScene($"{outerRune.TargetLocation}");

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

    private void LevelLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.transform.position = spawnPoint.transform.position;

        GameObject cameraInstance = GameObject.FindGameObjectWithTag("MainCamera");
        cameraInstance.GetComponent<CameraFollowsPlayer>().Init();

        GameManager.Instance.UnpauseGame();
    }
}
