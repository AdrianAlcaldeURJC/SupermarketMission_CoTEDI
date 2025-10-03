using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessage : MonoBehaviour
{
    [SerializeField] Vector2 disappearTimeRange;
    [SerializeField] Vector2 upDistanceRange;
    [SerializeField] Vector2 speedRange;


    public void Start()
    {
        float disappearTime = Random.Range(disappearTimeRange.x, disappearTimeRange.y);
        float upDistance = Random.Range(upDistanceRange.x, upDistanceRange.y);
        float speed = Random.Range(speedRange.x, speedRange.y);

        StartCoroutine(PopUp(disappearTime, upDistance, speed));
    }

    private IEnumerator PopUp(float disappearTime, float upDistance, float speed)
    {
        float time = 0;
        float alpha = 1;
        float disappearSpeedTime = disappearTime / speed;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0.0f, upDistance, 0.0f);
        Image image = GetComponent<Image>();
        TextMeshProUGUI tmp = GetComponentInChildren<TextMeshProUGUI>();

        while (time < disappearSpeedTime)
        {
            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, time / disappearSpeedTime);
            alpha = Mathf.Lerp(1, 0, time / disappearSpeedTime);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;
        Object.Destroy(gameObject);
        
    }
}
