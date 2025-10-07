using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;



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
        if (onQuest)
        {
            
            UIManageMent.Instance.DialogueUI.SetInfoDialogue(nameNpc, onQuestDialogue);
            UIManageMent.Instance.DialogueUI.TurnOnButton(CompleteQuest, OnGoingQuest);
            return;
        }
        int totalDialogue = npcDialogues[curNpcDialogues].Dialogues.Count;
        Debug.Log(UIManageMent.Instance.DialogueUI.Completed);
        if (UIManageMent.Instance.DialogueUI.Completed == true && indexDialogue < totalDialogue && curNpcDialogues < npcDialogues.Count)
        {
            UIManageMent.Instance.DialogueUI.SetInfoDialogue(npcDialogues[curNpcDialogues].Dialogues[indexDialogue].First, npcDialogues[curNpcDialogues].Dialogues[indexDialogue].Second);
            indexDialogue++;
            if (indexDialogue == totalDialogue)
            {
                UIManageMent.Instance.DialogueUI.TurnOnButton(AcceptQuest, Refuse);
            }
        }

    }
    public void TurnOnInteract()
    {
        if ((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.gameObject.transform.position).sqrMagnitude > interactRadius * interactRadius)
        {
            return;
        }
        interacting = true;
        UIManageMent.Instance.DialogueUI.TurnOn();
        StartDialogue();
    }
    public void TurnOffInteract()
    {
        interacting = false;
        UIManageMent.Instance.DialogueUI.TurnOff();
    }
    public void AcceptQuest()
    {
        if (!interacting )
        {
            return;
        }
        
        GameManageMent.Instance.QuestManager.AcceptQuest(npcDialogues[curNpcDialogues].QuestDefinition);
        
        TurnOffInteract();
        onQuest = true;
        
    }
    public void CompleteQuest()
    {

        TurnOffInteract();
        onQuest = false;
        curNpcDialogues++;
    }
    public void OnGoingQuest()
    {
        TurnOffInteract();
    }

    public void Refuse()
    {
        indexDialogue = 0;
        TurnOffInteract();
        UIManageMent.Instance.DialogueUI.TurnOfButton();
    }

    void Update()
    {
        if (!interacting)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !onQuest)
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
