using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShooterManager : MonoBehaviour
{
    [SerializeField] private RectTransform Aim;
    [SerializeField] private Button ShootButton;
    [SerializeField] private RectTransform RedDot;
    [SerializeField] private Transform Player;
    [SerializeField] private TMP_Text Score;
    [SerializeField] private GameObject RewardPanel;
    [SerializeField] private TMP_Text RewardScore;
    [SerializeField] private Animator Animator;


    private int ScoreIndex;
    private int ShootCount;


    private float maxDistance = 100f;

    private void Start()
    {
        ScoreIndex = 0;
        ShootCount = 0;
        AimStart();
        ShootButton.onClick.AddListener(Shoot);
    }

    private void Shoot()
    {
        Animator.SetTrigger("Shoot");
        ShootCount++;
        ScoreIndex += (int)(RedDot.localScale.x * 25f);
        Score.text = ScoreIndex.ToString();
        if (ShootCount == 4)
        {
            RewardPanel.SetActive(true);
            RewardScore.text = Score.text;
            return;
        }
        Vector3 targetRotation = Player.localEulerAngles + new Vector3(0, 9.5f, 0);
        Player.DOLocalRotate(targetRotation, 0.5f).SetEase(Ease.InOutSine);
    }
    void AimStart()
    {
        
        Aim.anchoredPosition = new Vector2(Aim.anchoredPosition.x, -160f);

        // Анимируем по Y от -160 до 160
        Aim.DOAnchorPosY(160f, 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnUpdate(UpdateScale);
    }

    void UpdateScale()
    {
        // Получаем текущую позицию по Y
        float y = Aim.anchoredPosition.y;

        // t = 1, когда y = 0; t = 0, когда y = ±100
        float t = Mathf.Clamp01(1f - Mathf.Abs(y) / maxDistance);

        // Применяем scale ко второму RectTransform
        RedDot.localScale = Vector3.one * t;
    }

    public void Restart()
    {
        ScoreIndex = 0;
        ShootCount = 0;
        Player.DOLocalRotate(new Vector3(0, -100.5f, 0), 0.1f).SetEase(Ease.InOutSine);
        RewardPanel.SetActive(false);
    }
}
