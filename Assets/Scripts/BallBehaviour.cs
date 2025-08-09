using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public enum BallState
    {   
        CleanShot,
        TouchedBackboard,
        TouchedRim,
    }

    public BallState state;

    void Start()
    {   
        state = BallState.CleanShot;
        GameObject.Destroy(gameObject, 10f);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Backboard"))
        {
            state = BallState.TouchedBackboard;
        }else if (col.gameObject.CompareTag("Rim") && state == BallState.CleanShot)
        {
            state= BallState.TouchedRim;
        }
    }

}
