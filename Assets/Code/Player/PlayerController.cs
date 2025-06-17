using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _hurtEffect;
    [SerializeField] private GameObject _hurtTrail;
    [SerializeField] private int m_maxSpeed, m_acceleration, m_deceleration, m_jumpForce;
    [SerializeField] private float m_lowJumpModifier, m_fallModifier;
    [SerializeField] private LayerMask deathContactLayers;
    private Animator m_animator;
    private BoxCollider2D m_collider;
    private Rigidbody2D m_rb;
    private float m_jumpInput, m_xAxisInput, m_yAxisInput;

    public bool m_isGrounded;
    public bool is_falling = false;

    public int PlayerIndex { get; private set; }
    public string PlayerName { get; set; }

    public int score { get; private set; }

    [SerializeField] private GameObject playerNumIcon;

    private PlayerInput _playerInput;
    private PlayerCustomization _customization;
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
        _customization = GetComponent<PlayerCustomization>();
    }

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider2D>();
        m_rb = GetComponent<Rigidbody2D>();
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
                    lobbyUIManager.FillStartBar(this);
                }
            }
            else if (context.canceled)
            {
                // If the action is canceled, reset the start bar
                LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
                if (lobbyUIManager != null)
                {
                    lobbyUIManager.CancelFillStartBar();
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
        if (GameManager.Instance.CurrentGameState == GameManager.GameStates.Lobby)
        {
            if (context.started)
            {
                // If in the lobby, reset the start bar
                LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
                if (lobbyUIManager != null)
                {
                    lobbyUIManager.FillCancelBar(this);
                }
            }
            if (context.canceled)
            {
                LobbyUIManager lobbyUIManager = FindAnyObjectByType<LobbyUIManager>();
                if (lobbyUIManager != null)
                {
                    lobbyUIManager.CancelFillCancelBar();
                }
                PlayerManager.Instance.RemovePlayer(this);
            }
        }
    }

    public void InitializePlayer(int index, string name)
    {
        PlayerIndex = index;
        PlayerName = name;

        // Randomize visual customization
        _customization?.Randomize();
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
        if (m_isGrounded)
        {
            m_rb.gravityScale = 1.0f;

            if (m_jumpInput != 0) Jump();
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
    }



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
        Vector3 offset = new Vector3(GetComponent<BoxCollider2D>().offset.x, GetComponent<BoxCollider2D>().offset.y);
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

        this.score += score;
    }

    public void cheer()
    {
        _isFinished = true;
        freezeInputs();
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
        m_animator.SetTrigger("Angry");
        m_rb.linearVelocity = new Vector2(0, 0); // stop player
    }
}