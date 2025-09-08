using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Attributes:")]
    [SerializeField] public int maxHp;
    [SerializeField] public int currentHp;

    [SerializeField] public int maxMana;
    [SerializeField] public int currentMana;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    public float JumpForce => _jumpForce;
    public bool hasAppliedJumpForce;

    //Double Jump
    public int maxJumpCount = 2;
    public int currentJumpCount;

    [Header("Layer:")]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _groundCheck;
    public bool IsGrounded => Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _playerLayer);
    public bool IsTrap => Physics2D.OverlapCircle(_groundCheck.position, 0.2f, _playerLayer);

    [Header("Logic:")]
    [SerializeField] private Color _flashColor = Color.red;
    private bool _isFlashing = false;

    private float _coyoteTime = 0.1f;
    private float _coyoteTimeCounter;

    [SerializeField] private bool _isDied = false;
    public bool IsDied => _isDied;

    [Header("Animation: :")]
    private IPlayerStates _currentState;

    [Header("State Hurt:")]
    private bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;

    [Header("Fire Bullet: ")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private PoolBullet _bulletPool;

    [SerializeField] private float _shootCooldown = 15f;
    private float _cooldownTimer;

    [Header("References: ")]
    public GameManager _gameManager;
    public AudioManager _audioManager;
    private PlayerCollision _playerCollision;

    [SerializeField] private BarUI[] _barUI;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Image _shootTimerUI;

    public Rigidbody2D playerRb { get; private set; }
    public Animator animator { get; private set; }

    [SerializeField] private Camera _camera;

    [Header("Effects: ")]
    [SerializeField] private GameObject _shieldBreakEffectPrefab;
    [SerializeField] private GameObject _healEffectPrefab;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _gameManager = FindAnyObjectByType<GameManager>();
        _audioManager = FindAnyObjectByType<AudioManager>();
        _playerCollision = GetComponent<PlayerCollision>();

        if (_groundCheck == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.SetParent(transform);
            check.transform.localPosition = Vector3.down * 0.85f;
            _groundCheck = check.transform;
        }
    }
    private void Start()
    {
        currentHp = maxHp;
        currentMana = maxMana;
        _cooldownTimer = _shootCooldown;
        _isDied = false;

        TransitionToState(new IdleState());
    }

    void Update()
    {
        if (_gameManager.IsGameOver() || _gameManager.IsVictoryGame()) return;

        float xInput = Input.GetAxisRaw("Horizontal");
        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);

        if (IsGrounded)
            currentJumpCount = 0;

        if (IsGrounded || IsTrap)
            _coyoteTimeCounter = _coyoteTime;
        else
            _coyoteTimeCounter -= Time.deltaTime;

        _cooldownTimer += Time.deltaTime;
        HandleShooting();

        _currentState?.Update(this, xInput, jumpPressed);

        UpdateAnimation();
    }
    #region UI
    public void TakeDamage(int damage)
    {
        if (_isInvincible) return;

        if (_gameManager.IsShieldActive())
        {
            SpawnShieldBreakEffect();
            _gameManager.DeactivatedShield();
            return;
        }
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        foreach (var HPbar in _barUI)
            HPbar.UpdateHpBarUI();

        _audioManager?.PlayHitSound();

        if (!_isFlashing)
        {
            StartCoroutine(FlashWhite());
            _playerCollision?.SpawnBlood();
        }

        if (currentHp == 0)
        {
            Die();
        }
        else
        {
            TransitionToState(new HurtState());
            StartCoroutine(Invincible(2f));
        }
    }
    private IEnumerator Invincible(float duration)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(duration);
        _isInvincible = false;
    }
    public void Heal(int healAmount)
    {
        currentHp += healAmount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        SpawnHealEffect();
        foreach (var HPbar in _barUI)
            HPbar.UpdateHpBarUI();
    }
    public void RecoveryMana(int manaAmount)
    {
        currentMana += manaAmount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        SpawnHealEffect();
        foreach (var manaBar in _barUI)
            manaBar.UpdateManaBarUI();
    }
    public void UseMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            foreach (var manaBar in _barUI)
                manaBar.UpdateManaBarUI();
        }
    }
    private void UpdateShootTimerUI()
    {
        if (_shootTimerUI != null)
        {
            _shootTimerUI.fillAmount = 0f;

            _shootTimerUI.DOKill();

            _shootTimerUI.DOFillAmount(1f, _shootCooldown)
                .SetEase(Ease.Linear);
        }
    }

    private void Die()
    {
        transform.SetParent(null);
        TransitionToState(new DieState());
        _isDied = true;
    }
    #endregion
    #region Player Controller
    public void PlayerMovement(float xDir)
    {
        playerRb.velocity = new Vector3(xDir * _moveSpeed, playerRb.velocity.y);

        if (xDir > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (xDir < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
    public void HandleShooting()
    {
        if (_cooldownTimer < _shootCooldown) return;
        if (currentMana <= 0) return;
        if (_currentState is StrikeState) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;

            Vector2 dir = (mouseWorldPos - transform.position).normalized;
            transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 1);

            UseMana(1);
            _cooldownTimer = 0f;
            UpdateShootTimerUI();

            string triggerName = "NormalAttack";
            if (_currentState is CrouchState) triggerName = "CrouchAttack";
            else if (_currentState is JumpState) triggerName = "JumpAttack";

            TransitionToState(new AttackState(triggerName, _currentState));
        }
    }
    public void ShootBullet()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        GameObject bulletObj = _bulletPool.GetBullet();
        bulletObj.SetActive(true);
        bulletObj.GetComponent<Bullet>().Shoot(_firePoint.position, mouseWorldPos);

        _audioManager?.PlayFireBallSound();
    }
    #endregion
    #region Animation
    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(playerRb.velocity.x) > 0.1f;
        bool isJumping = !IsGrounded;
    }
    public void TransitionToState(IPlayerStates newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }
    public void OnWinTrigger()
    {
        TransitionToState(new WinState());
    }
    public void SetInvincible(bool value)
    {
        _isInvincible = value;
    }
    public void OnAttackAnimationEnd()
    {
        if (_currentState is AttackState attackState)
        {
            attackState.BackToPreviousState(this);
        }
    }
    #endregion
    #region Audio
    public void PlayJumpSound()
    {
        _audioManager?.PlayJumpSound();
    }
    #endregion

    #region Effect
    private IEnumerator FlashWhite()
    {
        _isFlashing = true;

        _spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            _spriteRenderer.DOColor(Color.white, 0.3f).OnComplete(() =>
            {

            });_isFlashing = false;
        });
        yield return null;
    }
    private void SpawnShieldBreakEffect()
    {
        if (_shieldBreakEffectPrefab != null && _gameManager.IsShieldActive() == true)
        {
            GameObject shieldBreakEffect = Instantiate(_shieldBreakEffectPrefab, transform.position, Quaternion.identity);
            Destroy(shieldBreakEffect, 1f);
        }
    }
    private void SpawnHealEffect()
    {
        if (_healEffectPrefab != null)
        {
            GameObject healEffect = Instantiate(_healEffectPrefab, transform.position, Quaternion.identity);
            Destroy(healEffect, 1f);
        }
    }
    #endregion
}
