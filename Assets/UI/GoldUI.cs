using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private EconomyManager economy;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        if (economy == null)
        {
            economy = EconomyManager.Instance;
        }
    }

    private void Update()
    {
        if (economy == null || goldText == null) return;

        goldText.text = "Gold: " + economy.Gold;
    }
}