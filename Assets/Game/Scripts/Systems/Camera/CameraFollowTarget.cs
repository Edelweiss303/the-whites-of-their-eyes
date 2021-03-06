using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : ControllableBase
{
    public Transform followTarget;
    public float followSpeed;
    public Vector3 followOffset;


    void LateUpdate()
    {
        transform.position = followTarget.position + followOffset;
        transform.rotation = followTarget.rotation;
    }
     
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
