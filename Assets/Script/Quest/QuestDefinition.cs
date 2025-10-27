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

    [SerializeField] private string description;

    public string Description => description;

    [SerializeField] private int npcId;
    public int NpcId => npcId;

    [SerializeField] private List<Objective> objectives;
    public List<Objective> Objectives => objectives;

    [SerializeField] private int goldReward;

    public int GoldReward => goldReward;

    [SerializeField] private int expReward;
    public int ExpReward => expReward;

    [SerializeField] private List<ItemStack> itemIdReward;

    public List<ItemStack> ItemIdReward=> itemIdReward;

    
}
