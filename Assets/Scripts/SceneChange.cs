using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [Header("Scene")]
    public string sceneName = "Scene2";   // must match the scene file name exactly

    [Header("Who triggers it")]
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            SceneManager.LoadScene(sceneName);
    }
}