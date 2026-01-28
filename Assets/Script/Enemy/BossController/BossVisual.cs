using Cinemachine;
using UnityEngine;

public class BossVisual : MonoBehaviour
{
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
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        if(virtualCamera != null)
        {
            noisePerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        chargeVFX.Stop();
        novaVFX.Stop();
        
    }

    void Awake()
    {
        Init();
    }

    public void SetFlip(float xDirection)
    {
        if(xDirection < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
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

    public void SetDie()
    {
        animator.SetTrigger("die");
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
    







}
