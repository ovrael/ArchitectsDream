using AssemblyCSharp.Assets.Scripts.Stargate;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StargateMechanism : MonoBehaviour
{
    [SerializeField]
    Stargate outerStargate;

    [SerializeField]
    Stargate innerStargate;

    [SerializeField]
    TMP_Text targetText;

    [SerializeField]
    TMP_Text interactText;

    bool userInRange = false;
    bool userInteract = false;
    float yInput;

    private void Awake()
    {

        outerStargate.updateStargateText = UpdateTargetText;
        innerStargate.updateStargateText = UpdateTargetText;

        outerStargate.Activate();
        innerStargate.Deactivate();

        UpdateTargetText();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
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

            GameManager.Instance.ChangeLevel(activeOuterRune, activeInnerRune);
        }


        UpdateInput();
        HandleGateChange();
    }

    private void UserInteraction(bool state)
    {
        userInteract = state;
        Time.timeScale = state ? 0f : 1f;
        interactText.gameObject.SetActive(!state);
        outerStargate.ChangeUserInteraction(state);
        innerStargate.ChangeUserInteraction(state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.transform.parent.CompareTag("Player"))
            return;

        userInRange = true;
        interactText.gameObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || !collision.transform.parent.CompareTag("Player"))
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

        targetText.text = $"Rune: {outerRuneData.RuneName} - {innerRuneData.RuneName}";
        targetText.text += $"\nTarget location: {outerRuneData.TargetLocation}";
        targetText.text += $"\nModifications:";
        for (int i = 0; i < innerRuneData.Modifications.Length; i++)
        {
            targetText.text += $"\n* {innerRuneData.Modifications[i]}";
        }
    }
}
