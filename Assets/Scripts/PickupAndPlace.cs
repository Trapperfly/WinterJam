using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndPlace : MonoBehaviour
{
    float rayLength;
    [SerializeField] float pickupRange;
    [SerializeField] float placementRange;

    [SerializeField] float heldObjectRange;
    [SerializeField] float heldObjectSpeed;

    GameObject heldObject;
    GameObject heldObjectClone;

    [SerializeField] Material valid;
    [SerializeField] Material invalid;
    Material saved;

    [SerializeField] LayerMask mask;

    [SerializeField] Transform placePoint;
    Vector3 savedSpace;

    public bool holding;
    public bool holdingStruct;
    public bool holdingObj;

    float scrollDelta = 0;
    [SerializeField] float scrollSpeedNormal;
    [SerializeField] float scrollSpeedSlow;
    float scrollSpeed;
    bool caps;

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
                //hit structure
                if (hitInfo.transform.gameObject.layer == 14)
                {
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
                    foreach (Transform t in heldObjectClone.transform)
                    {
                        t.GetComponent<MeshRenderer>().material = valid;
                    }
                    holding = true;
                    holdingStruct = true;
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
                //hit object or block
                else
                {
                    heldObject.transform.parent = placePoint;
                    Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
                    if (heldRb)
                    {
                        heldRb.useGravity = false;
                        heldRb.drag = 0;
                        heldRb.angularDrag = 20;
                    }
                    //heldObject.GetComponent<Collider>().isTrigger = true;
                    holding = true;
                    holdingObj = true;
                    rayLength = placementRange;
                    mask |= 1 << LayerMask.NameToLayer("Ground");

                    heldObject.transform.localRotation = new Quaternion(0, 0, 0, 0);

                    center = heldObject.GetComponent<Renderer>().localBounds.center;
                    worldCenter = heldObject.GetComponent<Renderer>().bounds.center;
                    heldObject.transform.localPosition = new Vector3(0,0,0);

                    placePoint.eulerAngles = savedRot;

                    scrollDelta = savedRot.y;
                }
                
            }
            //Placing down
            else if (Input.GetKeyDown(KeyCode.F))
            {
                if (holdingStruct)
                {
                    heldObjectClone.transform.parent = null;

                    heldObject.transform.position = heldObjectClone.transform.position;

                    heldObject.transform.rotation = heldObjectClone.transform.rotation;

                    heldObject = null;
                    Destroy(heldObjectClone);
                    holding = false;
                    holdingObj = false;
                    holdingStruct = false;
                    rayLength = pickupRange;
                    mask &= ~1 << LayerMask.NameToLayer("Ground");
                }
                if (holdingObj)
                {
                    Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
                    if (heldRb)
                    {
                        heldRb.useGravity = true;
                        heldRb.drag = 0;
                        heldRb.angularDrag = 0;
                        heldRb.velocity = Vector3.zero;
                    }
                    heldObject.transform.parent = null;

                    heldObject = null;
                    Destroy(heldObjectClone);
                    holding = false;
                    holdingObj = false;
                    holdingStruct = false;
                    rayLength = pickupRange;
                    mask &= ~1 << LayerMask.NameToLayer("Ground");
                }
            }
        }

        if (!heldObject) { }
        else
        {
            if (heldObject.layer == 14) 
                heldObjectClone.transform.localPosition = new Vector3(-center.x, 0, -center.z);
            else 
                heldObject.transform.localPosition = new Vector3(-center.x, -center.y, -center.z);

            if (holdingObj) savedSpace = transform.position + (transform.forward * heldObjectRange);
            else if (hit && holdingStruct) savedSpace = hitInfo.point;

            if (holdingObj)
            {
                if (!Input.GetMouseButton(1)) placePoint.position = Vector3.Lerp(placePoint.position, savedSpace, heldObjectSpeed);
            }
            else if (holdingStruct)
            {
                if (!Input.GetMouseButton(1)) placePoint.position = savedSpace;
            }

            if (caps) scrollSpeed = scrollSpeedSlow;
            else scrollSpeed = scrollSpeedNormal;

            scrollDelta += Input.mouseScrollDelta.y * scrollSpeed;
            //Quaternion rot = placePoint.rotation;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                placePoint.transform.RotateAround(placePoint.position, placePoint.right, scrollDelta);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                placePoint.transform.RotateAround(placePoint.position, placePoint.forward, scrollDelta);
            }
            else placePoint.transform.RotateAround(placePoint.position, placePoint.up, scrollDelta);

            if (Input.GetKeyDown(KeyCode.X))
            {
                Quaternion nullRot = new Quaternion(0, 0, 0, 0);
                placePoint.rotation = nullRot;
                foreach (Transform t in placePoint.transform) { t.rotation = nullRot; }
            }


            //placePoint.rotation = rot;

            //rot = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z + scrollDelta);

            Debug.Log(scrollDelta);
            scrollDelta = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(placePoint.position, 1);
    }

    private void OnGUI()
    {
        if (Event.current.capsLock) caps = true; else caps = false;
    }
}
