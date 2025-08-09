using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform behindPlayerPosition; // Start camera position
    public Transform basketTarget;         // Position near basket
    public float riseHeight = 1f;           // How much higher to go in stage 1
    public float stage1Duration = 0.5f;
    public float stage2Duration = 1f;

    private Coroutine camMoveCoroutine;

    void OnEnable()
    {
        GameEvents.OnRelocatePlayer += MoveBehindPlayer;
        GameEvents.OnShootStarted += MoveUpAndForward;
    }

    void OnDisable()
    {
        GameEvents.OnRelocatePlayer -= MoveBehindPlayer;
        GameEvents.OnShootStarted -= MoveUpAndForward;
    }

    void Start()
    {
        MoveBehindPlayer();
    }

    void MoveBehindPlayer()
    {
        if (camMoveCoroutine != null) StopCoroutine(camMoveCoroutine);
        transform.position = behindPlayerPosition.position;
        transform.LookAt(basketTarget);
    }

    void MoveUpAndForward()
    {
        if (camMoveCoroutine != null) StopCoroutine(camMoveCoroutine);
        camMoveCoroutine = StartCoroutine(MoveCameraSequence());
    }

    IEnumerator MoveCameraSequence()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 upPos = startPos + Vector3.up * riseHeight;
        float t = 0;
        while (t < stage1Duration)
        {
            t += Time.deltaTime;
            float lerpT = t / stage1Duration;
            transform.position = Vector3.Lerp(startPos, upPos, lerpT);
            transform.LookAt(basketTarget.position);
            yield return null;
        }

        Vector3 finalPos = upPos + ((basketTarget.position - upPos)  *  0.3f);
        t = 0;
        while (t < stage2Duration)
        {
            t += Time.deltaTime;
            float lerpT = t / stage2Duration;
            transform.position = Vector3.Lerp(upPos, finalPos, lerpT);
            transform.LookAt(basketTarget.position);
            yield return null;
        }
    }
}