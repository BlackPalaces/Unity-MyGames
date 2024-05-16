using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcDialog
{
    public string characterName; // ชื่อตัวละคร
    public Sprite characterSprite; // รูปภาพตัวละคร
    [TextArea(3, 10)] // เพิ่ม TextArea เพื่อความสะดวกในการใส่ข้อความยาว ๆ
    public string dialogText; // ข้อความพูดคุย
    public AudioClip dialogAudio; // เสียงของ Dialog
    public float audioVolume = 0.5f;
    public float skipDelay = 3f;
}