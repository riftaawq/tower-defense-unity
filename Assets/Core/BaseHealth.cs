using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 20;

    
    [SerializeField] private int currentHP;

    public int CurrentHP => currentHP;

    private void Start()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
    }

    public void DamageBase(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);

        Debug.Log("Base HP: " + currentHP); 

        GameManager.Instance?.NotifyBaseDamaged();
    }
}
