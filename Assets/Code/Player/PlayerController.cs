using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _hurtEffect;
    [SerializeField] private GameObject _hurtTrail;

    [Header("Movement Parameters")]
    [SerializeField] private float m_jumpBufferTime;
    [SerializeField] private float m_coyoteTime;
    [SerializeField] private int m_maxSpeed, m_acceleration, m_deceleration, m_jumpForce;
    [SerializeField] private float m_lowJumpModifier, m_fallModifier;
    [SerializeField] private bool visualizeRaycasts = false;
    [SerializeField] private float m_jumpRaycastLength = 0.5f;

    [Header("Layers Settings")]
    [SerializeField] private LayerMask deathContactLayers;
    [SerializeField] private string baseLayer;
    [SerializeField] private string noCollideLayer;

    [Header("Player Number Icon")]
    public GameObject playerNumIcon;

    [Header("DEBUG: No assignments")]
    public bool m_isGrounded;
    public bool is_falling = false;
    public float jumpBufferTimeCounter;
    public float coyoteTimeCounter;
    public Transform[] m_jumpRaycastOrigin;

    public int PlayerIndex { get; private set; }
    public string PlayerName { get; set; }
    public int Score { get; private set; }

    private Animator m_animator;
    private BoxCollider2D m_collider;
    private Rigidbody2D m_rb;
    private float m_jumpInput, m_xAxisInput, m_yAxisInput;
    private PlayerInput _playerInput;
    private bool _isDead, _isFinished = false;

    public void SetPlayerIndex(int index)
    {
        PlayerIndex = index;
    }

    public void Setup(PlayerInput input)
    {
        _playerInput = input;
        switchActionMap();
    }

    private void Awake()
    {
        //_playerInput = GetComponent<PlayerInput>();
        //_customization = GetComponent<PlayerCustomization>();
    }

    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
        
        setNumbIcon();
        SpawnRaycastAtCollider();
    }

    public void setNumbIcon()
    {
        Transform playerNumIconTransform = transform.Find("PlayerNumIcon");
        playerNumIcon = playerNumIconTransform?.gameObject;
        if (playerNumIcon != null)
        {
            Animator playerNumIconAnimator = playerNumIcon.GetComponent<Animator>();
            switch (PlayerIndex)
            {
                case 0:
                    playerNumIconAnimator.Play("icon_1");
                    break;
                case 1:
                    playerNumIconAnimator.Play("icon_2");
                    break;
                case 2:
                    playerNumIconAnimator.Play("icon_3");
                    break;
                case 3:
                    playerNumIconAnimator.Play("icon_4");
                    break;
            }
        }
        else Debug.LogError("Could not find PlayerNumIcon");
    }

    [ContextMenu("Spawn Raycast at Player Collider")]
    private void SpawnRaycastAtCollider()
    {
        if (!m_collider)
        {
            m_collider = GetComponent<BoxCollider2D>();
        }

        // Delete existing raycast origins if any
        Transform raycastOriginsTransform = transform.Find("JumpRaycastOrigins");
        if (raycastOriginsTransform != null)
        {
            DestroyImmediate(raycastOriginsTransform.gameObject);
        }

        GameObject newRaycastOrigins = new("JumpRaycastOrigins");
        newRaycastOrigins.transform.parent = transform;

        // Create 4 raycast origins at the top of the player's collider
        m_jumpRaycastOrigin = new Transform[4];
        Vector3 offset = new(m_collider.offset.x, m_collider.offset.y);
        m_jumpRaycastOrigin[0] = new GameObject("LeftJumpRaycastOrigin").transform;
        m_jumpRaycastOrigin[0].position = transform.position - new Vector3(m_collider.size.x / 2.0f, -m_collider.size.y / 1.9f) + offset;
        m_jumpRaycastOrigin[1] = new GameObject("MiddleLeftJumpRaycastOrigin").transform;
        m_jumpRaycastOrigin[1].position = transform.position - new Vector3(m_collider.size.x / 4.0f, -m_collider.size.y / 1.9f) + offset;
        m_jumpRaycastOrigin[2] = new GameObject("MiddleRightJumpRaycastOrigin").transform;
        m_jumpRaycastOrigin[2].position = transform.position + new Vector3(m_collider.size.x / 4.0f, m_collider.size.y / 1.9f) + offset;
        m_jumpRaycastOrigin[3] = new GameObject("RightJumpRaycastOrigin").transform;
        m_jumpRaycastOrigin[3].position = transform.position + new Vector3(m_collider.size.x / 2.0f, m_collider.size.y / 1.9f) + offset;

        // Set the raycast origins as children of the JumpRaycastOrigins object
        foreach (Transform origin in m_jumpRaycastOrigin)
        {
            origin.parent = newRaycastOrigins.transform;
        }

        // Visualize raycasts in editor for testing
        for (int i = 0; i < m_jumpRaycastOrigin.Length; i++)
        {
            Debug.DrawRay(m_jumpRaycastOrigin[i].position, Vector3.up * m_jumpRaycastLength, Color.green, 5f);
        }
    }

    [ContextMenu("Set Raycast from Children")]
    private void SetRaycastFromChildren()
    {
        Transform raycastOriginsTransform = transform.Find("JumpRaycastOrigins");
        if (raycastOriginsTransform != null)
        {
            m_jumpRaycastOrigin = new Transform[raycastOriginsTransform.childCount];
            for (int i = 0; i < raycastOriginsTransform.childCount; i++)
            {
                m_jumpRaycastOrigin[i] = raycastOriginsTransform.GetChild(i);
                // Visualize raycasts in editor for testing
                Debug.DrawRay(m_jumpRaycastOrigin[i].position, Vector3.up * m_jumpRaycastLength, Color.green, 5f);
            }
        }
        else Debug.LogError("Could not find JumpRaycastOrigins");
    }

    public void switchActionMap()
    {
        if (_playerInput == null)
        {
            Debug.Log("PlayerInput component is not assigned.");
            return;
        }
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameManager.GameStates.MainMenu:
            case GameManager.GameStates.RoundSelect:
            case GameManager.GameStates.Lobby:
                _playerInput.SwitchCurrentActionMap("UI");
                break;
            case GameManager.GameStates.Scoreboard:
            case GameManager.GameStates.CoinLevel:
            case GameManager.GameStates.RaceLevel:
            case GameManager.GameStates.SurviveLevel:
                _playerInput.SwitchCurrentActionMap("Keyboard");
                break;
        }
        Debug.Log($"Switched action map to {_playerInput.currentActionMap.name} for player {PlayerIndex}");
        activateInputs();
    }

    private void freezeInputs()
    {
        if (_playerInput == null)
        {
            Debug.Log("PlayerInput component is not assigned.");
            return;
        }

        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.RaceLevel||GameManager.Instance.CurrentGameState == GameManager.GameStates.SurviveLevel
            || GameManager.Instance.CurrentGameState == GameManager.GameStates.CoinLevel) //don't deactivate UI inputs
        {
            _playerInput.DeactivateInput();
        }
    }

    public void activateInputs()
    {
        if (_playerInput == null)
        {
            Debug.Log("PlayerInput component is not assigned.");
            return;
        }

        if(m_animator) //Clear these in case
        {
            m_collider.contactCaptureLayers = LayerMask.NameToLayer("Everything");
            m_animator.ResetTrigger("Cheer");
            m_animator.ResetTrigger("Angry");
            this.gameObject.layer = LayerMask.NameToLayer(baseLayer);
        }

        _isFinished = false;

        _playerInput.ActivateInput();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        Debug.Log("Player is submitting");
        if(GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby)
        {
            if (context.started)
            {
                // If in the lobby, fill the start bar
                LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
                if (lobbyUIManager != null)
                {
                    lobbyUIManager.FillStartBar(this, context.control.device);
                }
            }
            else if (context.canceled)
            {
                // If the action is canceled, reset the start bar
                LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
                if (lobbyUIManager != null)
                {
                    lobbyUIManager.CancelFillStartBar(context.control.device);
                }
            }
        }   
        else if(GameManager.Instance.CurrentGameState == GameManager.GameStates.RoundSelect) //maybe add UI
        {
            if(context.started)
            {
                GameManager.Instance.StartMultiplayerGame();
            }
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameStates.RoundSelect)
            {
                Vector2 input = context.ReadValue<Vector2>();
                Debug.Log("NAVIGATING: " + input.x);
                GameManager.Instance.addMaxRounds(input.x < 0 ? -1 : 1);
                return;
            }
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        Debug.Log("Player is canceling");
        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.RoundSelect)
        {
            //if (context.started) --> done now in seperate script = LobbyLeave.cs
            //{
            //    // If in the lobby, reset the start bar
            //    LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
            //    if (lobbyUIManager != null)
            //    {
            //        lobbyUIManager.FillCancelBar(this);
            //    }
            //}
            if (context.started)
            {
                if(PlayerManager.Instance.players.Count > 1)
                    GameManager.Instance.GoToLobby();
                else
                    GameManager.Instance.GoToMainMenu();
            }
        }
        else if(GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby)
        {
            if(context.canceled)
                PlayerManager.Instance.RemovePlayer(this);
        }
    }

    public void InitializePlayer(int index, string name)
    {
        PlayerIndex = index;
        PlayerName = name;

        // Randomize visual customization
        //_customization?.Randomize();
    }

    public void SetVisible(bool visible)
    {
        foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = visible;
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        m_xAxisInput = context.ReadValue<Vector2>().x;

        float previousYAxisInput = m_yAxisInput;
        m_yAxisInput = context.ReadValue<Vector2>().y;

        if (previousYAxisInput != m_yAxisInput && previousYAxisInput <= -0.8)
        {
            m_animator.SetBool("crouching", false);
        }
        if (m_yAxisInput <= -0.8 && m_isGrounded)
        {
            m_animator.SetBool("crouching", true);
            m_xAxisInput = 0;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        m_jumpInput = context.ReadValue<float>();

        // Jump buffer
        jumpBufferTimeCounter = m_jumpBufferTime;

        // has coyote time or is grounded
        if (m_isGrounded || coyoteTimeCounter > 0f)
        {
            m_rb.gravityScale = 1.0f;

            if (m_jumpInput != 0) 
            {
                CheckJumpRaycasts();
                Jump();
            }

            // Make sure jump buffer is only set when falling
            jumpBufferTimeCounter = 0;
            // Make sure coyote time can only be used once
            coyoteTimeCounter = 0;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.Instance.TogglePauseGame(PlayerIndex, PlayerName);
        }
    }

    public Vector3 GetTopmostVertex()
    {
        //Get top most vertex of each sprite to dynamically place number icon
        Vector2[] localVertices = GetComponent<SpriteRenderer>().sprite.vertices;
        Vector3 topVertex = Vector3.zero;
        float maxY = float.MinValue;

        foreach (Vector2 localVertex in localVertices)
        {
            Vector3 worldVertex = transform.TransformPoint(localVertex);
            if (worldVertex.y > maxY)
            {
                maxY = worldVertex.y;
                topVertex = worldVertex;
            }
        }
        return topVertex;
    }

    #region FixedUpdate

    private void FixedUpdate()
    {
        if (_isDead || _isFinished)
            return;

        if (m_rb.linearVelocity.y < 0f)
        {
            is_falling = true;
        }
        else if (m_rb.linearVelocity.y >= 0f)
        {
            is_falling = false;
        }

        if (ComputeIsStandingOn("Solid") && !m_isGrounded)
            m_animator.SetTrigger("Impact");

        m_isGrounded = ComputeIsStandingOn("Solid");
        m_animator.SetBool("grounded", m_isGrounded);
        m_animator.SetBool("decending", is_falling);

        ComputeVelocity();
        // Place number icon
        Vector3 playerNumIconPos = playerNumIcon.transform.position;
        playerNumIconPos.y = GetTopmostVertex().y;
        playerNumIcon.transform.position = playerNumIconPos;

        MovementCounterTickdowns();
        CheckJumpBuffer();

        // Visualize jump raycasts
        if (visualizeRaycasts)
        {
            foreach (Transform origin in m_jumpRaycastOrigin)
            {
                Debug.DrawRay(origin.position, Vector3.up * m_jumpRaycastLength, Color.green);
            }
        }
    }

    #endregion



    private void ComputeVelocity()
    {
        ComputeXVelocity();
        ComputeYVelocity();
    }

    private void ComputeXVelocity()
    {
        float xVelocity = 0;

        if (m_xAxisInput != 0) // Accelerate
        {
            xVelocity = Mathf.MoveTowards(m_rb.linearVelocity.x, m_maxSpeed * m_xAxisInput, m_acceleration * Time.deltaTime);
        }
        else // Decelerate
        {
            xVelocity = Mathf.MoveTowards(m_rb.linearVelocity.x, 0, m_deceleration * Time.deltaTime);
        }
        m_rb.linearVelocity = new Vector2(xVelocity, m_rb.linearVelocity.y);
    }

    private void ComputeYVelocity()
    {
        // Up / Down acceleration
        if (m_rb.linearVelocity.y > 0 && m_jumpInput == 0 && m_rb.gravityScale <= 1)
        {
            m_rb.gravityScale += m_lowJumpModifier;
        }
        if (m_rb.linearVelocity.y < 0 && m_rb.gravityScale <= 1.0f + m_lowJumpModifier)
        {
            m_rb.gravityScale += m_fallModifier;
        }
    }

    private bool ComputeIsStandingOn(string tag)
    {
        // Make three raycasts for more accuracy
        float epsilon = 0.0625f;
        Vector3 offset = new(m_collider.offset.x, m_collider.offset.y);
        Ray ray = new(transform.position - new Vector3(0, m_collider.size.y / 2.0f) + offset, Vector3.down);
        Debug.DrawRay(ray.origin, Vector2.down, Color.yellow);
        RaycastHit2D[] isMiddleTouching = Physics2D.RaycastAll(ray.origin, ray.direction, epsilon);
        RaycastHit2D[] isLeftTouching = Physics2D.RaycastAll(ray.origin - new Vector3(m_collider.size.x / 2.0f - epsilon, 0), ray.direction, epsilon);
        RaycastHit2D[] isRightTouching = Physics2D.RaycastAll(ray.origin + new Vector3(m_collider.size.x / 2.0f - epsilon, 0), ray.direction, epsilon);

        // Check if raycasts collide with ground
        foreach (RaycastHit2D collision in isMiddleTouching)
        {
            if (collision.collider.tag.Equals(tag)) return true;
        }
        foreach (RaycastHit2D collision in isLeftTouching)
        {
            if (collision.collider.tag.Equals(tag)) return true;
        }
        foreach (RaycastHit2D collision in isRightTouching)
        {
            if (collision.collider.tag.Equals(tag)) return true;
        }

        return false;
    }

    public void Hurt()
    {
        m_animator.SetTrigger("Hurt");
        m_collider.isTrigger = true;
        m_collider.contactCaptureLayers = deathContactLayers; //set layer only to collide with deathTiles
        freezeInputs();
        _isDead = true;
        _hurtEffect.SetActive(true);
        _hurtTrail.SetActive(true);
    }

    public void resetDeath()
    {
        activateInputs();

        if (!m_collider)
            m_collider = GetComponent<BoxCollider2D>();

        _isDead = false;
        m_collider.isTrigger = false;
        m_collider.contactCaptureLayers = LayerMask.NameToLayer("Everything");
        _hurtEffect.SetActive(false);
        _hurtTrail.SetActive(false);
    }

    private void Jump()
    {
        m_rb.linearVelocity = new Vector2(m_rb.linearVelocity.x, 0);
        m_rb.AddForce(new Vector2(0f, m_jumpForce), ForceMode2D.Impulse);
        m_animator.SetTrigger("Jump");
    }

    public void AddScore(int score)
    {
        /*int scoreTest = Random.Range(0,999);//Testing
        this.score += scoreTest;*/

        this.Score += score;
    }

    public void cheer()
    {
        _isFinished = true;
        freezeInputs();
        this.gameObject.layer = LayerMask.NameToLayer(noCollideLayer);
        m_collider.contactCaptureLayers = deathContactLayers; //stop playing contacting other objects
        m_rb.linearVelocity = new Vector2(0, 0); // stop player
        m_animator.SetTrigger("Cheer");
    }

    public void checkAngry()
    {
        if (_isFinished == false)
            angry();
    }

    private void angry()
    {
        _isFinished = true;
        freezeInputs();
        this.gameObject.layer = LayerMask.NameToLayer(noCollideLayer);
        m_animator.SetTrigger("Angry");
        m_rb.linearVelocity = new Vector2(0, 0); // stop player
    }

    private void MovementCounterTickdowns()
    {
        // Movement counter tickdowns
        if (m_isGrounded)
        {
            coyoteTimeCounter = m_coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }
        if (jumpBufferTimeCounter > 0)
        {
            jumpBufferTimeCounter -= Time.fixedDeltaTime;
        }
    }

    private void CheckJumpBuffer()
    {
        // Jump when on the ground and jump was buffered
        if (m_isGrounded && jumpBufferTimeCounter > 0f)
        {
            m_rb.gravityScale = 1.0f;
            Jump();
            jumpBufferTimeCounter = 0;  // Reset jump buffer after jumping
        }
    }

    private void CheckJumpRaycasts()
    {
        // Don't correct jump if there aren't 4 raycasts assigned
        if (m_jumpRaycastOrigin.Length < 4)
        {
            return;
        }

        // Check each jump raycasts to see if all of them hit something
        RaycastHit2D leftHit = Physics2D.Raycast(m_jumpRaycastOrigin[0].position, Vector3.up, m_jumpRaycastLength);
        RaycastHit2D middleLeftHit = Physics2D.Raycast(m_jumpRaycastOrigin[1].position, Vector3.up, m_jumpRaycastLength);
        RaycastHit2D middleRightHit = Physics2D.Raycast(m_jumpRaycastOrigin[2].position, Vector3.up, m_jumpRaycastLength);
        RaycastHit2D rightHit = Physics2D.Raycast(m_jumpRaycastOrigin[3].position, Vector3.up, m_jumpRaycastLength);
        float correctJumpDistance = 0.15f;
        
        // Correct jump
        if (leftHit.collider != null && middleLeftHit.collider == null
            && middleRightHit.collider == null && rightHit.collider == null)
        {
            m_rb.position += new Vector2(correctJumpDistance, 0); //nudge player to avoid getting stuck
        }
        else if (rightHit.collider != null && middleRightHit.collider == null
            && middleLeftHit.collider == null && leftHit.collider == null)
        {
            m_rb.position += new Vector2(-correctJumpDistance, 0); //nudge player to avoid getting stuck
        }
    }
}