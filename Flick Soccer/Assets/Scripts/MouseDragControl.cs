using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDragControl : MonoBehaviour
{
    private Vector3 fp; //First finger position
    private Vector3 lp; //Last finger position
    private float dragDistance;  //Distance needed for a swipe to register
    public bool triggered = false;
    //variables for determining the shot power and position
    public float power;  //power at which the ball is shot
    private Vector3 footballPos; //initial football position for replacing the ball at the same posiiton
    private float factor = 34f; // keep this factor constant, also used to determine force of shot
    public bool canShoot = true;  //flag to check if shot can be taken
    public int scorePlayer = 0;  //score of player
    public int turn = 0;   //0 for striker, 1 for goalie
    public bool isGameOver = false; //flag for game over detection
    Vector3 oppKickDir;   //direction at which the ball is kicked by opponent
    public int shotsTaken = 0;  //number of rounds of penalties taken
    private bool returned = true;  //flag to check if the ball is returned to its initial position
    public bool isKickedPlayer = false; //flag to check if the player has kicked the ball
    public bool isKickedOpponent = false; //flag to check if the opponent has kicked the ball
    public Rigidbody rb;
    public Text countText;
    private int count;
    private GUIStyle guiStyle = new GUIStyle();
    public TargetColour other;

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
            fp = Input.mousePosition;
            lp = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            lp = Input.mousePosition;

            if (fp.x > lp.x)
            {
                Debug.Log("Mouse moved left");
            }
            else if (fp.x < lp.x)
            {
                Debug.Log("Mouse moved right");
            }


            if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
            {   //It's a drag

                //x and y repesent force to be added in the x, y axes.
                float x = (lp.x - fp.x) / Screen.height * factor;
                float y = (lp.y - fp.y) / Screen.height * factor;
                //Now check what direction the drag was
                //First check which axis
                if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                {   //If the horizontal movement is greater than the vertical movement...

                    if ((lp.x > fp.x) && canShoot)  //If the movement was to the right)
                    {   //Right move
                        rb.AddForce((new Vector3(x, 10, 15)) * power);
                    }
                    else
                    {   //Left move
                        rb.AddForce((new Vector3(x, 10, 15)) * power);
                    }
                }
                else
                {   //the vertical movement is greater than the horizontal movement
                    if (lp.y > fp.y)  //If the movement was up
                    {   //Up move
                        rb.AddForce((new Vector3(x, y, 15)) * power);
                    }
                    else
                    {   //Down move

                    }
                }
            }

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

        canShoot = true;     //set the canshoot flag to true
        returned = true;     //set football returned flag to true as well
        triggered = false;
    }

    IEnumerator DelayAdd()
    {
        yield return new WaitForSeconds(0.2f);  //I have added a delay of 0.2 seconds
        oppKickDir = new Vector3(Random.Range(-4f, 4f), Random.Range(5f, 10f), Random.Range(6f, 12f)); //generate a random x and y value in the range mentioned
        rb.AddForce(oppKickDir, ForceMode.Impulse); //add the force 
    }

    //function to check if its a goal
    void OnTriggerEnter(Collider other)
    {
        //check if the football has triggered an object named GoalLine and triggered is not true
        if (other.gameObject.name == "GoalLine" && !triggered)
        {

            scorePlayer++; //increment the goals tally of player
            count++;
            countText.text = "SCORE: " + count.ToString();
        }
        if (other.gameObject.name == "GoalTargettopleft" && !triggered)
        {
            scorePlayer++; //add aditional point for hot zone
            count++;
            //other.gameObject.SetActive (false);
            //other.gameObject.setActive (true);
            //GetComponent<Renderer>().material.color = Color.green;
        }
        if (other.gameObject.name == "GoalTargettopright" && !triggered)
        {
            scorePlayer++; //add aditional point for hot zone
            count++;
            //other.gameObject.SetActive (false);
            //other.gameObject.setActive (true);
            //GoalTargets.material.color = Color.green;
        }

    }

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
