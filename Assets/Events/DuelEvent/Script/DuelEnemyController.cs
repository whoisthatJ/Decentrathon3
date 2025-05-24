using System.Collections;
using UnityEngine;

public class DuelEnemyController : MonoBehaviour
{
    [field:SerializeField] Animator Animator { get; set; }
    CounterController PlayerController;

    public Coroutine coroutine { get; set; }

    private void Start()
    {
        PlayerController = FindObjectOfType<CounterController>();
    }

    public void Kill()
    {
        StopCoroutine(coroutine);
        Animator.SetTrigger("Death");
    }

    public void StartDuel()
    {
        coroutine = StartCoroutine(HoldGun());
    }

    IEnumerator HoldGun()
    {
        yield return new WaitForSeconds(0.2f);
        Animator.SetTrigger("HoldGun");
        yield return new WaitForSeconds(2.5f);
        Animator.SetTrigger("Shoot");
        PlayerController.Death();
    }

    public void ReLoad()
    {
        Animator.SetTrigger("ReStart");
    }


}
