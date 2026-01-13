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

    [TextArea(3, 10)]

    [SerializeField] private String endTalk;

    [SerializeField] private Sprite npcAvatar;

    [SerializeField] private bool onQuest;
    public bool OnQuest => onQuest;

    [SerializeField] private string onQuestDialogue;

    [SerializeField] private float interactRadius;

    [SerializeField] private int curNpcDialogues = 0;
    [SerializeField] private int indexDialogue = 0;

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
             UIManageMent.Instance.DialogueUI.TurnOn();
             UIManageMent.Instance.DialogueUI.SetInfoDialogue(nameNpc, endTalk, curTalkerAva);
             return;
        }
    
        if (onQuest)
        {
            UIManageMent.Instance.DialogueUI.TurnOn();
            UIManageMent.Instance.DialogueUI.SetInfoDialogue(nameNpc, onQuestDialogue, npcAvatar);
            UIManageMent.Instance.DialogueUI.setTextButtonAccept(npcDialogues[curNpcDialogues].OnQuestCompleteDialogue);
            UIManageMent.Instance.DialogueUI.setTextButtonRefuse(npcDialogues[curNpcDialogues].OnQuestNotCompleteDialogue);
            UIManageMent.Instance.DialogueUI.TurnOnButton(CompleteQuest, OnGoingQuest);
            return;
        }
        int totalDialogue = npcDialogues[curNpcDialogues].Dialogues.Count;
        int indexQuestDialogue = npcDialogues[curNpcDialogues].IndexQuestDialogue;
        
        if (!canContinueDialogue)
        {
            return;
        }
        if (UIManageMent.Instance.DialogueUI.Completed == true && indexDialogue < totalDialogue && curNpcDialogues < npcDialogues.Count)
        {
            UIManageMent.Instance.DialogueUI.TurnOn();
            canContinueDialogue = true;
            Sprite curTalkerAva = npcAvatar;
            if (npcDialogues[curNpcDialogues].Dialogues[indexDialogue].Name != nameNpc)
            {
                curTalkerAva = GameManageMent.Instance.PlayerManager.PlayerAvatar;
            }
            UIManageMent.Instance.DialogueUI.SetInfoDialogue(npcDialogues[curNpcDialogues].Dialogues[indexDialogue].Name, npcDialogues[curNpcDialogues].Dialogues[indexDialogue].DialogueContent, curTalkerAva);
            
            if (indexDialogue == indexQuestDialogue)
            {
                UIManageMent.Instance.DialogueUI.setTextButtonAccept(npcDialogues[curNpcDialogues].AcceptQuestLine);
                UIManageMent.Instance.DialogueUI.setTextButtonRefuse(npcDialogues[curNpcDialogues].RefuseQuestLine);
                UIManageMent.Instance.DialogueUI.TurnOnButton(AcceptQuest, Refuse);
                canContinueDialogue = false;
                Debug.Log("TURN ON QUEST BUTTON");
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
        NpcDataValue npcDataValue = GameManageMent.Instance.QuestManager.GetNpcData(npcId);
        onQuest  = npcDataValue.onQuest;
        curNpcDialogues = npcDataValue.curNpcDialogues;
        indexDialogue = npcDataValue.indexDialogue;
        interacting = true;
        canContinueDialogue = npcDataValue.canContinueDialogue;
        UIManageMent.Instance.DialogueUI.TurnOn();
        StartDialogue();
    }
    public void TurnOffInteract()
    {
        interacting = false;
        GameManageMent.Instance.QuestManager.UpdateNpcData(npcId, onQuest, curNpcDialogues, indexDialogue, canContinueDialogue);

        UIManageMent.Instance.DialogueUI.TurnOff();
    }
    public void AcceptQuest()
    {
        if (interacting)
        {
            
        GameManageMent.Instance.QuestManager.AcceptQuest(npcDialogues[curNpcDialogues].QuestDefinition);
        onQuest = true;
        TurnOffInteract();
        EventSystem.current.SetSelectedGameObject(null);
            
        }
        
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
                    EventSystem.current.SetSelectedGameObject(null);
                    return;
                }
                else
                {
                    UIManageMent.Instance.DialogueUI.TurnOfButton();
                    onQuest = false;
                    canContinueDialogue = true;
                    StartDialogue();
                    EventSystem.current.SetSelectedGameObject(null);
                    return;
                }
            }
        }

    }
    public void OnGoingQuest()
    {
        TurnOffInteract();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Refuse()
    {
        indexDialogue--;
        canContinueDialogue = true;
        TurnOffInteract();
        EventSystem.current.SetSelectedGameObject(null);
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
    [SerializeField] private List<DialogueLine> dialogues;
    public List<DialogueLine>  Dialogues => dialogues;

    [SerializeField] private QuestDefinition questDefinition;
    public QuestDefinition QuestDefinition => questDefinition;

    [SerializeField] private String onQuestCompleteDialogue;
    public String OnQuestCompleteDialogue  => onQuestCompleteDialogue;

    [SerializeField] private String onQuestNotCompleteDialogue;
    public String OnQuestNotCompleteDialogue =>onQuestNotCompleteDialogue;

    [SerializeField] private int indexQuestDialogue;
    public int IndexQuestDialogue => indexQuestDialogue;

    [SerializeField] private String acceptQuestLine;

    public String AcceptQuestLine => acceptQuestLine;

    [SerializeField] private String refuseQuestLine;

    public String RefuseQuestLine => refuseQuestLine;

}

[System.Serializable]

public class DialogueLine
{
    
    [SerializeField] private String name;
    public String Name => name;


    [TextArea(3, 10)]
    [SerializeField] private String dialogueContent;

    public String DialogueContent => dialogueContent;
}
