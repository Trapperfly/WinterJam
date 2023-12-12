using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player")) { }
        else
        {
            Debug.Log("Collided");
            Rigidbody rb = GetComponent<Rigidbody>();
            GetComponent<Collider>().enabled = false;
            Destroy(rb);

            //rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
            //rb.isKinematic = true; rb.useGravity = false; 

            transform.position = collision.GetContact(0).point;
            transform.parent = collision.transform;
            transform.localRotation = new Quaternion(0, 0, 0, 0);

            if (transform.parent != null)
            {
                if (transform.localScale == Vector3.one) { }
                else
                {
                    Vector3 pScale = transform.parent.localScale;
                    //transform.parent.GetComponent<Renderer>().bounds.extents * 2;
                    transform.localScale = new Vector3(1 / pScale.x, 1 / pScale.y, 1 / pScale.z);
                }
            }

        }
        
    }
}
