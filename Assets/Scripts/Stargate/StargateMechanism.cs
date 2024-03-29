using System;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp.Assets.Tools;

using TMPro;

using UnityEngine;
using UnityEngine.Tilemaps;

public class StargateMechanism : MonoBehaviour
{
    [SerializeField]
    Stargate outerStargate;

    [SerializeField]
    Stargate innerStargate;

    [SerializeField]
    TMP_Text targetText;

    bool canInteract = false;
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
        if (outerStargate.IsTurning || innerStargate.IsTurning)
            return;

        UpdateInput();
        HandleGateChange();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || !collision.transform.parent.CompareTag("Player"))
            return;

        canInteract = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || !collision.transform.parent.CompareTag("Player"))
            return;

        canInteract = false;
    }
    private void UpdateInput()
    {
        yInput = Input.GetAxis("Vertical");
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
            Debug.Log(innerRuneData.Modifications[i]);
        }
    }
}
