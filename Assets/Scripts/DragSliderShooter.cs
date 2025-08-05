using UnityEngine;
using UnityEngine.UI;

public class DragSliderShooter : MonoBehaviour
{
    public Slider shotSlider;
    public float sliderSpeed = 0.005f;
    public float idleTimeBeforeShoot = 0.4f;

    private Vector2 lastInputPos;
    private bool isDragging = false;
    private float idleTimer = 0f;

    void Start()
    {
        shotSlider.enabled = false;
    }


    void Update()
    {
        Vector2 currentInput;
        bool inputDown = GetInputDown(out currentInput);
        bool inputHeld = GetInputHeld(out currentInput);
        bool inputUp = GetInputUp();

        if (inputDown) // First Input Handler
        {
            isDragging = true;
            lastInputPos = currentInput;
            idleTimer = 0f;
        }

        if (inputHeld && isDragging) // Moving finger/mouse handler
        {
            float deltaY = currentInput.y - lastInputPos.y;
            if (deltaY < 0) { return; }
            shotSlider.value = Mathf.Clamp01(shotSlider.value + deltaY * sliderSpeed);
            lastInputPos = currentInput;

            // Reset timer while dragging
            idleTimer = 0f;
        }

        if (!inputHeld && isDragging) //Stationary Handler
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTimeBeforeShoot)
            {
                ResetSlider();
            }
        }

        if (inputUp)  //Finished dragging Handler
        {
            //SHOOT
        }
    }

    void ResetSlider()
    {
        isDragging = false;
        idleTimer = 0f;
        shotSlider.value = 0f;
    }


    //Check that the mouse/finger are starting to drag
    bool GetInputDown(out Vector2 pos)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            pos = Input.GetTouch(0).position;
            return true;
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
            return true;
        }
#endif
        pos = Vector2.zero;
        return false;
    }


    //Check that the mouse/finger are still down, either moving or stationary
    bool GetInputHeld(out Vector2 pos)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                pos = touch.position;
                return true;
            }
        }
#else
        if (Input.GetMouseButton(0))
        {
            pos = Input.mousePosition;
            return true;
        }
#endif
        pos = Vector2.zero;
        return false;
    }

    //Check if the mouse/finger finished to be down
    bool GetInputUp()
    {
#if UNITY_ANDROID || UNITY_IOS
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
#else
        return Input.GetMouseButtonUp(0);
#endif
    }
}
