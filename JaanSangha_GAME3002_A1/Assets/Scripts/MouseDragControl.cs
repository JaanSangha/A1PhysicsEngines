using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDragControl : MonoBehaviour
{
    private Vector3 firstP; //First mouse position
    private Vector3 lastP; //Last mouse position
    private float dragDistance;  //Distance needed for a swipe to register
    public bool triggered = false;
    public bool gameIsOver = false;

    //variables for determining the shot power and position
    public float power;  //shot power
    private Vector3 footballPos;    //initial ball position 
    private float factor = 34f; //force of shot
    public bool canShoot = true;  //check if shot can be taken
   // public int scorePlayer = 0;
    public int turn = 0;   
    public bool isGameOver = false; //flag for game over detection
    private bool returned = true;  //check if the ball is returned to its initial position
    public bool isKickedPlayer = false;     //check if the player has kicked the ball
    public Rigidbody rb;
    public Text countText;
    private int count;
    private GUIStyle guiStyle = new GUIStyle();
    public TargetColour other;
    public int shotsTaken = 0;  //shots player has taken
    public GameObject[] shots;  //ui goal records
    public enum SHOT_TYPE       //specify type of shot
    {
        GoalOne = 0,
        GoalTwo = 1,
        GoalThree = 2,
        GoalFour = 3,
        GoalFive = 4,
        MissOne = 5,
        MissTwo = 6,
        MissThree = 7,
        MissFour = 8,
        MissFive = 9,
        TargetOne = 10,
        TargetTwo = 11,
        TargetThree = 12,
        TargetFour = 13,
        TargetFive = 14,
    }   

    public void PresentShot(SHOT_TYPE shottype)
    {
        shots[(int)shottype].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        countText.text = "SCORE: " + count.ToString();
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1;    //set it to 1 on start so as to overcome the effects of restarting the game by script
        dragDistance = Screen.height * 20 / 100; //20% of the screen should be swiped to shoot
        Physics.gravity = new Vector3(0, -20, 0); //reset the gravity of the ball to 20
        footballPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (returned)
        {     //check if the football is in its initial position
            if (turn == 0 && !isGameOver)
            { //if its users turn to shoot and if the game is not over
                playerLogic();
            }
        }
    }

    void playerLogic()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstP = Input.mousePosition;
            lastP = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastP = Input.mousePosition;

            if (Mathf.Abs(lastP.x - firstP.x) > dragDistance || Mathf.Abs(lastP.y - firstP.y) > dragDistance)   //check if it is a mouse drag
            {   
                //force to be added in the x, y axes.
                float x = (lastP.x - firstP.x) / Screen.height * factor;
                float y = ((lastP.y - firstP.y) / Screen.height * factor) / 1.3f;

                //find direction by axis
                if (Mathf.Abs(lastP.x - firstP.x) > Mathf.Abs(lastP.y - firstP.y))  //x > y movement
                {   

                    if ((lastP.x > firstP.x) && canShoot)  //If the movement was to the right)
                    {   //Right move
                        rb.AddForce((new Vector3(x, 10, 15)) * power);
                    }
                    else
                    {   //Left move
                        rb.AddForce((new Vector3(x, 10, 15)) * power);
                    }
                }
                else
                {   //y > x movement (main movement)
                    if (lastP.y > firstP.y)  //make sure movement was up/ forward
                    {  
                        rb.AddForce((new Vector3(x, y, 20)) * power);
                    }
                }
            }

            shotsTaken = shotsTaken + 1;
    
            canShoot = false;
            returned = false;
            isKickedPlayer = true;
            StartCoroutine(ReturnBall());
        }
    }

    IEnumerator ReturnBall()
    {
        yield return new WaitForSeconds(3.0f);  //set a delay of 3 seconds before the ball is returned
        rb.velocity = Vector3.zero;   //set the velocity of the ball to zero
        rb.angularVelocity = Vector3.zero;  //set its angular vel to zero
        transform.position = footballPos;   //re positon it to initial position
        other = GameObject.Find("GoalTargettopleft").GetComponent<TargetColour>();
        other.ResetColour();

        canShoot = true;    
        returned = true;     //set ball returned 
        triggered = false;
    }

    IEnumerator DelayAdd()
    {
        yield return new WaitForSeconds(0.2f);  //I have added a delay of 0.2 seconds
    }

    //function to check if its a goal
    void OnTriggerEnter(Collider other)
    {
        //check if the football has triggered an object named GoalLine and triggered is not true
        if (other.gameObject.name == "GoalLine" && !triggered)
        {
            triggered = true;   // make sure ball doesnt hit goal line twice
            count++;
            countText.text = "SCORE: " + count.ToString();
            if (shotsTaken == 1)
            {
                PresentShot(SHOT_TYPE.GoalOne);
            }
            else if (shotsTaken == 2)
            {
                PresentShot(SHOT_TYPE.GoalTwo);
            }
            else if (shotsTaken == 3)
            {
                PresentShot(SHOT_TYPE.GoalThree);
            }
            else if (shotsTaken == 4)
            {
                PresentShot(SHOT_TYPE.GoalFour);
            }
            else if (shotsTaken == 5)
            {
                PresentShot(SHOT_TYPE.GoalFive);
                gameIsOver = true;
            }
        }
        if (other.gameObject.name == "GoalTargettopleft" || other.gameObject.name == "GoalTargettopright" && !triggered)
        {
            triggered = true;
            count += 2; 
            countText.text = "SCORE: " + count.ToString();
            if (shotsTaken == 1)
            {
                PresentShot(SHOT_TYPE.TargetOne);
            }
            else if (shotsTaken == 2)
            {
                PresentShot(SHOT_TYPE.TargetTwo);
            }
            else if (shotsTaken == 3)
            {
                PresentShot(SHOT_TYPE.TargetThree);
            }
            else if (shotsTaken == 4)
            {
                PresentShot(SHOT_TYPE.TargetFour);
            }
            else if (shotsTaken == 5)
            {
                PresentShot(SHOT_TYPE.TargetFive);
                gameIsOver = true;
            }
        }
  
        if (other.gameObject.name == "MissLineLeft" || other.gameObject.name == "MissLineRight" || other.gameObject.name == "MissLineAbove" && !triggered)
        {
            if (shotsTaken == 1)
            {
                PresentShot(SHOT_TYPE.MissOne);
            }                       
            else if (shotsTaken == 2)
            {                     
                PresentShot(SHOT_TYPE.MissTwo);
            }                         
            else if (shotsTaken == 3) 
            {                         
                PresentShot(SHOT_TYPE.MissThree);
            }                        
            else if (shotsTaken == 4) 
            {                       
                PresentShot(SHOT_TYPE.MissFour);
            }                        
            else if (shotsTaken == 5) 
            {                        
                PresentShot(SHOT_TYPE.MissFive);
                DelayAdd();
                gameIsOver = true;
            }
        }

    }
    //add score to screen
    void OnGUI()
    {
        //check if game is not over, if so, display the score
        if (!isGameOver)
        {
            guiStyle.fontSize = 50; //change the font size
            guiStyle.normal.textColor = Color.black;
            GUI.Label(new Rect(30, 30, Screen.width / 3, Screen.height / 4), "SCORE: " + (count).ToString(), guiStyle); //display player's score
        }
    }
}
