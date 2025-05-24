using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CounterController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField] GameObject HatPanel { get; set; }
    [field: SerializeField] GameObject CounterPanel { get; set; }
    [field: SerializeField] GameObject NexRoundPanel { get; set; }
    [field: SerializeField] GameObject WinPanel { get; set; }
    [field: SerializeField] GameObject LoosePanel { get; set; }

    public int HatCount = 0;

    [field: SerializeField] TMP_Text Counter { get; set; }
    [field: SerializeField] Button ShootButton { get; set; }
    private int Timer = 3;
    [field: SerializeField] RectTransform Aim { get; set; }
    [field: SerializeField] RectTransform RedDot { get; set; }
    [field: SerializeField] Animator Animator { get; set; }
    DuelEnemyController enemyController;

    private bool dead = false;
    private Tween aim;

    private bool HasGun = false;

    void Start()
    {
        enemyController = FindObjectOfType<DuelEnemyController>();
        StartCounter();
    }

    void StartCounter()
    {
        Sequence countdownSequence = DOTween.Sequence();

        for (int i = Timer; i >= 0; i--)
        {
            int valueToShow = i;
            countdownSequence.AppendCallback(() =>
            {
                Counter.text = valueToShow.ToString();
            });
            countdownSequence.AppendInterval(1f);
        }

        countdownSequence.OnComplete(() =>
        {
            Counter.text = "GO";
            ShootButton.interactable = true;
            enemyController.StartDuel();
        });
    }

     void Shoot()
    {
        if (dead) return;
        Animator.SetTrigger("Shoot");
        aim.Kill();
        Aim.gameObject.SetActive(false);   
        if (RedDot.localScale.x > 0.6f)
        {
            if (RedDot.localScale.x > 0.75f && HatCount <= 1) GetHat();
            GetHat();
            enemyController.Kill();
            if(HatCount >= 3)
            {
               WinPanel.gameObject.SetActive(true);
                return;
            }
                NexRoundPanel.gameObject.SetActive(true);
        }
    }

     void Hold()
    {
        Animator.SetTrigger("HoldGun");
        HasGun = true;
        Counter.text = "3";
        CounterPanel.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ShootButton.interactable) return;
        Hold();
        Aim.gameObject.SetActive(true);

        Aim.anchoredPosition = new Vector2(Aim.anchoredPosition.x, 0f);

        aim = Aim.DOAnchorPosY(800f, 3.5f).SetEase(Ease.InOutCubic);

        RedDot.DOScale(Vector3.zero, 1.25f).OnComplete(() => {
            RedDot.DOScale(Vector3.one, 1.4f).OnComplete(() => {
                RedDot.DOScale(Vector3.zero, 0f).SetEase(Ease.InCirc);
            });
        });
    }

    public void Death()
    {
        Animator.SetTrigger("Death");
        
        ShootButton.interactable = false;
        aim.Kill();
        Aim.gameObject.SetActive(false );
        dead = true;
        LoosePanel.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!HasGun) return;
        Shoot();
    }

    public void ReLoad()
    {
        CounterPanel.gameObject.SetActive(true);
        Animator.SetTrigger("ReStart");
        HasGun = false;
        Aim.gameObject.SetActive(false);
        StartCoroutine(CounterRestart());
        ShootButton.interactable = false;
        Animator.ResetTrigger("ReStart");
    }

    IEnumerator CounterRestart()
    {
        yield return new WaitForSeconds(2);
        StartCounter();
    }

    public void GetHat()
    {
        HatPanel.transform.GetChild(HatCount).gameObject.GetComponent<Image>().color = Color.brown;
        HatCount++;
    }
}
