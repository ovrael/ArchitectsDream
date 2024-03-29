using System;
using System.Collections;
using System.Collections.Generic;

using AssemblyCSharp.Assets.Scripts.Stargate;

using UnityEngine;


[CreateAssetMenu(fileName = "Create", menuName = "ScriptableObjects/Stargate/OuterRune", order = 1)]
[Serializable]
public class OuterRuneData : RuneData
{
    [SerializeField]
    private string runeName;
    public string RuneName { get { return runeName; } }

    [SerializeField]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private string targetLocation;
    public string TargetLocation { get { return targetLocation; } }
}