using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    // ���ҧ����������红����Ţͧ�����
    public string itemName; // ���ͧ͢�����
    public Sprite itemImage; // �ٻ�Ҿ�ͧ�����
    public Image SlotImage;
    // ��Ǩ�ͺ��Ҫ�ͧ���������������������
    private bool hasItem = false;

    // ������ա�ä�ԡ����ͧ�����
    private void OnMouseDown()
    {
        // ��������������㹪�ͧ���� ����ͧ������
        if (hasItem)
        {
            return;
        }

        // �������Ţͧ�����ŧ㹪�ͧ
        itemName = "���������";
        // �������Ҥس���ٻ�Ҿ������������ Assets/Sprites
        itemImage = Resources.Load<Sprite>("Sprites/YourItemImage");

        // ����¹ʶҹ����������
        hasItem = true;
    }

    // �س����ö�����ѧ��ѹ��� � ���ͨѴ��áѺ�����㹪�ͧ�������ͧ���
}
