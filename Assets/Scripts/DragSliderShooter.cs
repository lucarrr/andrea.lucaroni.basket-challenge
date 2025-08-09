using UnityEngine;
using UnityEngine.UI;

public class DragSliderShooter : MonoBehaviour
{   
    //REFERENCES
    public Slider shotSlider;
    private Shooter player;
    private SliderRegionVisualizer regionVisualizer;

    //FUNCTIONAL VARIABLES
    private Vector2 lastInputPos;
    private bool isDragging = false;
    private float idleTimer = 0f;

    //GAMEPLAY SETTINGS
    public float sliderSpeed = 0.005f;
    private float idleTimeBeforeShoot = 0.4f;
    private float precisionDirectAPShort = 0.99f, precisionDirectAPLong = 1.01f; //Precision necessary for almost perfect Direct Shots
    private float precisionBackboardAPShort = 0.96f, precisionBackboardAPLong = 1.01f; //Precision necessary for almost perfect Backboard Shots
    private float perfectShotThreshold = 0.4f, perfectBackboardThreshold = 0.6f, tolleranceRange= 0.05f, almostPerfectRange = 0.02f;
    private float betweenRegionPadding = 0.1f;
    

    void Start()
    {
        shotSlider.enabled = false;
        regionVisualizer = GetComponent<SliderRegionVisualizer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooter>();
        NewShotThresholds();
    }

    public enum ShotType
    {
        //DirectMiss,
        //BackboardMiss,
        //RimHitDirect,
        //RimHitBackboard,
        //BackboardPerfect,
        //DirectPerfect,
        Direct,
        Backboard
    }

    public struct ShotResult
    {
        public ShotType type;
        public float precision;

        public ShotResult(ShotType t, float p)
        {
            type = t;
            precision = p;
        }
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
            Debug.Log($"Slider value: {shotSlider.value}");
            ShotResult outcome = ShotOutcome(shotSlider.value);
            Debug.Log($"Shot of type: {outcome.type}  with precision: {outcome.precision}"); 
            player.Shoot((int)outcome.type, outcome.precision);
        }
    }

    void ResetSlider()
    {
        isDragging = false;
        idleTimer = 0f;
        shotSlider.value = 0f;
        NewShotThresholds();
    }

    public void NewShotThresholds()
    {
        perfectShotThreshold = Random.Range(0.4f, 1 - 2 * (tolleranceRange + almostPerfectRange) - betweenRegionPadding);
        perfectBackboardThreshold = Random.Range(perfectShotThreshold + tolleranceRange + almostPerfectRange + betweenRegionPadding, 1 - tolleranceRange- almostPerfectRange);
        
        Debug.Log($"Almost Perfect Direct Treshold: {perfectShotThreshold - tolleranceRange - almostPerfectRange} ____" +
            $" Direct Shot: {perfectShotThreshold - tolleranceRange} : {perfectShotThreshold + tolleranceRange} _____" +
            $" Almost Perfect Backboard Treshold: {perfectBackboardThreshold - tolleranceRange - almostPerfectRange} ____" +
            $" Backboard Treshold: {perfectBackboardThreshold - tolleranceRange} : {perfectBackboardThreshold + tolleranceRange} _____");
        
        regionVisualizer.NewShotRegions(perfectShotThreshold, perfectBackboardThreshold, tolleranceRange);
    }

    //Establish the precision of the shot according to the stablished zone in the slider
    private ShotResult ShotOutcome(float sliderValue)
    {
        float precision = 0f;
        float errorOffset = 0f;

        if (sliderValue < perfectShotThreshold - tolleranceRange - almostPerfectRange) //SHORT SHOT
        {
            //INTERPOLATE PRECISION BETWEEN 0 and ALMOST PERFECT SHOOT (0.99f)
            precision= sliderValue * precisionDirectAPShort / perfectShotThreshold;
            return new ShotResult(ShotType.Direct ,precision);
        }
        else if (sliderValue >= perfectShotThreshold - tolleranceRange - almostPerfectRange && sliderValue <= perfectShotThreshold - tolleranceRange) //ALMOST PERFECT DIRECT SHOT
        {
            //LITTLE UNDERSHOOT ERROR BUT THAT ENTERS
            precision = precisionDirectAPShort; 
            return new ShotResult(ShotType.Direct, precision);
        }
        else if (sliderValue >= perfectShotThreshold - tolleranceRange && sliderValue <= perfectShotThreshold + tolleranceRange) //PERFECT DIRECT SHOT
        {   
            precision = 1f;
            return new ShotResult(ShotType.Direct, precision);
        }
        else if (sliderValue > perfectShotThreshold + tolleranceRange && sliderValue < perfectShotThreshold + tolleranceRange + almostPerfectRange) //ALMOST PERFECT DIRECT SHOT
        {   
            //LITTLE OVERSHOOT ERROR BUT THAT ENTERS
            precision = precisionDirectAPLong; 
            return new ShotResult(ShotType.Direct, precision);
        }
        else if (sliderValue > perfectShotThreshold + tolleranceRange + almostPerfectRange && sliderValue < perfectBackboardThreshold - tolleranceRange - almostPerfectRange) //MISS DIRECT
        {   
            //CALCULATE ERROR BY INTERPOLATING THE DISTANCE BETWEEN THE PERFECT VALUE and SLIDER VALUE
            errorOffset = (sliderValue - perfectShotThreshold + tolleranceRange) * 0.5f;
            precision = 1f + errorOffset;
            return new ShotResult(ShotType.Direct, precision);
        }
        else if (sliderValue > perfectBackboardThreshold - tolleranceRange - almostPerfectRange && sliderValue < perfectBackboardThreshold - tolleranceRange) //ALMOST PERFECT BACKBOARD SHOT
        {
            precision = precisionBackboardAPShort;
            return new ShotResult(ShotType.Backboard, precision);
        }
        else if (sliderValue > perfectBackboardThreshold - tolleranceRange && sliderValue < perfectBackboardThreshold + tolleranceRange) //PERFECT BACKBOARD SHOT
        {
            precision = 1f;
            return new ShotResult(ShotType.Backboard, precision);
        }
        else if (sliderValue > perfectBackboardThreshold + tolleranceRange && sliderValue < perfectBackboardThreshold + tolleranceRange + almostPerfectRange) //ALMOST PERFECT BACKBOARD SHOT
        {
            precision = precisionBackboardAPLong;
            return new ShotResult(ShotType.Backboard, precision);
        }
        else if(sliderValue > perfectBackboardThreshold + tolleranceRange) //MISS BACKBOARD LONG
        {
            //CALCULATE ERROR BY INTERPOLATING THE DISTANCE BETWEEN THE PERFECT BACKBOARD VALUE and SLIDER VALUE(0.99f)
            errorOffset = (sliderValue - perfectBackboardThreshold + tolleranceRange) * 0.3f;
            precision = 1f + errorOffset;
            return new ShotResult(ShotType.Backboard, precision);
        }
        else return new ShotResult(ShotType.Direct, 0f);    
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
