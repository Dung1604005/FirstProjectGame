using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI bodyTextUI;

    [SerializeField] private float charsPerSecond = 40f; // tốc độ gõ
    [SerializeField] private float punctuationDelay = 0.25f; // dừng nhẹ sau . , ! ?

    [SerializeField] private Button buttonAccept;
    [SerializeField] private TextMeshProUGUI buttonAcceptText;
    [SerializeField] private Button buttonRefuse;

    [SerializeField] private TextMeshProUGUI buttonRefuseText;

    [SerializeField] private Image avatar;

    private Coroutine typingCo;
    public bool IsTyping { get; private set; }
    public bool Completed { get; private set; }

    void Start()
    {
        IsTyping = false;
        Completed = true;
        TurnOff();
        TurnOfButton();

    }
    public void ShowInstant(string dialogue)
    {
        if (typingCo != null) StopCoroutine(typingCo);
        bodyTextUI.text = dialogue;
        bodyTextUI.ForceMeshUpdate();
        bodyTextUI.maxVisibleCharacters = int.MaxValue;
        IsTyping = false;
        Completed = true;
    }
    public void StartTyping(string fullText)
    {
        if (typingCo != null || this.gameObject.activeInHierarchy == false) StopCoroutine(typingCo);
        typingCo = StartCoroutine(TypeRoutine(fullText));
    }

    IEnumerator TypeRoutine(string dialogue)
    {
        IsTyping = true;
        Completed = false;
        bodyTextUI.text = dialogue;
        bodyTextUI.ForceMeshUpdate();
        bodyTextUI.maxVisibleCharacters = 0;
        int total = dialogue.Length;
        float baseDelay = 1f / Mathf.Max(1f, charsPerSecond);
        for (int i = 0; i < total; i++)
        {
            bodyTextUI.maxVisibleCharacters = i + 1;

            char c = bodyTextUI.textInfo.characterInfo[i].character;
            float delay = baseDelay;
            if (c == '.' || c == ',' || c == '!' || c == '?' || c == ';' || c == ':')
                delay += punctuationDelay;
            float t = 0;
            while (t < delay)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ShowInstant(dialogue);
                    yield break;
                }
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }
        IsTyping = false;
        Completed = true;

    }

    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    public void AddOnClickAccept(UnityAction action)
    {
        buttonAccept.onClick.RemoveAllListeners(); 
        buttonAccept.gameObject.SetActive(true);
        buttonAccept.onClick.AddListener(action);
    }
    public void AddOnClickRefuse(UnityAction action)
    {
        buttonRefuse.onClick.RemoveAllListeners();
        buttonRefuse.gameObject.SetActive(true);
        buttonRefuse.onClick.AddListener(action);
    }

    public void TurnOfButton()
    {
        buttonAccept.gameObject.SetActive(false);
        buttonRefuse.gameObject.SetActive(false);
    }
    public void TurnOnButton(UnityAction actionAcc, UnityAction actionRefuse)
    {
        AddOnClickAccept(actionAcc);
        AddOnClickRefuse(actionRefuse);
    }
    public void setTextButtonAccept(String text)
    {
        buttonAcceptText.text = text;
    }
    public void setTextButtonRefuse(String text)
    {
        buttonRefuseText.text = text;
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
       
    }

    public void SetInfoDialogue(string nameTalker, string dialogue, Sprite ava)
    {
        avatar.sprite = ava;

        nameUI.text = nameTalker;
        StartTyping(dialogue);
    }
    
}
