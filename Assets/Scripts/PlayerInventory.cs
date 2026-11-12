using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("UI (optional)")]
    public TextMeshProUGUI keyCountText;
    public int totalKeysInLevel = 4;

    public int KeyCount { get; private set; }

    void Start() => UpdateUI();

    public void AddKey()
    {
        KeyCount++;
        UpdateUI();
    }

    public bool HasKeys(int amount) => KeyCount >= amount;

    public void UseKeys(int amount)
    {
        KeyCount = Mathf.Max(0, KeyCount - amount);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (keyCountText != null)
            keyCountText.text = $"Keys: {KeyCount}/{totalKeysInLevel}";
    }
}