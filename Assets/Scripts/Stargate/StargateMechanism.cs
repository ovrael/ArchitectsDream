using TMPro;

using UnityEngine;

public class StargateMechanism : MonoBehaviour
{
    [SerializeField]
    private Stargate outerStargate;

    [SerializeField]
    private Stargate innerStargate;

    [SerializeField]
    private TMP_Text dataText;

    [SerializeField]
    private TMP_Text outerRuneText;

    [SerializeField]
    private TMP_Text innerRuneText;

    [SerializeField]
    private TMP_Text interactText;

    private bool userInRange = false;
    private bool userInteract = false;
    private float yInput;

    private void Awake()
    {
        dataText.transform.parent.gameObject.SetActive(false);
        outerRuneText.transform.parent.gameObject.SetActive(false);
        innerRuneText.transform.parent.gameObject.SetActive(false);

        outerStargate.updateStargateText = UpdateTargetText;
        innerStargate.updateStargateText = UpdateTargetText;

        outerStargate.Activate();
        innerStargate.Deactivate();

        UpdateTargetText();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

        if (!userInteract)
        {
            if (!userInRange)
                return;

            if (Input.GetButtonDown("Interact"))
            {
                UserInteraction(true);
                return;
            }
        }

        if (outerStargate.IsTurning || innerStargate.IsTurning || !userInteract)
            return;

        if (Input.GetButtonDown("Cancel"))
        {
            UserInteraction(false);
            return;
        }

        if (Input.GetButtonDown("Submit"))
        {
            OuterRuneData activeOuterRune = outerStargate.GetActiveRuneData<OuterRuneData>();
            InnerRuneData activeInnerRune = innerStargate.GetActiveRuneData<InnerRuneData>();

            GameManager.Instance.LevelManager.ChangeLevel(activeOuterRune, activeInnerRune);
        }


        UpdateInput();
        HandleGateChange();
    }

    private void UserInteraction(bool state)
    {
        userInteract = state;
        interactText.gameObject.SetActive(!state);
        outerStargate.ChangeUserInteraction(state);
        innerStargate.ChangeUserInteraction(state);
        dataText.transform.parent.gameObject.SetActive(state);
        outerRuneText.transform.parent.gameObject.SetActive(state);
        innerRuneText.transform.parent.gameObject.SetActive(state);
        GameManager.Instance.playerIsInteracting = state;

        if (state)
            GameManager.Instance.PauseGame();
        else
            GameManager.Instance.UnpauseGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null)
            return;

        if (collision.transform.parent == null)
            return;

        if (!collision.transform.parent.CompareTag("Player"))
            return;

        userInRange = true;
        interactText.gameObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null)
            return;

        if (collision.transform.parent == null)
            return;

        if (!collision.transform.parent.CompareTag("Player"))
            return;

        userInRange = false;
        interactText.gameObject.SetActive(false);
    }
    private void UpdateInput()
    {
        //yInput = Input.GetAxis("Vertical");
        if (Input.GetButton("Up"))
            yInput = 1f;
        else if (Input.GetButton("Down"))
            yInput = -1f;
        else
            yInput = 0f;
    }
    private void HandleGateChange()
    {
        if (yInput == 0)
            return;

        if (yInput > 0)
        {
            innerStargate.Deactivate();
            outerStargate.Activate();
        }

        if (yInput < 0)
        {
            outerStargate.Deactivate();
            innerStargate.Activate();
        }
    }

    public void UpdateTargetText()
    {
        OuterRuneData outerRuneData = outerStargate.GetActiveRuneData() as OuterRuneData;
        InnerRuneData innerRuneData = innerStargate.GetActiveRuneData() as InnerRuneData;

        outerRuneText.text = outerRuneData.RuneName;
        innerRuneText.text = innerRuneData.RuneName;
        dataText.text = $"\nTarget location: {outerRuneData.TargetLocation}";
        if (innerRuneData.Description.Length > 0)
            dataText.text += $"\nEnvironment: {innerRuneData.Description}";
        dataText.text += $"\nModifications:";
        for (int i = 0; i < innerRuneData.Modifications.Length; i++)
        {
            dataText.text += $"\n* {innerRuneData.Modifications[i]}";
        }
    }
}
