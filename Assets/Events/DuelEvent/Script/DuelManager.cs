using UnityEngine;
using UnityEngine.UI;

public class DuelManager : MonoBehaviour
{
    CounterController PlayerController;
    DuelEnemyController enemyController;
    

    [field: SerializeField] GameObject LoosePanel { get; set; }
    [field: SerializeField] GameObject NexRoundPanel { get; set; }
    [field: SerializeField] GameObject WinPanel { get; set; }
    [field: SerializeField] GameObject HatPanel { get; set; }


    private void Start()
    {
        PlayerController = FindObjectOfType<CounterController>();
        enemyController = FindObjectOfType<DuelEnemyController>();
    }

    public void Reload()
    {
        PlayerController.ReLoad();
        enemyController.ReLoad();
        LoosePanel.SetActive(false);
        NexRoundPanel.SetActive(false);
        WinPanel.SetActive(false);
    }

    public void ReloadMiniGame()
    {
        Reload();
        PlayerController.HatCount = 0;
        for (int i = 0; i < 3; i++)
        {
            HatPanel.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
    }




}
