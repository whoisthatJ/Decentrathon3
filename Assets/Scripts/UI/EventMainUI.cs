using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class EventMainUI : MonoBehaviour
{

    [SerializeField] Transform goldPiecesParent;
    [SerializeField] Sprite [] goldPiecesSprites;
    [SerializeField] GameObject goldPiecesPrefab;
    [SerializeField] GameObject cart50;
    [SerializeField] GameObject cart70;
    [SerializeField] GameObject cartFull;

    [SerializeField] TextMeshProUGUI goldTxt;
    [SerializeField] TextMeshProUGUI hatText;
    [SerializeField] RectTransform goldRect;
    [SerializeField] RectTransform hatRect;

    [SerializeField] private int gold;
    [SerializeField] private int hat;
    private int goldMax = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GoldEarned(10);
    }

    public void GoldEarned(int pieces) {
        int tGold = gold;
        gold += pieces;

        for (int i = 0; i < pieces; i++) {
            GameObject piece = Instantiate(goldPiecesPrefab, goldPiecesParent);
            Image image = piece.GetComponent<Image>();
            image.sprite = goldPiecesSprites[Random.Range(0, goldPiecesSprites.Length)];
            image.color = new Color(1, 1, 1, 0);
            RectTransform rect = piece.GetComponent<RectTransform>();
            float horOffset = Random.Range(-120f, 120f);
            float vertOffset = Random.Range(-50f, 50f);
            rect.anchoredPosition = new Vector2(horOffset, vertOffset);
            int id = i;
            rect.DOAnchorPosY(-300f, 1f).SetEase(Ease.InQuad).SetDelay(i*Random.Range(0.2f, 0.25f)).OnStart(()=> { image.color = new Color(1, 1, 1, 1); }).OnComplete(() => {                
                goldRect.DOPunchScale(Vector3.one * 1.01f, 0.05f).OnComplete(() => { goldRect.transform.localScale = Vector3.one; });
                goldTxt.text = $"{tGold + id + 1}";
                if (id == pieces - 1) {
                    goldTxt.text = gold.ToString();
                    UpdateCart();
                }
                Destroy(piece);
            });
        }
        
    }
    private void UpdateCart() {
        if (!cart50.activeSelf && gold > goldMax / 2) {
            cart50.SetActive(true);
            cart50.transform.localScale = Vector3.one*0.5f;
            cart50.transform.DOScale(Vector3.one, 0.5f);
        }
        if(!cart70.activeSelf && gold > goldMax/10*7) {
            cart70.SetActive(true);
            cart70.transform.localScale = Vector3.one * 0.5f;
            cart70.transform.DOScale(Vector3.one, 0.5f);
        }
        if (!cartFull.activeSelf && gold >= goldMax) {
            cartFull.SetActive(true);
            cartFull.transform.localScale = Vector3.one * 0.5f;
            cartFull.transform.DOScale(Vector3.one, 0.5f);
        }
    }
}
