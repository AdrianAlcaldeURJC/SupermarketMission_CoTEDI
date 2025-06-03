using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{

    private ObstaclesGame miniGameManager;
    private Animator anim;
    
    [SerializeField] private ObstaclesListener obstaclesListener;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        miniGameManager = FindObjectOfType<ObstaclesGame>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            anim.SetTrigger("MoveLeft");
            StartCoroutine(DeactivateTrigger(0));
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            anim.SetTrigger("MoveRight");
            StartCoroutine(DeactivateTrigger(1));
        }

    }

    IEnumerator DeactivateTrigger(int direction)
    {
        int isCorrect = isMoving ? 0 : 1;
        isMoving = true;
        obstaclesListener.AddSlide(direction, isCorrect);
        yield return new WaitForSeconds(0.5f);
        isMoving = false;
        anim.ResetTrigger("MoveLeft");
        anim.ResetTrigger("MoveRight");
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.GetInstance().PlaySFXClip(AudioManager.GetInstance().trolleyHitSFX);
        anim.SetTrigger("Damaged");
        miniGameManager.DamagePlayer();

        obstaclesListener.AddImpact(miniGameManager.GetNumObstacles(),
                                    other.gameObject.GetComponent<ObstacleMovement>().GetObstacleID(),
                                    transform.position.x,
                                    miniGameManager.GetPlayerLives());
    }
}
