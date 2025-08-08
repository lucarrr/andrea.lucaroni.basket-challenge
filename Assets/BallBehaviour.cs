using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    
    void Start()
    {
        GameObject.Destroy(gameObject, 10f);
    }

}
