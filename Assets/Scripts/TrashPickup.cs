using UnityEngine;

public class TrashPickup : MonoBehaviour
{
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var carry = other.GetComponent<PlayerCarry>();
        if (carry == null) return;

        bool picked = carry.TryPickUp(this);
        if (picked)
        {
            Debug.Log("Picked up trash (carrying)");
        }
    }

    public void AttachTo(Transform parent)
    {
        if (col != null) col.enabled = false;

        transform.SetParent(parent);
        transform.localPosition = new Vector3(0f, 0.7f, 0f); // sits above player a bit
    }

    public void DestroyTrash()
    {
        Destroy(gameObject);
    }
}
