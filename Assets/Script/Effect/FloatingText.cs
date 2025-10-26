using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour, IPoolable
{
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatDuration = 1f;

    [SerializeField] private TextMeshProUGUI textMesh;

    private CanvasGroup canvasGroup;

    private float timer = 0f;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnSpawn()
    {
        timer = 0f;
        canvasGroup.alpha = 1f;
    }
    public void OnDeSpawn()
    {
        
    }
    public void SetUp(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
    }
    void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / floatDuration);
        if (timer >= floatDuration)
        {
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
            return;
        }
    }

}
