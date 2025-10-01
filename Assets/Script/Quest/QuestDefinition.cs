using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemStack
{
    private int itemId;
    public int ItemId => itemId;
    private int count;
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

    [SerializeField] private List<ItemStack> itemIdReward;

    public List<ItemStack> ItemIdReward=> itemIdReward;

    
}
