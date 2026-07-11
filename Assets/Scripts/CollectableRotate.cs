using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableRotate : MonoBehaviour
{
    [SerializeField] int rotateSpeed = 1;
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);

    }
}
