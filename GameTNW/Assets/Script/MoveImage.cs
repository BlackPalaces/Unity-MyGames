using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveImage : MonoBehaviour
{
    public RectTransform imageTransform; // อ้างอิงถึง RectTransform ของ GameObject Image
    public float moveDistance = 50f; // ระยะที่ต้องการให้ GameObject เคลื่อนที่
    public float moveSpeed = 100f; // ความเร็วในการเคลื่อนที่

    private Vector3 startPosition; // ตำแหน่งเริ่มต้นของ GameObject

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้น
        startPosition = imageTransform.localPosition;
        // เริ่ม Coroutine สำหรับการเคลื่อนที่แบบสุ่ม
        StartCoroutine(RandomMovement());
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            // สุ่มทิศทางและระยะทางในแกน x และ y
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 randomTarget = startPosition + new Vector3(randomDirection.x, randomDirection.y, 0f) * moveDistance;

            // เคลื่อนที่ไปยังตำแหน่งที่สุ่มได้
            while (Vector3.Distance(imageTransform.localPosition, randomTarget) > 0.1f)
            {
                imageTransform.localPosition = Vector3.MoveTowards(imageTransform.localPosition, randomTarget, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // รอสักครู่ก่อนที่จะสุ่มตำแหน่งใหม่
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }
}
