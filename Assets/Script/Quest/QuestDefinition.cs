using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    [SerializeField] private int itemId;
    public int ItemId => itemId;
    [SerializeField] private int count;
    public int Count => count;
}
[CreateAssetMenu(menuName = "Quest")]
public class QuestDefinition : ScriptableObject
{
    [SerializeField] private int id;

    public int Id => id;
    [SerializeField] private string nameQuest;

    public string NameQuest => nameQuest;

    [TextArea(3, 10)]

    [SerializeField] private string description;

    public string Description => description;

    [SerializeField] private int npcId;
    public int NpcId => npcId;

    [SerializeField] private List<ItemStack> itemQuestList;

    public List<ItemStack> ItemQuestList => itemQuestList;

    [SerializeField] private List<Objective> objectives;
    public List<Objective> Objectives => objectives;

    [SerializeField] private int goldReward;

    public int GoldReward => goldReward;

    [SerializeField] private int expReward;
    public int ExpReward => expReward;

    [SerializeField] private List<ItemStack> itemIdReward;

    public List<ItemStack> ItemIdReward=> itemIdReward;

    [SerializeField] private bool haveDirection;

    public bool HaveDirection => haveDirection;

    [SerializeField] private Vector3 destinationPosition;
    public Vector3 DestinationPosition => destinationPosition;

    
}
