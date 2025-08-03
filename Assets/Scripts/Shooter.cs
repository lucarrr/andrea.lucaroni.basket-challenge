using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform ring, backboard;
    public GameObject ballPrefab;
    public Transform shootingStart;

    public float directShootingAngle = 70f, backboardShootingAngle = 68f;
    public float precision =1f;
    private Vector3 distToHoop;

    void Start()
    {
        if (ring == null) Debug.LogWarning("Ring transform not assigned to shooter");
        if (backboard == null) Debug.LogWarning("Backboard transform not assigned to shooter");
    }

    void ShootToRing(float precision, float angle)
    {

        GameObject currentBall = Instantiate(ballPrefab, shootingStart.position, Quaternion.identity);
        Vector3 vel = ComputeVelocity(angle, ring.position);

        //Add and offset according to how precise the user was
        Vector3 errorOffset = new Vector3(0.2f, .5f, .2f) * (1-precision);

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.velocity = vel  /* * randomizedOffset */;
        rb.useGravity = true;
    }

    void ShootToBackboard(float precision, float angle)
    {

        GameObject currentBall = Instantiate(ballPrefab, shootingStart.position, Quaternion.identity);

        //Compute the position of a mirrored bascket behind the backboard, for correctly bouncing in the rim
        Vector3 backboardNormal = backboard.transform.forward;
        Vector3 backboardNormalized = new Vector3(backboard.position.x, ring.position.y, backboard.position.z); 

        Vector3 toHoop = ring.position - backboardNormalized;
        Vector3 reflectedHoop = ring.position - 2 * Vector3.Dot(toHoop, backboardNormal) * backboardNormal;

        Debug.DrawRay(backboard.position, toHoop, Color.yellow, 3f);
        Debug.DrawLine(backboardNormalized,  reflectedHoop, Color.green, 3f);

        Vector3 vel = ComputeVelocity(angle, reflectedHoop);

        //Add and offset according to how precise the user was
        Vector3 errordOffset = new Vector3(0.2f, .5f, .2f) * (1 - precision);

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.velocity = vel /* * randomizedOffset */;
        rb.useGravity = true;
    }

    private Vector3 ComputeVelocity(float angleDeg, Vector3 target)
    {   
        float gravity = Mathf.Abs(Physics.gravity.y);

        //Distance from player to The Ring
        Vector3 distToHoop = (target - shootingStart.position);

        Debug.Log(target);
        Debug.Log(shootingStart.position);

        //Distance vector only on horizontal plane
        Vector3 distToHoopXZ = new Vector3(distToHoop.x, 0f, distToHoop.z);

        //Debug.Log(distToHoop);
        //Debug.DrawLine(shootingStart.position, shootingStart.position +  distToHoop, Color.red, 2f);
        //Debug.DrawLine(shootingStart.position, shootingStart.position +  distToHoopXZ, Color.blue, 2f);

        float horizontalDist = distToHoopXZ.magnitude;
        float verticalDist = distToHoop.y;
        float angleRad = angleDeg * Mathf.Deg2Rad;

        float cosAngle = Mathf.Cos(angleRad);
        float sinAngle = Mathf.Sin(angleRad);

        //Check that the angle is between feasible shot limits
        if (cosAngle < 0.01f) return Vector3.zero;

        //Ballistic trajectory
        float speedSq = (gravity * horizontalDist * horizontalDist) /
            (2 * (horizontalDist * Mathf.Tan(angleRad) - verticalDist) * cosAngle * cosAngle);

        //Invalid Speed - avoid to do sqrt of a illegal value
        if (speedSq <= 0) return Vector3.zero;

        float speed = Mathf.Sqrt(speedSq);

        //Horizontal dir vector
        Vector3 dirHor = distToHoopXZ.normalized;

        //The composite velocity of the splitted horizontal and vertical components
        Vector3 velocity = dirHor *  speed * cosAngle + Vector3.up * speed * sinAngle;

        Debug.DrawRay(shootingStart.position, shootingStart.position + velocity, Color.green, 3f);
        return velocity;
    }

    
    void Update()
    {
        if(Input.GetKeyDown("p")) ShootToRing(precision,directShootingAngle);    
        if(Input.GetKeyDown("o")) ShootToBackboard(precision,backboardShootingAngle);    
    }
}
