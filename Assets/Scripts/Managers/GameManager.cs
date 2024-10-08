using UnityEngine;


[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(HudManager))]
public class GameManager : MonoBehaviour
{
    private LevelManager levelManager;
    public LevelManager LevelManager { get { return levelManager; } }

    private HudManager hudManager;
    public HudManager HudManager { get { return hudManager; } }

    private bool gameIsPaused = false;
    public bool playerIsInteracting = false;

    #region Singletone
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Game manager is null!");
            }
            return instance;
        }
    }

    private void Awake()
    {
        CreateSingleton();
        GetManagers();
    }

    private void CreateSingleton()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField]
    private GameObject pauseMenu;

    private void GetManagers()
    {
        levelManager = GetComponent<LevelManager>();
        hudManager = GetComponent<HudManager>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Pause") && !playerIsInteracting)
        {
            TogglePause();
        }
    }

    public void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1.0f;
    }

    private void TogglePause()
    {
        if (gameIsPaused)
        {
            UnpauseGame();
            pauseMenu.SetActive(false);
        }
        else
        {
            PauseGame();
            pauseMenu.SetActive(true);
        }
    }
}
