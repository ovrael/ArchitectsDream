using AssemblyCSharp.Assets.Scripts.Stargate;
using System;
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
    private string targetPlanet;
    public string TargetLocation { get { return targetPlanet; } }

    [SerializeField]
    private string location;
    public string Location { get { return location; } }
}