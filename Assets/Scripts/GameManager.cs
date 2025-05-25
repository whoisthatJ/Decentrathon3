using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] EventMainUI eventUI;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void LoadShooter() {
        SceneManager.LoadScene("ShooterScene");
    }
    public void LoadDuel() {
        SceneManager.LoadScene("DuelScene");
    }
    private int goldToTranfer;
    public void LoadMainAfterShooter(int gold) {
        goldToTranfer = gold;
        SceneManager.sceneLoaded += SetPlayerAfterShooter;
        SceneManager.LoadScene("MainScene");
    }
    private void SetPlayerAfterShooter(Scene s, LoadSceneMode a) {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        eventUI = FindFirstObjectByType<EventMainUI>();
        player.transform.position = new Vector3(-41.87f, 0, 51.78f);
        player.transform.eulerAngles = new Vector3(0, -265.85f, 0);
        SceneManager.sceneLoaded -= SetPlayerAfterShooter;
        StartCoroutine(WaitOpenWindow());
    }
    private IEnumerator WaitOpenWindow() {
        yield return new WaitForSeconds (1f);

        eventUI.OpenWindow();
        eventUI.GoldEarned(goldToTranfer);
    }
}
