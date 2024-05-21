using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GradOject : MonoBehaviour
{
    [SerializeField]
    private Transform grabPoint;
    [SerializeField]
    private Transform rayPoint;
    [SerializeField]
    private float rayDistance;

    private GameObject grabbedOdject;
    private int layerIndex;
    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("GrapObject");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, transform.right, rayDistance);

        if (hitInfo.collider!=null && hitInfo.collider.gameObject.layer == layerIndex) 
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame && grabbedOdject == null)
            {
                grabbedOdject = hitInfo.collider.gameObject;
                grabbedOdject.GetComponent<Rigidbody2D>().isKinematic = true;
                grabbedOdject.transform.position = grabPoint.position;
                grabbedOdject.transform.SetParent(transform);
            }
            else if (Keyboard.current.spaceKey.wasPressedThisFrame)
            { 
                grabbedOdject.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedOdject.transform.SetParent(null);
                grabbedOdject = null;
            }
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }
}
