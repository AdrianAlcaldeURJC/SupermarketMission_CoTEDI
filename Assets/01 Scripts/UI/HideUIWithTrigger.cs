using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HideUIWithTrigger : MonoBehaviour
{
    [SerializeField] Image characterImage;
    [SerializeField] Image textBoxImage;
    [SerializeField] float transparency = 0.4f; 

    private void OnTriggerEnter(Collider other)
    {
        Color transparentWhite = new Color(1f, 1f, 1f, transparency);
        Color transparentBlack = new Color(0f, 0f, 0f, transparency);

        characterImage.color = transparentWhite;
        textBoxImage.color = transparentWhite;
        textBoxImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = transparentBlack;
    }

    private void OnTriggerExit(Collider other)
    {
        characterImage.color = Color.white;
        textBoxImage.color = Color.white;
        textBoxImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.black;
    }
}
