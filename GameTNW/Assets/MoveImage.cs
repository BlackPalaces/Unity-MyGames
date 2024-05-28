using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveImage : MonoBehaviour
{
    public RectTransform imageTransform; // ��ҧ�ԧ�֧ RectTransform �ͧ GameObject Image
    public float moveDistance = 50f; // ���з���ͧ������ GameObject ����͹���
    public float moveSpeed = 100f; // ��������㹡������͹���

    private Vector3 startPosition; // ���˹�������鹢ͧ GameObject

    void Start()
    {
        // �ѹ�֡���˹��������
        startPosition = imageTransform.localPosition;
        // ����� Coroutine ����Ѻ�������͹���Ẻ����
        StartCoroutine(RandomMovement());
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            // ������ȷҧ������зҧ�᡹ x ��� y
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 randomTarget = startPosition + new Vector3(randomDirection.x, randomDirection.y, 0f) * moveDistance;

            // ����͹�����ѧ���˹觷��������
            while (Vector3.Distance(imageTransform.localPosition, randomTarget) > 0.1f)
            {
                imageTransform.localPosition = Vector3.MoveTowards(imageTransform.localPosition, randomTarget, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // ���ѡ�����͹�����������˹�����
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }
}
