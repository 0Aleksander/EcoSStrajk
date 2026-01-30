using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var carry = other.GetComponent<PlayerCarry>();
        if (carry == null) return;

        if (carry.IsCarrying)
        {
            carry.Deposit();
            LevelManager.Instance?.OnTrashDeposited();
            Debug.Log("Deposited trash");
        }
    }
}
