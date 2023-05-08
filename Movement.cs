using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    [Header("Movement tweaks")]
    public float speed;
    public float maxSpeed;
    public Rigidbody rb;
    public GameObject mainCamera;

    float previousCameraRotation;

    public bool timeToAct; // if timeToAct is true player can then control player, if not then player has to wait for everything else to end
    bool playerIsRotating = false; //The process during player's rotation. Used to avoid a second input if the process is not done yet.

    bool jumpLeft = false;
    bool jumpRight = false;

    bool rotateCameraLeft;
    bool rotateCameraRight;
    bool touchedStep;
    

    private void Start()
    {
        touchedStep = true;
        timeToAct = true;
        previousCameraRotation = 0;

        rotateCameraLeft = false;
        rotateCameraRight = false;
        Physics.gravity = new Vector3(0, -9.81f, 0);

        rb.AddForce(0, 0, speed, ForceMode.Impulse);
    }

    private void Update()
    {       
        GetInput();

        PositionPlayerToCenter();
    }



    public void JumpLeftButton() //On left button input
    {
        if (timeToAct && !playerIsRotating) 
        {
            jumpLeft = true;
            timeToAct = false;
            touchedStep = false;
            playerIsRotating = true;
            Invoke("TimeToAct", 0.17f);
        }
    }

    public void JumpRightButton()//On right button input
    {
        if (timeToAct && !playerIsRotating)
        {
            jumpRight = true;
            timeToAct = false;
            touchedStep = false;
            playerIsRotating = true;
            Invoke("TimeToAct", 0.17f);
        }
    }

    //On Player's Input
    void GetInput()
    {
        if (timeToAct && !playerIsRotating)
        {
            if (Input.GetKeyDown(KeyCode.A) && !jumpLeft)//Jumps left on button press
            {
                jumpLeft = true;
                timeToAct = false;
                touchedStep = false;
                playerIsRotating = true;
                Invoke("TimeToAct", 0.15f);
            }

            else if (Input.GetKeyDown(KeyCode.D) && !jumpRight)//Jumps right on button press
            {
                jumpRight = true;
                timeToAct = false;
                touchedStep = false;
                playerIsRotating = true;
                Invoke("TimeToAct", 0.15f);
            }
        }
    }

    public IEnumerator ControlPlayerSpeed()
    {
        if (speed <= 1.7)
        {
            float timeElapsed = 0;
            float percentageCompleted = 0;

            float initialSpeed = speed;
            float endSpeed = speed + speed * 0.1f;


            while (percentageCompleted <= 1)
            {
                timeElapsed += Time.deltaTime;
                percentageCompleted = timeElapsed / StepGenerator.timeForSecondStep; //time elapsed divided by the time it takes to instantiate the second step for the new theme

                speed = Mathf.Lerp(initialSpeed, endSpeed, percentageCompleted);

                yield return null;
            }
            yield return null; 
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    //Player Movement 
    void MovePlayer()
    {
        rb.AddForce(0, 0, speed, ForceMode.Impulse); // Pushes the player on the Z axis

        if (rb.velocity.magnitude >= maxSpeed)//if the ball velocity has reached the max limit, lower it
        {
            rb.velocity *= 0.955f;
        }

        // If gravity is down
        if (Physics.gravity == new Vector3(0f, -9.81f, 0f))
        {
            if (jumpLeft == true)
            {
                rb.AddForce(-10f, 10f, 0f, ForceMode.Impulse);
                jumpLeft = false;
            }

            if (jumpRight == true)
            {
                rb.AddForce(10f, 10f, 0f, ForceMode.Impulse);
                jumpRight = false;
            }
        }
        //If gravity is left
        else if (Physics.gravity == new Vector3(-9.81f, 0, 0f))
        {
            if (jumpLeft == true)
            {
                rb.AddForce(10f, 10f, 0f, ForceMode.Impulse);
                jumpLeft = false;
            }

            if (jumpRight == true)
            {
                rb.AddForce(10f, -10f, 0f, ForceMode.Impulse);
                jumpRight = false;
            }
        }
        //If gravity is right
        else if (Physics.gravity == new Vector3(9.81f, 0, 0f))
        {
            if (jumpLeft == true)
            {
                rb.AddForce(-10f, -10f, 0f, ForceMode.Impulse);
                jumpLeft = false;
            }

            if (jumpRight == true)
            {
                rb.AddForce(-10f, 10f, 0f, ForceMode.Impulse);
                jumpRight = false;
            }
        }
        //If gravity is up
        else if (Physics.gravity == new Vector3(0f, 9.81f, 0f))
        {
            if (jumpLeft == true)
            {
                rb.AddForce(10f, -10f, 0f, ForceMode.Impulse);
                jumpLeft = false;
            }

            if (jumpRight == true)
            {
                rb.AddForce(-10f, -10f, 0f, ForceMode.Impulse);
                jumpRight = false;
            }
        }
    }

    IEnumerator RotateCameraToDirection()
    {
        Vector3 initialRotation = new Vector3(0f, 0f, previousCameraRotation);
        Vector3 targetRotation;

        float durationOfRotation = 0.15f;
        float timeElapsed = 0f;
        float percentageCompleted = 0f;

        if (rotateCameraRight)
        {
            rotateCameraRight = false;
            targetRotation = new Vector3(0f, 0f, previousCameraRotation - 90);
            previousCameraRotation -= 90;

            while (percentageCompleted < 1)
            {
                timeElapsed += Time.deltaTime;
                percentageCompleted = timeElapsed / durationOfRotation;
                
                mainCamera.transform.rotation = Quaternion.Slerp(Quaternion.Euler(initialRotation), Quaternion.Euler(targetRotation), percentageCompleted);
                yield return null;
            }
        }
        else if (rotateCameraLeft)
        {
            rotateCameraLeft = false;
            targetRotation = new Vector3(0f, 0f, previousCameraRotation + 90);
            previousCameraRotation += 90;

            while (percentageCompleted < 1)
            {
                timeElapsed += Time.deltaTime;
                percentageCompleted = timeElapsed / durationOfRotation;

                mainCamera.transform.rotation = Quaternion.Slerp(Quaternion.Euler(initialRotation), Quaternion.Euler(targetRotation), percentageCompleted);
                yield return null;
            }
        }
        timeToAct = true;
        yield return null;
    }
    
    //TimeToAct restricts how often the player's input will be executed
    void TimeToAct()
    {
        timeToAct = true;
    }

    // Corrects player's position to the center of the platform after the jump
    void PositionPlayerToCenter()
    {
        // If gravity is down
        if (Physics.gravity == new Vector3(0f, -9.81f, 0f) && touchedStep)
        {
           transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        //If gravity is left
        else if (Physics.gravity == new Vector3(-9.81f, 0f, 0f) && touchedStep)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        //If gravity is right
        else if (Physics.gravity == new Vector3(9.81f, 0f, 0f) && touchedStep)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        //If gravity is up
        else if (Physics.gravity == new Vector3(0f, 9.81f, 0f) && touchedStep)
        {
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        }
    }

    // Sets jump bools accordingly on collision
    private void OnCollisionEnter(Collision collision)
    {
        #region Change gravicty direction according to where the player lands
        //if gravity is down
        if (Physics.gravity == new Vector3(0f, -9.81f, 0f))
        {
            if (transform.position.x <= -1.4f)
            {
                Physics.gravity = new Vector3(-9.81f, 0f, 0f);
                rb.velocity = new Vector3(0f,0f, rb.velocity.z);
                rotateCameraRight = true;
            }

            else if (transform.position.x >= 1.4f)
            {
                Physics.gravity = new Vector3(9.81f, 0f, 0f);
                rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
                rotateCameraLeft = true;
            }
        }
        //if gravity is left
        else if (Physics.gravity == new Vector3(-9.81f, 0f, 0f))
        {
            if (transform.position.y <= -1.4f)
            {
                Physics.gravity = new Vector3(0f, -9.81f, 0f);
                rb.velocity = new Vector3(0f, 0f, rb.velocity.z);                
                rotateCameraLeft = true;
            }

            else if (transform.position.y >= 1.4f)
            {
                Physics.gravity = new Vector3(0f, 9.81f, 0f);
                rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
                rotateCameraRight = true;
            }
        }
        //if gravity is right
        else if (Physics.gravity == new Vector3(9.81f, 0f, 0f))
        {
            if (transform.position.y >= 1.4f)
            {
                Physics.gravity = new Vector3(0f, 9.81f, 0f);
                rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
                rotateCameraLeft = true;
            }

            else if (transform.position.y <= -1.4f)
            {
                Physics.gravity = new Vector3(0f, -9.81f, 0f);
                rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
                rotateCameraRight = true;
            }
        }
        //if gravity is up
        else if (Physics.gravity == new Vector3(0f, 9.81f, 0f))
        {
            if (transform.position.x <= -1.4f)
            {
                Physics.gravity = new Vector3(-9.81f, 0f, 0f);
                rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
                rotateCameraLeft = true;
            }

            else if (transform.position.x >= 1.4f)
            {
                Physics.gravity = new Vector3(9.81f, 0f, 0f);
                rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
                rotateCameraRight = true;
            }            
        }

        StartCoroutine(RotateCameraToDirection());
        jumpLeft = false;
        jumpRight = false;
        touchedStep = true;
        playerIsRotating = false;
        #endregion

        if (collision.gameObject.tag == "Enemy Step") 
        {
            GameObject.FindWithTag("GameManager").GetComponent<GameManager>().GameOver();
        }
    }
}