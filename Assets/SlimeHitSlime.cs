using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHitSlime : MonoBehaviour
{
    [SerializeField] Combining combine;
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Slime collided with slime");
        if (collision.collider.CompareTag("CanCombine"))
        {
            if (!combine.combinedThisFrame) combine.CombineSlimes(gameObject, collision.collider.gameObject);
        }
    }
}
