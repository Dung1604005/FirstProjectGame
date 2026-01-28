using System.Linq.Expressions;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    private Context context;

    [SerializeField] private BossVisual visualRoot;

    [Header("Movement Stats")]

    [SerializeField] private float defaultSpeed ;

    [SerializeField] private float currentSpeed;

    [SerializeField] private float maxSpeed ;

    [SerializeField]  private float acceleration;

    [SerializeField] private float wFlow;

    [SerializeField] private float rangeStop;


    [Header("State")]

    private bool canMove;

    private Transform target;

    public Transform Target => target;

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();

        context = new Context();

        defaultSpeed = GetComponent<BossStat>().BossData.RunSpeed;

        currentSpeed = defaultSpeed;
        if(GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) != null)
        {
            target = GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0).transform;
        }

       

        canMove = true;
    }
    Vector2 GetDirection()
    {
        if((target.position - this.transform.position).sqrMagnitude < rangeStop){
            return Vector2.zero;
        }

        Vector2 flow = (target.position - this.transform.position).normalized;
        Vector2 dir = flow.normalized;
        Vector2 pos = rb.position;
        for (int i = 0; i < context.Interest.Length; i++)
        {
            context.SetInterestElement(i, Mathf.Max(0, wFlow * Vector2.Dot(context.Dirs[i].normalized, flow)));
        }
        return context.GetDirection();
    }

    private void ApplyForce(Vector2 dir)
    {
      
        rb.AddForce(dir*currentSpeed*acceleration);
    }

    private void FaceTarget()
    {
        Vector2 dir = (target.position - this.transform.position).normalized;
        visualRoot.SetFlip(dir.x);
        
    }
    public void StopMoving(bool immediate)
    {
        canMove = false;

        if (immediate)
        {
            rb.linearVelocity = Vector2.zero;
        }
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
    void FixedUpdate()
    {
        if(!canMove || target == null)
        {
            return;
        }

        Vector2 direction = GetDirection();
        ApplyForce(direction);
        if(canMove == false || currentSpeed  == 0f || direction == Vector2.zero)
        {
            visualRoot.SetMove(false);
        }
        else
        {
            visualRoot.SetMove(true);
        }
        if(rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }
        FaceTarget();



    }

    
}
