using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketDetector : MonoBehaviour
{   
    public int baskets = 0;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Ball"))
        {
            baskets ++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
