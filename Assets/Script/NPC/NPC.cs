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

    [SerializeField] private String endTalk;

    [SerializeField] private Sprite npcAvatar;

    [SerializeField] private bool onQuest;
    public bool OnQuest => onQuest;

    [SerializeField] private string onQuestDialogue;

    [SerializeField] private float interactRadius;

    private int curNpcDialogues = 0;
    private int indexDialogue = 0;

    [SerializeField] private bool interacting = false;

    [SerializeField] private GameObject interactingKey;

    [SerializeField] private bool isQuestGiver;
    public bool IsQuestGiver => isQuestGiver;


    private bool canContinueDialogue = true;





    public void StartDialogue()
    {
        if(curNpcDialogues >= npcDialogues.Count)
        {
            
             Sprite curTalkerAva = npcAvatar;
             UIManageMent.Instance.DialogueUI.SetInfoDialogue(nameNpc, endTalk, curTalkerAva);
             return;
        }
        Debug.Log("IsOnQuest " + onQuest);
        if (onQuest)
        {

            UIManageMent.Instance.DialogueUI.SetInfoDialogue(nameNpc, onQuestDialogue, npcAvatar);
            UIManageMent.Instance.DialogueUI.setTextButtonAccept(npcDialogues[curNpcDialogues].OnQuestCompleteDialogue);
            UIManageMent.Instance.DialogueUI.setTextButtonRefuse(npcDialogues[curNpcDialogues].OnQuestNotCompleteDialogue);
            UIManageMent.Instance.DialogueUI.TurnOnButton(CompleteQuest, OnGoingQuest);
            return;
        }
        int totalDialogue = npcDialogues[curNpcDialogues].Dialogues.Count;
        int indexQuestDialogue = npcDialogues[curNpcDialogues].IndexQuestDialogue;
        Debug.Log(indexDialogue);
        if (!canContinueDialogue)
        {
            return;
        }
        if (UIManageMent.Instance.DialogueUI.Completed == true && indexDialogue < totalDialogue && curNpcDialogues < npcDialogues.Count)
        {
            canContinueDialogue = true;
            Sprite curTalkerAva = npcAvatar;
            if (npcDialogues[curNpcDialogues].Dialogues[indexDialogue].First != nameNpc)
            {
                curTalkerAva = GameManageMent.Instance.PlayerManager.PlayerAvatar;
            }
            UIManageMent.Instance.DialogueUI.SetInfoDialogue(npcDialogues[curNpcDialogues].Dialogues[indexDialogue].First, npcDialogues[curNpcDialogues].Dialogues[indexDialogue].Second, curTalkerAva);
            
            if (indexDialogue == indexQuestDialogue)
            {
                UIManageMent.Instance.DialogueUI.setTextButtonAccept("Let's do it");
                UIManageMent.Instance.DialogueUI.setTextButtonRefuse("Nah may be later");
                UIManageMent.Instance.DialogueUI.TurnOnButton(AcceptQuest, Refuse);
                canContinueDialogue = false;
            }
            indexDialogue++;
            
            
        }
        else if(UIManageMent.Instance.DialogueUI.Completed == true && indexDialogue >= totalDialogue && curNpcDialogues < npcDialogues.Count)
        {
            curNpcDialogues++;
            indexDialogue = 0;
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
        if (!interacting)
        {
            return;
        }

        GameManageMent.Instance.QuestManager.AcceptQuest(npcDialogues[curNpcDialogues].QuestDefinition);

        TurnOffInteract();
        onQuest = true;

    }
    public void CompleteQuest()
    {
        for (int i = 0; i < GameManageMent.Instance.QuestManager.CurQuestDefinitons.Count; i++)
        {
            if (GameManageMent.Instance.QuestManager.CurQuestDefinitons[i].Id == npcDialogues[curNpcDialogues].QuestDefinition.Id)
            {
                if (!GameManageMent.Instance.QuestManager.Complete(i))
                {
                    UIManageMent.Instance.DialogueUI.TurnOfButton();
                    TurnOffInteract();
                    return;
                }
                else
                {
                    UIManageMent.Instance.DialogueUI.TurnOfButton();
                    TurnOffInteract();
                    onQuest = false;
                    canContinueDialogue = true;
                    return;
                }
            }
        }

    }
    public void OnGoingQuest()
    {
        TurnOffInteract();
    }

    public void Refuse()
    {
        indexDialogue--;
        TurnOffInteract();
        UIManageMent.Instance.DialogueUI.TurnOfButton();
    }

    void Update()
    {
        if (!interacting)
        {
            if((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.gameObject.transform.position).sqrMagnitude <= interactRadius * interactRadius)
            {
                interactingKey.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    TurnOnInteract();
                }
            }
            else
            {
                interactingKey.SetActive(false);
            }
            return;
        }
        interactingKey.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Space) && !onQuest)
        {
            if(interacting && curNpcDialogues >= npcDialogues.Count)
            {
                TurnOffInteract();
                return;
            }
            StartDialogue();
        }

    }






}


[System.Serializable]

public class NpcDialogue
{
    [SerializeField] private List<Pair<string, string>> dialogues;
    public List<Pair<string, string>> Dialogues => dialogues;

    [SerializeField] private QuestDefinition questDefinition;
    public QuestDefinition QuestDefinition => questDefinition;

    [SerializeField] private String onQuestCompleteDialogue;
    public String OnQuestCompleteDialogue  => onQuestCompleteDialogue;

    [SerializeField] private String onQuestNotCompleteDialogue;
    public String OnQuestNotCompleteDialogue =>onQuestNotCompleteDialogue;

    [SerializeField] private int indexQuestDialogue;
    public int IndexQuestDialogue => indexQuestDialogue;
}
