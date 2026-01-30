using UnityEngine;

public class PlayerCarry : MonoBehaviour
{
    public bool IsCarrying => carriedTrash != null;

    private TrashPickup carriedTrash;

    public bool TryPickUp(TrashPickup trash)
    {
        if (trash == null) return false;
        if (IsCarrying) return false;

        carriedTrash = trash;
        carriedTrash.AttachTo(transform);
        return true;
    }

    public void Deposit()
    {
        if (!IsCarrying) return;

        carriedTrash.DestroyTrash();
        carriedTrash = null;
    }

}
