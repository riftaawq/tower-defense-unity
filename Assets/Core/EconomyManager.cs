using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    [SerializeField] private int startGold = 300;
    [SerializeField] private int startAttackBudget = 200;
    [SerializeField] private int budgetGrowthPerRound = 35;

    public int Gold { get; private set; }
    public int AttackBudget { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetEconomy();
    }

    public void ResetEconomy()
    {
        Gold = startGold;
        AttackBudget = startAttackBudget;
    }

    public bool TrySpendGold(int amount)
    {
        if (Gold < amount) return false;

        Gold -= amount;
        Debug.Log("Gold after spend: " + Gold);
        return true;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        Debug.Log("Gold after reward: " + Gold);
    }

    public void AdvanceRoundBudget(int round)
    {
        AttackBudget = startAttackBudget + round * budgetGrowthPerRound;
    }
}