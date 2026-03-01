using Cinemachine;
using UnityEngine;

public class BossVisual : MonoBehaviour
{
    [Header("Reference")]

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer => spriteRenderer;


    [Header("VFX")]

    [SerializeField] private ParticleSystem chargeVFX;

    [SerializeField] private ParticleSystem novaVFX;

    [Header("Camera Shake")]

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    private CinemachineBasicMultiChannelPerlin noisePerlin;

    [SerializeField] private float shakeAmplitude;

    [SerializeField] private float shakeFrequency;

    [SerializeField] private float lerpSpeed;

    private bool isShaking = false;


    public void Init()
    {
        animator  = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        virtualCamera = GameManageMent.Instance.CinemachineVirtualCamera;
        if(GetComponent<CinemachineImpulseSource>() != null)
        {
            cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }
        if(virtualCamera != null)
        {
            noisePerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        if(chargeVFX != null)
        {
            chargeVFX.Stop();
        }
        if(novaVFX != null)
        {
            novaVFX.Stop();
        }
        
        
    }

    void Awake()
    {
        Init();
    }
    void Start()
    {
        BossStat bossStat = GetComponentInParent<BossStat>();
        if(bossStat != null)
        {
            bossStat.OnBossDie += SetAnimDie;
            bossStat.OnBossDie += Die;
        }
    }

    public void SetFlip(float xDirection)
    {
        if(xDirection < 0)
        {
            this.transform.localScale = new Vector3(-1, 1 , 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1 , 1);
        }
    }

    void Update()
    {
        if (isShaking)
        {
            if(noisePerlin != null)
            {
                noisePerlin.m_AmplitudeGain = Mathf.Lerp(noisePerlin.m_AmplitudeGain, shakeAmplitude, lerpSpeed*Time.deltaTime);
            }
        }
        else
        {
            if(noisePerlin != null)
            {
                noisePerlin.m_AmplitudeGain = Mathf.Lerp(noisePerlin.m_AmplitudeGain, 0f, lerpSpeed*Time.deltaTime);
            }
        }
    }

    public void StartShakingNoise()
    {
        isShaking = true;
        noisePerlin.m_FrequencyGain = shakeFrequency;
    }
    public void EndShakingNoise()
    {
        isShaking = false;
    }
    public void ShakingImpulse()
    {
        if(cinemachineImpulseSource!= null)
        {
            cinemachineImpulseSource.GenerateImpulse();
        }
    }

    public void SetMove(bool isMoving)
    {
        animator.SetBool("moving", isMoving);
    }

    public void SetAnimDie()
    {
        animator.SetTrigger("die");
    }
    public void Die()
    {
        DisableAnimator();
        EndShakingNoise();
        PlayChargeEffect(false);
        
    }
    public void SetAnimAttack(bool state)
    {
        animator.SetBool("attack", state);
    }

    public void PlayShootEffect()
    {
        EndShakingNoise();
        novaVFX.Play();
        ShakingImpulse();
    }

    public void PlayChargeEffect(bool isCharge)
    {
        if(chargeVFX == null)
        {
            return;
        }
        if (isCharge)
        {
            StartShakingNoise();
            chargeVFX.Play();
        }
        else
        {
            chargeVFX.Stop();
        }
        

    }

    /// <summary>
    /// Reset boss visual to initial state
    /// </summary>
    public void ResetVisual()
    {
        // Reset shaking
        isShaking = false;
        EndShakingNoise();
        
        // Stop all VFX
        if (chargeVFX != null)
        {
            chargeVFX.Stop();
        }
        if (novaVFX != null)
        {
            novaVFX.Stop();
        }
        
        // Reset animation states
        if (animator != null)
        {
            animator.SetBool("moving", false);
            animator.SetBool("attack", false);
        }
        
        
        this.transform.localScale = new Vector3(1,1,1);
    }
    public void DisableAnimator() { GetComponent<Animator>().enabled = false; }







}
