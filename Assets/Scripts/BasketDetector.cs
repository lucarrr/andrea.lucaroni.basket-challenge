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
            BallBehaviour ball= coll.gameObject.GetComponent<BallBehaviour>();
            ScoreManager.Instance.BallInBasket(ball.state);
        }
    }

}
