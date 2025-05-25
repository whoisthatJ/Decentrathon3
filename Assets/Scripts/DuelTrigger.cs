using UnityEngine;

public class DuelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        //PlayerPrefs.SetString(id, gameObject.name);
        //gameObject.SetActive(false);
        GameManager.Instance.LoadDuel();
    }
}
