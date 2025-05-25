using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarEventManager : MonoBehaviour
{
    [field: SerializeField] public Button ClickButton { get; set; }
    [field: SerializeField] public Image ProgressBar { get; set; }
    [field: SerializeField] public TMP_Text TimeCounter { get; set; }
    [field: SerializeField] public GameObject TimePanel { get; set; }
    [field: SerializeField] public GameObject WinPanel { get; set; }
    [field: SerializeField] public GameObject LoosePanel { get; set; }
    [field: SerializeField] public GameObject StartPanel { get; set; }
    [field: SerializeField] public TMP_Text PlayerCounter { get; set; }
    [field: SerializeField] public TMP_Text EnemyCounter { get; set; }
    [field: SerializeField] public Animator PlayerAnimator { get; set; }
    [field: SerializeField] public Animator EnemyAnimator { get; set; }

    private int Timer = 3;
    private int DrinkCount = 0;
    private int EnemyDrinkCount = 0;
    private bool IsDrinking = false;
    public double DrinkProgress;

    public void Start()
    {
        StartPanel.SetActive(true);
    }
    public void StartGame()
    {
        TimePanel.SetActive(true);
        ClickButton.onClick.AddListener(Drink);
        StartEvent();
        StartPanel.SetActive(false);
    }

    public void StartEvent()
    {
        Restart();
        

        ClickButton.interactable = false;
        Timer = 3;
        Sequence countdownSequence = DOTween.Sequence();

        for (int i = Timer; i > 0; i--)
       {
           int valueToShow = i;
           countdownSequence.AppendCallback(() =>
           {
               TimeCounter.text = valueToShow.ToString();
           });
           countdownSequence.AppendInterval(1f);
       }


        
        countdownSequence.OnComplete(() =>
        {
            PlayerAnimator.SetTrigger("Start");
            EnemyAnimator.SetTrigger("Start");
            ClickButton.interactable = true;
            StartCoroutine(EnemyCorutine());
            Timer = 30;
            TimeCounter.text = "30";
            Sequence TimerSequence = DOTween.Sequence();

            for (int i = Timer; i > 0; i--)
            {
                int valueToShow = i;
                TimerSequence.AppendCallback(() =>
                {
                    TimeCounter.text = valueToShow.ToString();
                });
                TimerSequence.AppendInterval(1f);
            }

            TimerSequence.OnComplete(() =>
            {
                if(EnemyDrinkCount < DrinkCount)
                {
                    WinPanel.SetActive(true);
                }
                else
                {
                    LoosePanel.SetActive(true);
                }
                TimeCounter.text = "3";
                TimePanel.SetActive(false);
                ClickButton.interactable = false;
            });
        });
    }

    void Drink()
    {
        if(!IsDrinking) PlayerAnimator.SetTrigger("Drink");
        IsDrinking = true;
        DrinkProgress += 0.1f;
        ProgressBar.DOFillAmount(1f - (float)DrinkProgress, 0.2f).SetEase(Ease.Linear);
        if (DrinkProgress >= 1) {
            Fill();
        }
        
    }

    void Fill()
    {
        IsDrinking = false;
        DrinkCount++;
        PlayerCounter.text = DrinkCount.ToString();
        ClickButton.interactable = false;
        ProgressBar.DOFillAmount(1f, 2f).SetEase(Ease.Linear).OnComplete(() => { ClickButton.interactable = true; });
        DrinkProgress = 0;
        PlayerAnimator.SetTrigger("Refill");
    }

    IEnumerator EnemyCorutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            EnemyAnimator.SetTrigger("Drink");
            yield return new WaitForSeconds(3f);
            EnemyDrinkCount++;
            EnemyCounter.text = EnemyDrinkCount.ToString();
            EnemyAnimator.SetTrigger("Refill");
            yield return new WaitForSeconds(1f);
        }
    }
    public void Restart()
    {
        IsDrinking = false;
        WinPanel.SetActive(false);
        LoosePanel.SetActive(false);
        DrinkCount = 0;
        EnemyDrinkCount = 0;
        DrinkProgress = 0;
        ProgressBar.DOFillAmount(1f, 2f).SetEase(Ease.Linear).OnComplete(() => { ClickButton.interactable = false; });
    }

    public void ReLoad()
    {
        SceneManager.LoadScene("BarScene");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
