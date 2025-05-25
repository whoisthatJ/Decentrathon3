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
    [SerializeField] private int tries = 0;

    [field: SerializeField] TMP_Text Counter { get; set; }
    [field: SerializeField] TMP_Text RoundText { get; set; }

    [field: SerializeField] TMP_Text HatsText { get; set; }
    [field: SerializeField] Button ShootButton { get; set; }
    private int Timer = 3;
    [field: SerializeField] RectTransform Aim { get; set; }
    [field: SerializeField] RectTransform RedDot { get; set; }
    [field: SerializeField] Animator Animator { get; set; }
    
    [SerializeField] AudioSource bellAudioSource;
    [SerializeField] AudioSource playerAudioSource;
    [SerializeField] AudioSource enemyAudioSource;
    [SerializeField] AudioClip [] revolverShots;
    [SerializeField] AudioClip[] deathClips;


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
        Counter.text = "";
        countdownSequence.AppendInterval(Random.Range(2.5f, 3.5f));
        
        /*for (int i = Timer; i > 0; i--)
        {
            int valueToShow = i;
            countdownSequence.AppendCallback(() =>
            {
                Counter.text = valueToShow.ToString();
            });
            countdownSequence.AppendInterval(1f);
        }*/

        countdownSequence.OnComplete(() =>
        {
            Counter.text = "GO";
            tries++;
            ShootButton.interactable = true;
            enemyController.StartDuel();
            bellAudioSource.Play();
        });
    }

     void Shoot()
    {
        if (dead) return;
        Animator.SetTrigger("Shoot");
        aim.Kill();
        Aim.gameObject.SetActive(false);
        playerAudioSource.clip = revolverShots[Random.Range(0, revolverShots.Length)];
        playerAudioSource.Play();
        Debug.Log(RedDot.localScale.x);
        if (RedDot.localScale.x > 0.45f)
        {
            if (RedDot.localScale.x > 0.7f)
                GetHat();
            if (RedDot.localScale.x > 0.81f)
                GetHat();
            GetHat();
            enemyController.Kill();
            
            enemyAudioSource.clip = deathClips[Random.Range(0, deathClips.Length)];
            enemyAudioSource.Play();
            if (HatCount >= 3 || (tries >= 3 && HatCount >= 2)) {
                WinPanel.gameObject.SetActive(true);
                HatsText.text = "+" + HatCount.ToString();
                return;
            }
            else if (tries >= 3) { 
                LoosePanel.gameObject.SetActive(true);
                return;
            }
            NexRoundPanel.gameObject.SetActive(true);
            RoundText.gameObject.SetActive(true);
        }
        
    }

    void Hold()
    {
        Camera.main.transform.DOMove(new Vector3(1.87f, 1.09f, -5.82f), 0.4f);
        Animator.SetTrigger("HoldGun");
        HasGun = true;
        Counter.text = "";
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
        enemyAudioSource.clip = revolverShots[Random.Range(0, revolverShots.Length)];
        enemyAudioSource.Play();
        playerAudioSource.clip = deathClips[Random.Range(0, deathClips.Length)];
        playerAudioSource.Play();
        if (tries >= 3) {
            LoosePanel.gameObject.SetActive(true);
        }
        else {
            RoundText.gameObject.SetActive(false);
            NexRoundPanel.gameObject.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!HasGun) return;
        Shoot();
    }

    public void ReLoad()
    {
        Camera.main.transform.DOMove(new Vector3(2.21f, 1.16f, -6.73f), 0.2f);
        CounterPanel.gameObject.SetActive(true);
        Animator.SetTrigger("ReStart");
        HasGun = false;
        Aim.gameObject.SetActive(false);
        StartCoroutine(CounterRestart());
        ShootButton.interactable = false;
        dead = false;
        //Animator.ResetTrigger("ReStart");
    }

    IEnumerator CounterRestart()
    {
        yield return new WaitForSeconds(2);
        StartCounter();
    }

    public void GetHat()
    {
        if (HatCount >= 3)
            return;
        HatPanel.transform.GetChild(HatCount).gameObject.GetComponent<Image>().color = Color.brown;
        HatCount++;
    }
}
