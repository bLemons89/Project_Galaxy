using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class BaseMission
{
    [Header("Bools")]
    public bool accepted;
    public bool declined;
    public bool firstConvoComplete;
    public bool isComplete;

    public bool hasNoRequirements;

    [Header("Mission Specifics")]
    public MissionInfo info;
}
