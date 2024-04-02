using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp.Assets.Scripts.Stargate;

using Unity.Loading;

using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [SerializeField]
    Transform environmentalObject;

    [SerializeField]
    GameObject playerPrefab = null;

    #region Singletone
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Game manager is null!");
                //instance = new GameManager();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    // Update is called once per frame
    void Update()
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
        GameObject environmentalModification = Instantiate(innerRune.EnvironmentalPrefab, environmentalObject);
        environmentalModification.SetActive(true);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.transform.position = spawnPoint.transform.position;

        GameObject cameraInstance = GameObject.FindGameObjectWithTag("MainCamera");
        cameraInstance.GetComponent<CameraFollowsPlayer>().Init();

        Time.timeScale = 1f;
    }
}
