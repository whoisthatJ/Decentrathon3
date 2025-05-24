using UnityEngine;

public class DuelManager : MonoBehaviour
{
    CounterController PlayerController;
    DuelEnemyController enemyController;
    private int HatCount = 0;

    public void GetHat()
    {
        HatCount++;

    }

    private void Start()
    {
        PlayerController = FindObjectOfType<CounterController>();
        enemyController = FindObjectOfType<DuelEnemyController>();
    }
}
