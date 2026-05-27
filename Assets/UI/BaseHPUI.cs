using TMPro;
using UnityEngine;

public class BaseHPUI : MonoBehaviour
{
    [SerializeField] private BaseHealth baseHealth;
    [SerializeField] private TextMeshProUGUI hpText;

    private void Update()
    {
        if (baseHealth == null || hpText == null) return;
        hpText.text = "HP: " + baseHealth.CurrentHP;
    }
}