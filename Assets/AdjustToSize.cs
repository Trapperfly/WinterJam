using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustToSize : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float modifier;
    [SerializeField] bool adjust;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (adjust) { 
            ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main; 
            main.startSize = modifier + target.localScale.x; 
        }
    }
}
