using DemoInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float sensivity = 1f;
    [SerializeField] private Vector3 _initialPositionOffset = new Vector3(0, 400, -300);
    private Vector2 turn = new Vector2(0, -50f);

    [SerializeField] private GameObject _demoProviderObject;
    private DemoProvider _demoProvider;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _demoProvider = _demoProviderObject.GetComponent<DemoProvider>();
        InitialCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        MoveCamera();
    }

    private void InitialCameraPosition()
    {
        // Find the position of a player, so camera can be moved to correct area
        List<long> players = _demoProvider.Players.GetPlayers();
        Vector? firstPlayerPosition = null;
        bool foundPosition = false;
        for (int tick = 0; tick <= _demoProvider.FinalTick; tick++)
        {
            foreach (long steamId in players)
            {
                Vector? position = _demoProvider.Players.GetPlayerPosition(steamId, tick);
                if (position != null)
                {
                    firstPlayerPosition = position;
                    foundPosition = true;
                    break;
                }
            }
            if (foundPosition)
            {
                break;
            }
        }
        
        Vector3 firstPlayerPositionVector = Util.DemoInfoVecToVec3(firstPlayerPosition);
        //Debug.Log($"Found player position: {firstPlayerPositionVector}");
        transform.position = firstPlayerPositionVector + _initialPositionOffset;
    }

    private Vector3 GetKeyboardInput()
    {
        Vector3 input = Vector3.zero;
        // Z-axis
        if (Input.GetKey(KeyCode.W))
        {
            input.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input.z -= 1;
        }

        // X-axis
        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            input.x -= 1;
        }

        // Y-axis
        if (Input.GetKey(KeyCode.Space))
        {
            input.y += 1;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            input.y -= 1;
        }
        return input;
    }

    private void MoveCamera()
    {
        // Handle Movement
        Vector3 input = GetKeyboardInput();
        input *= Time.deltaTime + speed;
        Vector3 movementDelta = input.x * transform.right + input.z * transform.forward + input.y * Vector3.up;
        transform.position += movementDelta;

        // Handle Mouse Movement
        turn.x += Input.GetAxis("Mouse X") * sensivity;
        turn.y += Input.GetAxis("Mouse Y") * sensivity;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
