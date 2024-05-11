using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX",input.x);
                animator.SetFloat("moveY", input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }
    void Interact()
    {
        // คำนวณเวกเตอร์ทิศทางที่มุมหันของตัวละคร
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));

        // คำนวณตำแหน่งที่ต้องการวาดเส้น Debug Line
        var interactPos = transform.position + facingDir.normalized * 0.3f; // ให้ปรับตำแหน่งเล็กน้อยด้วยการเทรนฟอร์ม 0.3f หรือค่าที่ต้องการ

        Debug.Log("Interact Position: " + interactPos);
        // วาดเส้น Debug Line จากตำแหน่งปัจจุบันไปยังตำแหน่งที่ต้องการ Interact
        Debug.DrawLine(transform.position, interactPos, Color.blue, 1f);

        // ตรวจสอบว่ามี Collider ใดๆ อยู่ในตำแหน่งที่ต้องการ Interact หรือไม่
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);

        // ถ้ามี Collider ใดๆ อยู่ในตำแหน่งที่ต้องการ Interact ให้เรียก Interact method ของ Interactable component ที่อยู่ใน Collider นั้น
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
 
}
