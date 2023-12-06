using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] float fireRate;
    [SerializeField] GameObject prefab;
    [SerializeField] float force;
    [SerializeField] Transform shootPoint;
    float timer;
    private void Start()
    {
        timer = 60 / fireRate;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {   
            if (timer >= 60 / fireRate)
            {
                Rigidbody snowball = Instantiate(prefab, shootPoint.position, Quaternion.Euler(Random.Range(0, 361), Random.Range(0, 361), Random.Range(0, 361))).GetComponent<Rigidbody>();
                snowball.AddForce(shootPoint.forward * force, ForceMode.Impulse);
                timer = 0;
            }
        }
        timer += 60 * Time.deltaTime;
    }
}
