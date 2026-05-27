using UnityEngine;
using TMPro;

public class UIHUD : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI prepTimerText;
    public GameObject endPanel;
    public TextMeshProUGUI endLabel;

    public void Refresh()
    {
        if (GameManager.Instance == null) return;

        goldText.text = $"Gold: {GameManager.Instance.economyManager.Gold}";
        hpText.text = $"Base HP: {GameManager.Instance.baseHealth.CurrentHP}";
        roundText.text = $"Round: {GameManager.Instance.CurrentRound}/{GameManager.Instance.maxRounds}";
    }

    public void SetStateLabel(string state)
    {
        if (stateText != null)
            stateText.text = $"State: {state}";
    }

    public void ShowPreparation(int round, float time)
    {
        Refresh();
        UpdatePreparationTimer(time);
    }

    public void UpdatePreparationTimer(float time)
    {
        if (prepTimerText != null)
            prepTimerText.text = $"Prep: {Mathf.CeilToInt(time)}";
    }

    public void ShowEndScreen(bool victory)
    {
        if (endPanel != null) endPanel.SetActive(true);
        if (endLabel != null) endLabel.text = victory ? "Victory!" : "Defeat!";
    }
}
