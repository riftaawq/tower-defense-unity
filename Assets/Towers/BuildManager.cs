using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    public EconomyManager economy;
    public GameObject[] towerPrefabs;
    public TowerData[] towerData;

    private int selectedIndex = 0;

    private void Awake()
    {
        Instance = this;

        if (economy == null)
        {
            economy = EconomyManager.Instance;
        }
    }

    public void SelectTower(int index)
    {
        if (towerData == null || towerData.Length == 0) return;
        if (index < 0 || index >= towerData.Length) return;

        selectedIndex = index;
    }

    public void TryBuildAt(BuildSlot slot)
    {
        if (slot == null) return;
        if (slot.IsOccupied) return;

        if (towerPrefabs == null || towerData == null) return;
        if (towerPrefabs.Length == 0 || towerData.Length == 0) return;
        if (selectedIndex < 0) return;
        if (selectedIndex >= towerPrefabs.Length || selectedIndex >= towerData.Length) return;

        TowerData data = towerData[selectedIndex];
        GameObject towerPrefab = towerPrefabs[selectedIndex];

        if (data == null || towerPrefab == null) return;

        if (economy == null)
        {
            Debug.LogWarning("EconomyManager is not assigned!");
            return;
        }

        if (!economy.TrySpendGold(data.cost))
        {
            Debug.Log("Not enough gold");
            return;
        }

        GameObject towerObj = Instantiate(towerPrefab, slot.transform.position, Quaternion.identity);
        TowerBase tower = towerObj.GetComponent<TowerBase>();

        if (tower != null)
        {
            tower.Configure(data);
        }

        slot.IsOccupied = true;
        slot.CurrentTower = towerObj;

        Debug.Log("Tower built. Gold left: " + economy.Gold);

        if (GameManager.Instance.ui != null)
        {
            GameManager.Instance.ui.Refresh();
        }
    }

    public void RemoveTowerAt(BuildSlot slot)
    {
        if (slot == null) return;
        if (!slot.IsOccupied) return;
        if (slot.CurrentTower == null) return;
        if (!GameManager.Instance.CanBuild()) return;

        TowerBase tower = slot.CurrentTower.GetComponent<TowerBase>();
        if (tower != null && tower.Data != null)
        {
            int refund = Mathf.RoundToInt(tower.Data.cost * 0.3f);
            economy.AddGold(refund);
            Debug.Log("Tower removed. Gold refunded: " + refund);
        }

        Destroy(slot.CurrentTower);
        slot.CurrentTower = null;
        slot.IsOccupied = false;

        if (GameManager.Instance.ui != null)
        {
            GameManager.Instance.ui.Refresh();
        }
    }
}