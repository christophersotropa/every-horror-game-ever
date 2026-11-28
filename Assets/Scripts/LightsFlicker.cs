using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float baseIntensity = 100f;
    public float dimIntensity = 20f;
    public Vector2 timeBetweenFlickers = new Vector2(2f, 8f);   // steady time (seconds)
    public Vector2 flickerDuration = new Vector2(0.2f, 0.8f);   // how long each burst lasts
    public Vector2 stepTime = new Vector2(0.02f, 0.1f);         // speed within a burst

    Light lamp;

    void Awake() { lamp = GetComponent<Light>(); }

    void OnEnable() { StartCoroutine(FlickerLoop()); }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            lamp.intensity = baseIntensity;
            yield return new WaitForSeconds(Random.Range(timeBetweenFlickers.x, timeBetweenFlickers.y));

            float end = Time.time + Random.Range(flickerDuration.x, flickerDuration.y);
            while (Time.time < end)
            {
                lamp.intensity = Random.value < 0.5f
                    ? Random.Range(0f, dimIntensity)
                    : Random.Range(dimIntensity, baseIntensity);
                yield return new WaitForSeconds(Random.Range(stepTime.x, stepTime.y));
            }
        }
    }
}