using System.Collections;
using UnityEngine;
using TMPro;

public class TextShake : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Drag your TextMeshProUGUI component here in the Inspector
    public float magnitude = 1f; // Magnitude of the shake

    void OnEnable()
    {
        // Start shaking text when the GameObject is enabled
        StartCoroutine(ShakeTextContinuously());
    }

    IEnumerator ShakeTextContinuously()
    {
        Vector3 originalPos = textComponent.rectTransform.localPosition;

        while (true)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            textComponent.rectTransform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            yield return new WaitForSeconds(0.02f); // Adjust the frequency of the shake
        }
    }
}
