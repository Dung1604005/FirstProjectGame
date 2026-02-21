using System;
using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private Context context;

    [SerializeField] private BossVisual visualRoot;

    [SerializeField] private GridManagement gridManagement;

    [Header("Movement Stats")]

    [SerializeField] private float defaultSpeed;

    [SerializeField] private float currentSpeed;

    [SerializeField] private float maxSpeed;

    [SerializeField] private float acceleration;

    [SerializeField] private float wFlow;

    [SerializeField] protected float wAvoid;

    [SerializeField] protected float avoidDistance;

    [SerializeField] protected float separationStrength;

    [SerializeField] protected float wSeparation;

    [SerializeField] protected float separationRadius;

    [SerializeField] private float rangeStop;

    [Header("Unstuck Settings")]
    [SerializeField] private float stuckThreshold = 0.5f; // If velocity below this, consider stuck
    [SerializeField] private float stuckTime = 1f; // How long to wait before unstuck action
    [SerializeField] private float unstuckForce = 5f; // Force to apply when escaping
    [SerializeField] private float directionSmoothSpeed = 0.1f; // Smoothing for direction changes (lower = smoother)
    [SerializeField] private float unstuckCooldown = 2f; // Cooldown after unstuck before detecting stuck again
    [SerializeField] private float flipThreshold = 0.2f; // Minimum x value to change facing direction

    private float stuckTimer = 0f;
    private Vector2 lastPosition;
    private bool isStuck = false;
    private Vector2 currentDirection = Vector2.right;
    private Vector2 stuckDirection = Vector2.zero; // Direction to maintain when stuck
    private float unstuckTimer = 0f;
    private float lastFacingX = 1f; // Track last facing direction

    private String[] masks = { GameConfig.OBJECT_MASK, GameConfig.BUILDING_MASK, GameConfig.PLAYER_WALL_MASK };
    private LayerMask layerMask;

    private BoxCollider2D boxCollider2D;


    [Header("State")]


    [SerializeField] private bool isDashing;

    [SerializeField] private float dashDuration;

    [SerializeField] private bool canMove;

    private Transform target;

    public Transform Target => target;

    [Header("Ghost Stat")]

    [SerializeField] private float ghostTimer;

    [SerializeField] private float ghostSpawnInterval;

    [SerializeField] private Color ghostSpriteColor = new Color(0.2f, 1f, 0.8f, 0.6f);



    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();

        context = new Context();

        defaultSpeed = GetComponent<BossStat>().BossData.RunSpeed;

        currentSpeed = defaultSpeed;
        if (GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
        {
            target = GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0).transform;
        }

        // Auto-find GridManagement if not assigned
        if (gridManagement == null)
        {
            gridManagement = FindFirstObjectByType<GridManagement>();
        }

        canMove = true;
        boxCollider2D = GetComponent<BoxCollider2D>();
        layerMask = LayerMask.GetMask(masks);
        lastPosition = rb.position;
        currentDirection = Vector2.right;
    }
    Vector2 GetDirection()
    {
        if ((target.position - this.transform.position).sqrMagnitude < rangeStop)
        {
            return Vector2.zero;
        }


        Vector2 flow = (target.position - this.transform.position).normalized;
        bool usingFlowField = false;
        if (gridManagement != null)
        {
            Vector2 gridPosition = gridManagement.GridBuilder.WorldToGridPosition(transform.position);
            if (gridManagement.GridBuilder.IsValidGridPosition(gridPosition))
            {
                GridCell cell = gridManagement.GridBuilder.GridCells[(int)gridPosition.x][(int)gridPosition.y];
                float distance = cell.DistanceFromPlayer;
                Vector2 fieldFlow = cell.FlowDirection;
                if (distance < float.MaxValue / 10 && distance > 0 && fieldFlow != Vector2.zero)
                {
                    flow = fieldFlow;
                    usingFlowField = true;
                    Debug.DrawRay(transform.position, flow * 2f, Color.blue);
                }
                else
                {
                    Debug.Log($"Boss not using FlowField - distance: {distance}, fieldFlow: {fieldFlow}");
                }
            }
            else
            {
                Debug.Log($"Boss gridPosition invalid: {gridPosition}");
            }
        }
        else
        {
            Debug.LogWarning("Boss: gridManagement is null!");
        }
        if (!usingFlowField)
        {
            Debug.DrawRay(transform.position, flow * 2f, Color.magenta); // Direct line fallback
        }
        Vector2 dir = flow.normalized;
        Vector2 pos = rb.position;
        for (int i = 0; i < context.Interest.Length; i++)
        {
            context.SetInterestElement(i, Mathf.Max(0, wFlow * Vector2.Dot(context.Dirs[i].normalized, flow)));

            context.SetDangerElement(i, 0);
        }
        RaycastHit2D hit;

        for (int i = 0; i < context.Directions; i++)
        {
            hit = Physics2D.BoxCast(pos + context.Dirs[i], new Vector2(boxCollider2D.size.x + 0.1f, boxCollider2D.size.y + 0.1f), 0, context.Dirs[i].normalized, avoidDistance, layerMask);
            Color c = Color.red;
            if (hit.collider != null)
            {
                float t = 1f - Mathf.Clamp01(hit.distance / avoidDistance);
                context.SetDangerElement(i, Mathf.Max(context.Danger[i], wAvoid * t));
            }
            else
            {
                c = Color.green;
            }
            Debug.DrawRay(pos + context.Dirs[i], context.Dirs[i] * avoidDistance, c);
        }
        float bestScore = float.NegativeInfinity;
        Vector2 bestDir = flow;
        for (int i = 0; i < context.Directions; i++)
        {
            float score = Mathf.Clamp01(context.Interest[i] - context.Danger[i]);
            if (score > bestScore)
            {
                bestScore = score;
                bestDir = context.Dirs[i];
            }
        }

        // If stuck, ignore avoidance and follow flow directly
        if (isStuck)
        {
            // Lock direction when stuck to prevent oscillation
            if (stuckDirection == Vector2.zero)
            {
                stuckDirection = flow;
            }
            bestDir = stuckDirection;
            bestScore = 1f;
            Debug.DrawRay(pos, bestDir * avoidDistance, Color.red); // Stuck escape direction
        }
        // If all directions blocked (low score), prefer FlowField
        else if (bestScore < 0.1f && usingFlowField)
        {
            bestDir = flow;
            Debug.DrawRay(pos, bestDir * avoidDistance, Color.magenta); // Emergency flow
        }
        else
        {
            stuckDirection = Vector2.zero; // Reset stuck direction when moving normally
        }
        // Tach dan
        LayerMask enemyMask = LayerMask.GetMask(GameConfig.BODY_ENEMY_MASK);

        Vector2 separationForce = Vector2.zero;

        Collider2D[] collider = Physics2D.OverlapCircleAll(pos, separationRadius, enemyMask);
        int enemyCount = 0;
        foreach (Collider2D enemy in collider)
        {
            if (enemy != boxCollider2D)
            {
                enemyCount++;

                float distanceToZombie = Vector2.Distance(enemy.transform.position, transform.position);
                separationForce += (1f - Mathf.Clamp01(distanceToZombie / separationRadius)) * separationStrength * ((Vector2)(transform.position - enemy.transform.position)).normalized;

            }
        }
        if (enemyCount > 0)
        {
            separationForce /= Mathf.Sqrt(enemyCount);
        }
        bestDir = bestDir + separationForce.normalized * wSeparation;
        bestDir = bestDir.normalized;

        // Smooth direction changes to prevent oscillation
        // When stuck or near stuck, smooth even more
        float smoothFactor = directionSmoothSpeed;
        if (stuckTimer > stuckTime * 0.3f) // If approaching stuck state
        {
            smoothFactor *= 0.5f; // Extra smooth to stabilize
        }
        if (isStuck)
        {
            smoothFactor = 1f; // Immediate when stuck to commit to escape direction
        }

        currentDirection = Vector2.Lerp(currentDirection, bestDir, smoothFactor);
        dir = currentDirection.normalized;

        Debug.DrawRay(pos, dir * avoidDistance, Color.yellow); // hướng chọn cuối




        return dir;
    }

    private void ApplyForce(Vector2 dir)
    {

        rb.AddForce(dir * currentSpeed * acceleration);
    }

    void SpawnGhostSprite()
    {
        GhostSprite ghostSprite = GameManageMent.Instance.PoolManager.GhostSpritePools.Spawn(this.transform.position);
        ghostSprite.SetInfo(visualRoot.SpriteRenderer.sprite, visualRoot.SpriteRenderer.flipX, ghostSpriteColor);
    }

    // Gọi hàm này khi boss quyết định lướt
    public void StartDash(Vector2 dashDirection)
    {
        StartCoroutine(DashRoutine(dashDirection));
    }

    private IEnumerator DashRoutine(Vector2 direction)
    {
        
        isDashing = true;

        // 1. Dọn dẹp quán tính cũ để lướt không bị trượt
        rb.linearVelocity = Vector2.zero;

        // 2. Setup thông số
        Vector2 startPos = rb.position;

        Vector2 targetPos = rb.position + direction; 
        float time = 0f;
        ghostTimer = 0f;

        // 3. Quá trình lướt
        while (time < dashDuration)
        {
            ghostTimer  += Time.fixedDeltaTime;
            time += Time.fixedDeltaTime;
            float t = time / dashDuration;

            // Công thức Ease-Out: Nhanh ở đầu, chậm về cuối
            float easeT = t * (2f - t);
            if(ghostTimer >= ghostSpawnInterval)
            {
                SpawnGhostSprite();
                ghostTimer = 0f;
            }

            // Lerp và MovePosition
            Vector2 nextPosition = Vector2.Lerp(startPos, targetPos, easeT);
            rb.MovePosition(nextPosition);

            // Chờ đến frame vật lý tiếp theo
            yield return new WaitForFixedUpdate();
        }

        // 4. Kết thúc lướt
        isDashing = false;
    }




    private void FaceTarget(Vector2 dir)
    {
        if (dir == Vector2.zero)
        {
            dir = (target.position - this.transform.position).normalized;
        }

        // Don't change facing when stuck to prevent oscillation
        if (!isStuck && stuckTimer < stuckTime * 0.5f) // Only update when not near stuck state
        {
            // Only change facing if direction is strong enough (prevent oscillation)
            if (Mathf.Abs(dir.x) > flipThreshold)
            {
                lastFacingX = dir.x;
            }
        }

        visualRoot.SetFlip(lastFacingX);

    }
    public void StopMoving()
    {
        canMove = false;


    }
    public void ResumeMoving()
    {
        canMove = true;
    }

    public void SetStationary(bool isStationary)
    {
        if (isStationary)
        {
            currentSpeed = 0f;
        }
        else
        {
            currentSpeed = defaultSpeed;
        }
    }
    void Awake()
    {
        Init();
    }
    void Start()
    {
        GetComponent<BossStat>().OnBossDie += StopMoving;
    }
    void FixedUpdate()
    {

        if (isDashing)
        {
            return;
        }
       
        if (!canMove || target == null)
        {
            return;
        }

        // Update unstuck cooldown
        if (unstuckTimer > 0)
        {
            unstuckTimer -= Time.fixedDeltaTime;
        }

        // Check if stuck (only if cooldown expired)
        if (unstuckTimer <= 0)
        {
            float distanceMoved = Vector2.Distance(rb.position, lastPosition);
            if (rb.linearVelocity.magnitude < stuckThreshold && distanceMoved < 0.1f)
            {
                stuckTimer += Time.fixedDeltaTime;
                if (stuckTimer >= stuckTime)
                {
                    isStuck = true;
                }
            }
            else
            {
                stuckTimer = 0f;
                isStuck = false;
            }
        }
        lastPosition = rb.position;

        Vector2 direction = GetDirection();

        // Apply stronger force when stuck
        if (isStuck)
        {
            rb.linearVelocity = Vector2.zero; // Reset velocity
            rb.AddForce(direction * currentSpeed * acceleration * unstuckForce, ForceMode2D.Impulse);
            isStuck = false; // Reset after applying escape force
            stuckTimer = 0f;
            stuckDirection = Vector2.zero;
            unstuckTimer = unstuckCooldown; // Start cooldown
            Debug.Log("Boss unstuck applied!");
        }
        else
        {
            ApplyForce(direction);
        }
        if (canMove == false || currentSpeed == 0f || direction == Vector2.zero)
        {
            visualRoot.SetMove(false);
        }
        else
        {
            visualRoot.SetMove(true);
        }
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
        FaceTarget(direction);

        



    }
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartDash((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.transform.position));

        }
    }


}
