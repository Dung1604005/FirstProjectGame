using System;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class LoadingAdditive : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image iconLoading;

    [SerializeField] private TextMeshProUGUI loadingText;

    [SerializeField] private float timeGapLoadingText;

    private float fillTarget = 0f;

    [SerializeField] private float fillSpeed;

    private int current_DotLoading = 0;

    private Coroutine textCoroutine;

    public void TurnOn()
    {
        
        this.gameObject.SetActive(true);
        iconLoading.fillAmount = 0f;
        fillTarget = 0f;
        current_DotLoading = 0;
        textCoroutine = StartCoroutine(LoadingText());

    }
    public void TurnOff()
    {
        
        this.gameObject.SetActive(false);
        iconLoading.fillAmount = 0f;
        fillTarget = 0f;
        if(textCoroutine != null)
        {
            textCoroutine = null;
        }

    }
    IEnumerator LoadingText()
    {
        for(int time = 1; time <= 10000; time++)
        {
            String text = "Loading";
        for(int i = 0; i < current_DotLoading   ; i++)
        {
            text += ".";
        }
        loadingText.text = text;
        
        
        current_DotLoading += 1;
        current_DotLoading %= 4;
        float t = 0f;
        while(t < timeGapLoadingText)
        {
            
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        }
       
        

    }


    public void SetFillTarget(float _fillTarget)
    {
        fillTarget = _fillTarget;
    }

    /// <summary>
    /// Kiem tra thanh loading da gan dat den target chua (sai so nho hon 0.01)
    /// </summary>
    public bool IsFillComplete()
    {
        return Mathf.Abs(iconLoading.fillAmount - fillTarget) < 0.01f;
    }

    void Awake()
    {
        //TurnOff();
    }

    void Update()
    {
        if(iconLoading.fillAmount != fillTarget)
        {
            // Dung unscaledDeltaTime de khong bi anh huong boi timeScale
            // Dung MoveTowards thay vi Lerp de toc do deu va dat duoc target chinh xac
            iconLoading.fillAmount = Mathf.MoveTowards(iconLoading.fillAmount, fillTarget, fillSpeed * Time.unscaledDeltaTime);
        }
    }

}
