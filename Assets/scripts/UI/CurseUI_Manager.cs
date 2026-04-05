using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CurseUI_Manager : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private TextMeshProUGUI title, curseName, curseValue;
    [SerializeField] private Color red, green;

    private float panelAlpha, titleAlpha, curseNameAlpha, curseValueAlpha;

    private void Awake()
    {
        panelAlpha = panel.color.a;
        titleAlpha = title.color.a;
        curseNameAlpha = curseName.color.a;
        curseValueAlpha = curseValue.color.a;

        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0f);
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0f);
        curseName.color = new Color(curseName.color.r, curseName.color.g, curseName.color.b, 0f);
        curseValue.color = new Color(curseValue.color.r, curseValue.color.g, curseValue.color.b, 0f);
    }
    public void Set_UI_Elements(string name, string value, string level)
    {
        curseName.text = name;
        curseValue.text = value;
        curseName.color = curseValue.color = (level == "good") ? green : red;

        // Start fade-in, then automatically fade-out after 1 second
        FadeAnimation(true, 1f, 1f); // 1f = duration, 1f = wait time before fade-out
    }

    public void FadeAnimation(bool fadeIn, float duration = 1f, float autoFadeOutDelay = 0f)
    {
        StopAllCoroutines(); // Stop any ongoing fade
        StartCoroutine(FadeCoroutine(fadeIn, duration, autoFadeOutDelay));
    }

    private IEnumerator FadeCoroutine(bool fadeIn, float duration, float autoFadeOutDelay)
    {
        float timer = 0f;

        // Initial alpha values
        float startPanelAlpha = panel.color.a;
        float startTitleAlpha = title.color.a;
        float startCurseNameAlpha = curseName.color.a;
        float startCurseValueAlpha = curseValue.color.a;

        // Target alpha values
        float targetPanelAlpha = fadeIn ? panelAlpha : 0f;
        float targetTitleAlpha = fadeIn ? titleAlpha : 0f;
        float targetCurseNameAlpha = fadeIn ? curseNameAlpha : 0f;
        float targetCurseValueAlpha = fadeIn ? curseValueAlpha : 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // Lerp each element's alpha
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, Mathf.Lerp(startPanelAlpha, targetPanelAlpha, t));
            title.color = new Color(title.color.r, title.color.g, title.color.b, Mathf.Lerp(startTitleAlpha, targetTitleAlpha, t));
            curseName.color = new Color(curseName.color.r, curseName.color.g, curseName.color.b, Mathf.Lerp(startCurseNameAlpha, targetCurseNameAlpha, t));
            curseValue.color = new Color(curseValue.color.r, curseValue.color.g, curseValue.color.b, Mathf.Lerp(startCurseValueAlpha, targetCurseValueAlpha, t));

            yield return null;
        }

        // Ensure target alpha is set at the end
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, targetPanelAlpha);
        title.color = new Color(title.color.r, title.color.g, title.color.b, targetTitleAlpha);
        curseName.color = new Color(curseName.color.r, curseName.color.g, curseName.color.b, targetCurseNameAlpha);
        curseValue.color = new Color(curseValue.color.r, curseValue.color.g, curseValue.color.b, targetCurseValueAlpha);

        // If fade-in, wait and then fade out automatically
        if (fadeIn && autoFadeOutDelay > 0f)
        {
            yield return new WaitForSeconds(autoFadeOutDelay);
            StartCoroutine(FadeCoroutine(false, duration, 0f)); // Fade out with same duration
        }
    }
}
