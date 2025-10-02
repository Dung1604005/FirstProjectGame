using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class NPC : MonoBehaviour
{

    [SerializeField] private int npcId;

    [SerializeField] private string nameNpc;


    [SerializeField] private List<NpcDialogue> npcDialogues;

    [SerializeField] private bool onQuest;
    public bool OnQuest => onQuest;

    [SerializeField] private string onQuestDialogue;

    [SerializeField] private float interactRadius;

    private int curNpcDialogues = 0;
    private int indexDialogue = 0;

    [SerializeField] private bool interacting = false;


    public void StartDialogue()
    {
        int totalDialogue = npcDialogues[curNpcDialogues].Dialogues.Count;
        Debug.Log(UIManageMent.Instance.DialogueUI.Completed);
        if (UIManageMent.Instance.DialogueUI.Completed == true && indexDialogue < totalDialogue)
        {
            UIManageMent.Instance.DialogueUI.SetInfoDialogue(npcDialogues[curNpcDialogues].Dialogues[indexDialogue].First, npcDialogues[curNpcDialogues].Dialogues[indexDialogue].Second);
            indexDialogue++;
        }

    }
    public void TurnOnInteract()
    {
        interacting = true;
    }
    public void TurnOffInteract()
    {
        interacting = false;
    }
    void Update()
    {
        if (!interacting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartDialogue();
        }
        
    }
    





}


[System.Serializable]

public class NpcDialogue
{
    [SerializeField]private List<Pair<string, string>> dialogues;
    public List<Pair<string, string>> Dialogues => dialogues;
    
    [SerializeField] private QuestDefinition questDefinition;
    public QuestDefinition QuestDefinition => questDefinition;
}
