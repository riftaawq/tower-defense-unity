using UnityEngine;

public class BuildMenuUI : MonoBehaviour
{
    public static BuildMenuUI Instance;

    [SerializeField] private GameObject menuRoot;

    private BuildSlot currentSlot;

    private void Awake()
    {
        Instance = this;
        HideMenu();
    }

    public void ShowMenu(BuildSlot slot, Vector3 worldPosition)
    {
        currentSlot = slot;
        menuRoot.SetActive(true);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
        menuRoot.transform.position = screenPos;
    }

    public void HideMenu()
    {
        currentSlot = null;
        menuRoot.SetActive(false);
    }

    public void BuildTower(int towerIndex)
    {
        if (currentSlot == null)
            return;

        BuildManager.Instance.SelectTower(towerIndex);
        BuildManager.Instance.TryBuildAt(currentSlot);
        HideMenu();
    }
}