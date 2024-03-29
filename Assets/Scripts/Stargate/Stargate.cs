using System;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp.Assets.EditorUtils;
using AssemblyCSharp.Assets.Scripts.Stargate;

using UnityEngine;
using UnityEngine.U2D;

public class Stargate : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] Color activeColor = new Color(0.8f, 0f, 0f);
    [SerializeField] Color inactiveColor = new Color(0.2f, 0f, 0f);

    [Header("Runes")]
    [SerializeField] GameObject runePrefab;
    [SerializeField] Transform runesParent;
    [SerializeField] Transform runeSpawnPoint;
    [SerializeField, Range(4, 7)] int minRunes = 5;
    [SerializeField, Range(5, 10)] int maxRunes = 10;
    [SerializeReference] List<RuneData> runesData = new List<RuneData>();

    public delegate void UpdateStargateText();
    public UpdateStargateText updateStargateText;


    List<GameObject> runes = new List<GameObject>();
    List<int> usedRunes = new List<int>();

    float rotationAngle;
    float rotateDuration = 0.5f;

    bool isTurning = false;
    bool isActive = false;
    SpriteRenderer sprite;

    float xInput = 0;
    int activeRuneIndex = 0;

    public bool IsTurning { get { return isTurning; } }

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        activeRuneIndex = 0;
        CreateRunes();
    }


    private void Update()
    {
        if (isTurning || !isActive)
            return;

        xInput = Input.GetAxis("Horizontal");
        HandleGateRotation();

    }

    private void HandleGateRotation()
    {
        if (xInput == 0)
            return;

        StartCoroutine(ChangeActiveRune());
        StartCoroutine(RotateGate());
    }

    public void Activate()
    {
        isActive = true;
        sprite.color = activeColor;
    }

    public void Deactivate()
    {
        isActive = false;
        sprite.color = inactiveColor;
    }

    private void CreateRunes()
    {
        if (maxRunes < minRunes)
            maxRunes = minRunes + 1;

        int runesCount = UnityEngine.Random.Range(minRunes, maxRunes);
        rotationAngle = 360.0f / runesCount;

        for (int i = 0; i < runesCount; i++)
        {
            GameObject rune = Instantiate(runePrefab, runeSpawnPoint.position, Quaternion.identity, runesParent);
            rune.transform.RotateAround(transform.position, new Vector3(0, 0, 1), -rotationAngle * i);
            rune.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = RandomRuneData(usedRunes).Sprite;
            rune.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 1f);
            runes.Add(rune);
        }
        runes[activeRuneIndex].transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2f, 1f, 0.2f);
    }

    private RuneData RandomRuneData(List<int> usedRunes)
    {
        int runeIndex = UnityEngine.Random.Range(0, runesData.Count);
        while (usedRunes.Contains(runeIndex))
        {
            runeIndex = UnityEngine.Random.Range(0, runesData.Count);
        }

        usedRunes.Add(runeIndex);

        return runesData[runeIndex];
    }

    public RuneData GetActiveRuneData()
    {
        return runesData[usedRunes[activeRuneIndex]];
    }

    float EaseInOut(float t)
    {
        return Mathf.Sin(t * Mathf.PI * 0.5f);
    }

    IEnumerator ChangeActiveRune()
    {
        // Disable current active rune
        int oldIndex = activeRuneIndex;
        GameObject activeRune = runes[oldIndex];
        activeRune.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 1f);

        // Compute next rune index change
        int indexChange = xInput > 0 ? 1 : runes.Count - 1;

        // Wait for gate to rotate
        yield return new WaitForSeconds(rotateDuration);

        // Active new rune
        activeRuneIndex = (activeRuneIndex + indexChange) % runes.Count;
        activeRune = runes[activeRuneIndex];
        activeRune.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2f, 1f, 0.2f);

        // Deactivate old rune for sure
        activeRune = runes[oldIndex];
        activeRune.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 1f);

        updateStargateText.Invoke();
    }

    IEnumerator RotateGate()
    {
        isTurning = true;

        float elapsedTime = 0.0f;
        float angle = rotationAngle * Mathf.Sign(xInput);

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles + Vector3.forward * angle);

        // Animation
        while (elapsedTime < rotateDuration)
        {
            // Interpolation
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, EaseInOut(elapsedTime / rotateDuration));
            elapsedTime += Time.deltaTime;
            // Next frame
            yield return null;
        }


        // Set location for sure
        transform.rotation = targetRotation;
        isTurning = false;
    }
}
