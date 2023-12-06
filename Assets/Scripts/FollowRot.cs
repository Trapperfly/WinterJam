using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRot : MonoBehaviour
{
    [SerializeField] Transform follow;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, follow.eulerAngles.y, 0);
    }
}
