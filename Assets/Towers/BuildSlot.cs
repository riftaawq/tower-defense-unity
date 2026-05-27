using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSlot : MonoBehaviour
{
    public bool IsPath;
    public bool IsOccupied;
    public GameObject CurrentTower;

    private void OnMouseOver()
    {
        if (Mouse.current == null) return;
        if (!GameManager.Instance.CanBuild()) return;
        if (IsPath) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (IsOccupied) return;

            BuildMenuUI.Instance.ShowMenu(this, transform.position);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (!IsOccupied) return;

            BuildManager.Instance.RemoveTowerAt(this);
        }
    }
}