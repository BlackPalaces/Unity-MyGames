using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcDialog
{
    public string characterName; // ���͵���Ф�
    public Sprite characterSprite; // �ٻ�Ҿ����Ф�
    [TextArea(3, 10)] // ���� TextArea ���ͤ����дǡ㹡������ͤ������ �
    public string dialogText; // ��ͤ����ٴ���
    public AudioClip dialogAudio; // ���§�ͧ Dialog
    public float audioVolume = 0.5f;
    public float skipDelay = 3f;
}