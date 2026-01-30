using UnityEngine;
using System;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int TotalTrash => totalTrash;
    public int DepositedTrash => depositedTrash;

    public event Action<int, int> OnTrashChanged; // (deposited, total)


    [Header("Progress")]
    [SerializeField] private int totalTrash = 0;
    [SerializeField] private int depositedTrash = 0;

    [Header("References")]
    [SerializeField] private LevelExit levelExit;  // <-- changed from ExitDoor to LevelExit

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        totalTrash = FindObjectsOfType<TrashPickup>(true).Length;
        OnTrashChanged?.Invoke(depositedTrash, totalTrash);

        if (levelExit != null)
            levelExit.SetLocked(true);

        Debug.Log($"[LevelManager] totalTrash = {totalTrash}");
    }

    public void OnTrashDeposited()
    {
        depositedTrash++;
        OnTrashChanged?.Invoke(depositedTrash, totalTrash);
        Debug.Log($"[LevelManager] depositedTrash = {depositedTrash}/{totalTrash}");

        if (totalTrash > 0 && depositedTrash >= totalTrash)
        {
            Debug.Log("[LevelManager] All trash deposited -> EXIT UNLOCKED!");
            if (levelExit != null)
                levelExit.SetLocked(false);
        }
    }
}
