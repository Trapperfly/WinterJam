using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] float force;
    [SerializeField] Transform shootPoint;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Rigidbody snowball = Instantiate(prefab, shootPoint.position, Quaternion.Euler(Random.Range(0,361), Random.Range(0, 361), Random.Range(0, 361))).GetComponent<Rigidbody>();
            snowball.AddForce(shootPoint.forward * force, ForceMode.Impulse);
        }
    }
}
