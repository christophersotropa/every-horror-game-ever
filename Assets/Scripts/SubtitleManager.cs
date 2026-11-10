using System.Collections;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    [System.Serializable]
    public class SubtitleLine
    {
        [TextArea(1, 3)]
        public string text;
        public float delayBefore = 3f;  // wait after the previous line ends (or after spawn for the first)
        public float duration = 3f;     // how long the line stays on screen
    }

    [Header("References")]
    public TextMeshProUGUI subtitleText;

    [Header("Subtitles")]
    public SubtitleLine[] lines;

    void Start()
    {
        subtitleText.text = "";
        StartCoroutine(PlaySubtitles());
    }

    IEnumerator PlaySubtitles()
    {
        foreach (SubtitleLine line in lines)
        {
            yield return new WaitForSeconds(line.delayBefore);

            subtitleText.text = line.text;
            yield return new WaitForSeconds(line.duration);

            subtitleText.text = "";
        }
    }

    // Call this from other scripts for one-off subtitles, e.g. when entering a trigger
    public void ShowSubtitle(string text, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowOne(text, duration));
    }

    IEnumerator ShowOne(string text, float duration)
    {
        subtitleText.text = text;   
        yield return new WaitForSeconds(duration);
        subtitleText.text = "";
    }
}