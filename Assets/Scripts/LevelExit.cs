using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [Tooltip("Scene name to load (must be in Build Settings).")]
    public string nextSceneName;

    [Header("Lock")]
    [SerializeField] private bool locked = true;

    private bool triggered;

    public void SetLocked(bool value)
    {
        locked = value;
        Debug.Log($"[LevelExit] Locked = {locked}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (locked) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        SceneManager.LoadScene(nextSceneName);
    }
}
