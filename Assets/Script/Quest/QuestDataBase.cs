using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataBase", menuName = "Scriptable Objects/QuestDataBase")]
public class QuestDataBase : ScriptableObject
{
    [SerializeField] private List<QuestDefinition> allQuests;

    
    public QuestDefinition GetQuestByID(int idToFind)
    {
        if (idToFind >= allQuests.Count)
        {
            return null;
        }
        return allQuests[idToFind];
    }
}
