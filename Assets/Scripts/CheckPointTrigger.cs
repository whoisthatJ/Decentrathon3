using UnityEngine;
using UnityEngine.Events;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private UnityEvent action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start() {
        /*if(PlayerPrefs.HasKey(id))
            gameObject.SetActive(false);*/
    }
    private void OnTriggerEnter(Collider other) {
        //PlayerPrefs.SetString(id, gameObject.name);
        //gameObject.SetActive(false);
        GameManager.Instance.LoadShooter();
    }
}
