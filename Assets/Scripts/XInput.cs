using UnityEngine;
using XInputDotNetPure; // Required in C#

public class XInput : MonoBehaviour
{
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Rigidbody body;
    private float boostPower = 0;
    public float Speed = 5f;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    // Use this for initialization
    void Start()
    {
        // No need to initialize anything for the plugin
        //controller = gameObject.GetComponent<CharacterController>();
        body = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // SetVibration should be sent in a slower rate.
        // Set vibration according to triggers
        //GamePad.SetVibration(playerIndex, state.Triggers.Left, state.Triggers.Right);
    }

    // Update is called once per frame
    void Update()
    {
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);
        
        /*if(groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        */

        //Vector3 move = new Vector3(state.ThumbSticks.Left.X, 0, state.ThumbSticks.Left.Y);
        //Vector3 move = new Vector3(0, 0, 1);
        //controller.Move(move * Time.deltaTime * playerSpeed);
        transform.localRotation *= Quaternion.Euler(0.0f, state.ThumbSticks.Left.X * 100.0f * Time.deltaTime, 0.0f);
        //if (body.velocity.sqrMagnitude < 50 && state.Buttons.A != ButtonState.Pressed)
        if (state.Buttons.A != ButtonState.Pressed && body.velocity.sqrMagnitude < maxSpeed)
        {
            body.AddForce(gameObject.transform.forward*acceleration, ForceMode.Acceleration);
            //body.AddForce(gameObject.transform.forward * playerSpeed);
        }
        
        playerVelocity.y += gravityValue * Time.deltaTime;

        Debug.Log(groundedPlayer);

        // Detect if a button was pressed this frame
        /*if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed)
        {
        */
        if(state.Buttons.A == ButtonState.Pressed) {
            if (boostPower < 500)
            {
                boostPower += 6;
            }
        }

        // Detect if a button was released this frame
        if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released)
        {
            body.AddForce(gameObject.transform.forward * boostPower);
            boostPower = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            groundedPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            groundedPlayer = false;
        }
    }

    /*void OnGUI()
    {
        string text = "Use left stick to turn the cube, hold A to change color\n";
        text += string.Format("IsConnected {0} Packet #{1}\n", state.IsConnected, state.PacketNumber);
        text += string.Format("\tTriggers {0} {1}\n", state.Triggers.Left, state.Triggers.Right);
        text += string.Format("\tD-Pad {0} {1} {2} {3}\n", state.DPad.Up, state.DPad.Right, state.DPad.Down, state.DPad.Left);
        text += string.Format("\tButtons Start {0} Back {1} Guide {2}\n", state.Buttons.Start, state.Buttons.Back, state.Buttons.Guide);
        text += string.Format("\tButtons LeftStick {0} RightStick {1} LeftShoulder {2} RightShoulder {3}\n", state.Buttons.LeftStick, state.Buttons.RightStick, state.Buttons.LeftShoulder, state.Buttons.RightShoulder);
        text += string.Format("\tButtons A {0} B {1} X {2} Y {3}\n", state.Buttons.A, state.Buttons.B, state.Buttons.X, state.Buttons.Y);
        text += string.Format("\tSticks Left {0} {1} Right {2} {3}\n", state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y, state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), text);
    }
    */
}
