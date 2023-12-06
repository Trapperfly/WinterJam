using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndPlace : MonoBehaviour
{
    float rayLength;
    [SerializeField] float pickupRange;
    [SerializeField] float placementRange;

    GameObject heldObject;
    GameObject heldObjectClone;

    [SerializeField] Material valid;
    [SerializeField] Material invalid;
    Material saved;

    [SerializeField] LayerMask mask;

    [SerializeField] Transform placePoint;
    Vector3 savedSpace;


    public bool holding;

    float scrollDelta = 0;
    [SerializeField] float scrollSpeedNormal;
    [SerializeField] float scrollSpeedSlow;
    float scrollSpeed;

    Vector3 center;
    Vector3 worldCenter;

    private void Start()
    {
        rayLength = pickupRange;
    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(ray, out hitInfo, rayLength, mask);

        if (hit || holding)
        {
            //Picking up
            if (!holding && Input.GetKeyDown(KeyCode.F))
            {
                heldObject = hitInfo.transform.gameObject;
                Vector3 savedRot = hitInfo.transform.eulerAngles;

                heldObjectClone = Instantiate(heldObject, placePoint);
                Rigidbody heldRb = heldObjectClone.GetComponent<Rigidbody>();
                if (heldRb)
                {
                    heldRb.isKinematic = true;
                    heldRb.useGravity = false;
                    heldRb.drag = 0;
                }
                heldObjectClone.GetComponent<Collider>().isTrigger = true;
                heldObjectClone.GetComponent<MeshRenderer>().material = valid;
                holding = true;
                rayLength = placementRange;
                heldObjectClone.layer = 0;
                mask |= 1 << LayerMask.NameToLayer("Ground");

                heldObjectClone.transform.localRotation = new Quaternion(0, 0, 0, 0);

                center = heldObjectClone.GetComponent<Renderer>().localBounds.center;
                worldCenter = heldObjectClone.GetComponent<Renderer>().bounds.center;
                heldObjectClone.transform.localPosition = new Vector3(-center.x, -center.y, -center.z);

                placePoint.eulerAngles = savedRot;

                scrollDelta = savedRot.y;
            }
            //Placing down
            else if (Input.GetKeyDown(KeyCode.F))
            {
                heldObjectClone.transform.parent = null;

                heldObject.transform.position = heldObjectClone.transform.position;

                heldObject.transform.rotation = heldObjectClone.transform.rotation;

                heldObject = null;
                Destroy(heldObjectClone);
                holding = false;
                rayLength = pickupRange;
                mask &= ~1 << LayerMask.NameToLayer("Ground");
            }
        }

        if (!heldObject) { }
        else
        {
            scrollDelta += Input.mouseScrollDelta.y * scrollSpeed;
            placePoint.rotation = Quaternion.Euler(0, scrollDelta, 0);
            if (heldObject.layer == 14) 
                heldObjectClone.transform.localPosition = new Vector3(-center.x, 0, -center.z);
            else 
                heldObjectClone.transform.localPosition = new Vector3(-center.x, -center.y, -center.z);

            if (hit) savedSpace = hitInfo.point;
            if (!Input.GetMouseButton(1)) placePoint.position = savedSpace;

            if (Input.GetKey(KeyCode.LeftShift)) scrollSpeed = scrollSpeedSlow;
            else scrollSpeed = scrollSpeedNormal;

            Debug.Log(scrollDelta);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(placePoint.position, 1);
    }
}
