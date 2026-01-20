using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectiveType
{
    Collect, Kill, TalkToNpc, ReachArea
}

[System.Serializable]
public class Objective
{
    public ObjectiveType objectiveType;

    public int targetId;

    public int requiredCount;

    public String objectiveRequirement;

    public bool haveDirection;

    public Vector3 destinationPosition;

    public int destinationIdSceneData;

}
