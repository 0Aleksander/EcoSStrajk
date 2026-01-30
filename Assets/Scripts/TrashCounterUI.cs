using TMPro;
using UnityEngine;

public class TrashCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (text == null)
            text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (LevelManager.Instance == null) return;

        text.text = $"Trash: {LevelManager.Instance.DepositedTrash}/{LevelManager.Instance.TotalTrash}";
    }
}
