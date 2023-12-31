using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHitSlime : MonoBehaviour
{
    [SerializeField] Combining combine;

    private void Awake()
    {
        combine = GameObject.FindGameObjectWithTag("Global").GetComponent<Global>().combine;
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Slime collided with slime");
        if (collision.collider.CompareTag("CanCombine") && transform.CompareTag("CanCombine"))
        {
            if (!combine.combinedThisFrame) combine.CombineSlimes(gameObject, collision.collider.gameObject);
        }
    }
}
