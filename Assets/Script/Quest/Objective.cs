using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectiveType
{
    Collect, Kill, Reach
}

[System.Serializable]
public class Objective
{
    public ObjectiveType objectiveType;

    public int targetId;

    public int requiredCount;

}
