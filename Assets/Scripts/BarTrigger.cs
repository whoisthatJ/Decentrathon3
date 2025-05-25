using UnityEngine;

public class BarTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        //PlayerPrefs.SetString(id, gameObject.name);
        //gameObject.SetActive(false);
        GameManager.Instance.LoadBar();
    }

}
