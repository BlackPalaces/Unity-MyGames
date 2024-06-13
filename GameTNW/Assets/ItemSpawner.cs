using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Collider2D spawnArea; // Collider2D ที่กำหนดขอบเขตของพื้นที่สุ่ม
    public Transform itemsParent; // Transform ของ Sorting Group ที่เก็บไอเท็ม

    void Start()
    {
        MoveItemsToRandomPositions();
    }

    void MoveItemsToRandomPositions()
    {
        int numberOfItems = itemsParent.childCount;
        for (int i = 0; i < numberOfItems; i++)
        {
            Transform itemTransform = itemsParent.GetChild(i);
            Vector2 randomPosition = GetRandomPositionInSpawnArea();
            itemTransform.position = randomPosition;
        }
    }

    Vector2 GetRandomPositionInSpawnArea()
    {
        // รับขอบเขตของ Collider2D
        Bounds bounds = spawnArea.bounds;

        // สุ่มตำแหน่งภายในขอบเขต
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(randomX, randomY);
    }
}
