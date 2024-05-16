using UnityEngine;

public class Book : MonoBehaviour
{
    public Sprite[] pages; // �ٻ�Ҿ�ͧ˹�ҡ�д�ɷ�����
    private SpriteRenderer spriteRenderer;
    private int currentPageIndex = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Sprite Renderer component not found!");
        }
    }

    void Update()
    {
        // ����ͼ���顴�������ͷӡ���Դ˹ѧ��� �������¹˹�ҡ�д��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePage();
        }
    }

      public void ChangePage()
    {
        // ��Ǩ�ͺ�������¹˹�ҡ�д�������˹�ҶѴ�������͹��ѧ
        currentPageIndex++;
        if (currentPageIndex >= pages.Length)
        {
            currentPageIndex = 0; // ��������件֧˹���ش���� ��Ѻ价��˹���á
        }

        // ����¹�ٻ�Ҿ�����˹�ҡ�д�ɵ���ӴѺ
        spriteRenderer.sprite = pages[currentPageIndex];
    }
}
