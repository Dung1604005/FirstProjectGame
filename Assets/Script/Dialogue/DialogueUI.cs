using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameUI;
    [SerializeField] private TextMeshProUGUI bodyTextUI;

    [SerializeField] private float charsPerSecond = 40f; // tốc độ gõ
    [SerializeField] private float punctuationDelay = 0.25f; // dừng nhẹ sau . , ! ?

     private Coroutine typingCo;
    public bool IsTyping { get; private set; }
    public bool Completed { get; private set; }

    void Start()
    {
        IsTyping = false;
        Completed = true;
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
        if (typingCo != null) StopCoroutine(typingCo);
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
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }

    public void SetInfoDialogue(string nameTalker, string dialogue)
    {
        
        nameUI.text = nameTalker;
        StartTyping(dialogue);
    }
}
