using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckRaycast : MonoBehaviour
{
    public bool grounded = false;
    public float groundedCheckDistance;
    private float bufferCheckDistance = 0.1f; 
    // Start is called before the first frame updat

    // Update is called once per frame
    void Update()
    {
        groundedCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
    }
}
