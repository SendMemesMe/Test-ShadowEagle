using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BtnAttack : MonoBehaviour
{
    public Button button; 
    public Image fillImage; 
    public Text text; 
    private bool isClicked = false;
    private bool isCooldown = false;

    
    public void OnAttackButtonClick()
    {
        if (isCooldown) { return; }
        StartCoroutine(FillImageCoroutine());
        text.enabled = false;
        fillImage.enabled = true;
        isCooldown = true;
        isClicked = true;

    }

    private IEnumerator FillImageCoroutine()
    {
        float elapsedTime = 0f;
        float fillDuration = 1f; 

        while (elapsedTime < fillDuration)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);
            fillImage.fillAmount = fillAmount;

            yield return null;
        }
        fillImage.fillAmount = 1f;
        OnFillComplete();
    }

    private void OnFillComplete()
    {
        isCooldown = false;
        text.enabled = true;
        fillImage.enabled = false;
    }

    public bool IsAttackReady()
    {
        bool state = isClicked;
        isClicked=false;
        return state;
    }
}
