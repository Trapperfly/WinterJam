using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject whatToSpawn;
    [SerializeField] bool turnOnGravity;
    [SerializeField] bool active;
    [SerializeField] float spawnAmount;
    [SerializeField] float spawnRate;
    [SerializeField] float spawnDelay;
    [SerializeField] float spawnForce;

    private void Start()
    {
        StartCoroutine(nameof(SpawnOnRepeat));
    }

    IEnumerator SpawnOnRepeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            if (active)
            {
                for (int i = 0; i < spawnAmount; i++)
                {
                    Rigidbody rb = Instantiate(whatToSpawn, transform.position, new Quaternion(), null).GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.AddForce( new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * spawnForce, ForceMode.Impulse);
                    yield return new WaitForSeconds(spawnRate);
                }
            }
            yield return null;
        }
    }
}
