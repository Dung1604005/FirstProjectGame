using Cinemachine;
using UnityEngine;

public class BossVisual : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;


    [Header("VFX")]

    [SerializeField] private ParticleSystem chargeVFX;

    [SerializeField] private ParticleSystem novaVFX;

    [Header("Camera Shake")]

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    private CinemachineBasicMultiChannelPerlin noisePerlin;

    [SerializeField] private float shakeAmplitude;

    [SerializeField] private float shakeFrequency;

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
                noisePerlin.m_AmplitudeGain = Mathf.Lerp(noisePerlin.m_AmplitudeGain, shakeAmplitude, 0.1f);
            }
        }
        else
        {
            if(noisePerlin != null)
            {
                noisePerlin.m_AmplitudeGain = Mathf.Lerp(noisePerlin.m_AmplitudeGain, 0f, 0.1f);
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
        noisePerlin.m_FrequencyGain = 0f;
        
    }
    public void ShakingImpulse()
    {
        if(cinemachineImpulseSource!= null)
        {
            cinemachineImpulseSource.GenerateImpulse();
        }
    }

    public void PlayShootEffect()
    {
        EndShakingNoise();
        novaVFX.Play();
        ShakingImpulse();

    }
    







}
