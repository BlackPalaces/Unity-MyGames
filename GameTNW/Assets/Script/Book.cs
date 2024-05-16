using UnityEngine;

public class Book : MonoBehaviour
{
    public Sprite[] pages; // รูปภาพของหน้ากระดาษทั้งหมด
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
        // เมื่อผู้ใช้กดปุ่มหรือทำการเปิดหนังสือ ให้เปลี่ยนหน้ากระดาษ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePage();
        }
    }

      public void ChangePage()
    {
        // ตรวจสอบว่าเปลี่ยนหน้ากระดาษให้เป็นหน้าถัดไปหรือย้อนหลัง
        currentPageIndex++;
        if (currentPageIndex >= pages.Length)
        {
            currentPageIndex = 0; // เมื่อเล่นไปถึงหน้าสุดท้าย กลับไปที่หน้าแรก
        }

        // เปลี่ยนรูปภาพให้เป็นหน้ากระดาษตามลำดับ
        spriteRenderer.sprite = pages[currentPageIndex];
    }
}
