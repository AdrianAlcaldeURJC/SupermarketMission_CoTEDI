using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{

    private ObstaclesGame miniGameManager;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        miniGameManager = FindObjectOfType<ObstaclesGame>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetTrigger("MoveLeft");
            StartCoroutine(DeactivateTrigger());
        }
        if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetTrigger("MoveRight");
            StartCoroutine(DeactivateTrigger());
        }
    }

    IEnumerator DeactivateTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        anim.ResetTrigger("MoveLeft");
        anim.ResetTrigger("MoveRight");
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().trolleyHitSFX);
        anim.SetTrigger("Damaged");
        miniGameManager.DamagePlayer();
    }
}
