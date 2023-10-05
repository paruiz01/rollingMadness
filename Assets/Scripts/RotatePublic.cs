using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePublic : MonoBehaviour
{
    public float RotationSpeed1;
    public float RotationSpeed2;
    public float RotationSpeed3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotationSpeed1 * Time.deltaTime, RotationSpeed2 
            * Time.deltaTime, RotationSpeed3 * Time.deltaTime, Space.World);    
    }
}
