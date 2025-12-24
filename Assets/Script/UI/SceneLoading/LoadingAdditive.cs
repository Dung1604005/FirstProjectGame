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
        
        String text = "Loading";
        for(int i = 0; i < current_DotLoading   ; i++)
        {
            text += ".";
        }
        loadingText.text = text;
        current_DotLoading += 1;
        current_DotLoading %= 4;
        yield return new WaitForSeconds(timeGapLoadingText);

    }


    public void SetFillTarget(float _fillTarget)
    {
        fillTarget = _fillTarget;
    }

    void Start()
    {
        TurnOff();
    }

    void Update()
    {
        if(iconLoading.fillAmount != fillTarget)
        {
            iconLoading.fillAmount = Mathf.Lerp(iconLoading.fillAmount, fillTarget, fillSpeed*Time.deltaTime);
        }
    }

}
