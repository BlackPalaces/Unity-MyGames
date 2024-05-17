using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // สร้างตัวแปรเพื่อเก็บข้อมูลของไอเท็ม
    public string itemName; // ชื่อของไอเท็ม
    public Sprite itemImage; // รูปภาพของไอเท็ม
    public Image SlotImage;
    // ตรวจสอบว่าช่องนี้มีไอเท็มอยู่หรือไม่
    private bool hasItem = false;

    // เมื่อมีการคลิกที่ช่องไอเท็ม
    private void OnMouseDown()
    {
        // ถ้ามีไอเท็มอยู่ในช่องแล้ว ไม่ต้องทำอะไร
        if (hasItem)
        {
            return;
        }

        // ใส่ข้อมูลของไอเท็มลงในช่อง
        itemName = "ไอเท็มใหม่";
        // สมมติว่าคุณมีรูปภาพไอเท็มในโฟลเดอร์ Assets/Sprites
        itemImage = Resources.Load<Sprite>("Sprites/YourItemImage");

        // เปลี่ยนสถานะเป็นมีไอเท็ม
        hasItem = true;
    }

    // คุณสามารถเพิ่มฟังก์ชันอื่น ๆ เพื่อจัดการกับไอเท็มในช่องนี้ตามต้องการ
}
