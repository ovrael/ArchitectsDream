using System;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp.Assets.Scripts.Stargate;

using UnityEngine;


[CreateAssetMenu(fileName = "Create", menuName = "ScriptableObjects/Stargate/InnerRune", order = 2)]
[Serializable]
public class InnerRuneData : RuneData
{
    [SerializeField]
    private string runeName;
    public string RuneName { get { return runeName; } }

    [SerializeField]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private GameObject environmentalPrefab;
    public GameObject EnvironmentalPrefab { get { return environmentalPrefab; } }

    [SerializeField]
    private string[] modifications;
    public string[] Modifications { get { return modifications; } }
}