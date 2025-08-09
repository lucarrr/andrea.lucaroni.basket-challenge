using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootinSpotsManager : MonoBehaviour
{
    public Transform[] shootingSpots;
    public Transform rim;
    
    public Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void SwitchPos()
    {
        int randomSpot = Random.Range(0, shootingSpots.Length);
        player.position= shootingSpots[randomSpot].position;
        player.LookAt(new Vector3 (rim.position.x, player.position.y, rim.position.z));
    }

}
