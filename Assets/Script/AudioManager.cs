using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    // Đang set chay sound cho bgm, nếu có boss không nằm trong forest thì phải code lại, sound gắn vào scene Data

    [Header("=== Background Music ===")]
    [SerializeField] private AudioClip bgmMainMenu;
    [SerializeField] private AudioClip bgmForest;

    [SerializeField] private AudioClip bgmIndoor;

    [SerializeField] private AudioClip bossCombat;

    [Header("=== Player SFX ===")]
    [SerializeField] private AudioClip sfxFootstep;
    [SerializeField] private AudioClip sfxPunch;
    [SerializeField] private AudioClip sfxWoodenMace;
    [SerializeField] private AudioClip sfxPistolShot;
    [SerializeField] private AudioClip sfxRifleShot;
    [SerializeField] private AudioClip sfxShotgunShot;
    [SerializeField] private AudioClip sfxEnergyGunShot;
    [SerializeField] private AudioClip sfxPlayerHit;

    [SerializeField] private AudioClip sfxPlayerHeal;

    [Header("=== Enemy SFX ===")]
    [SerializeField] private AudioClip sfxZombieHit;
    [SerializeField] private AudioClip sfxBossHit;
    [SerializeField] private AudioClip sfxBossCharge;

    [Header("=== UI SFX ===")]
    [SerializeField] private AudioClip sfxUIClick;

    [SerializeField] private AudioClip sfxITemEquip;

    [Header("=== Environment SFX ===")]
    [SerializeField] private AudioClip sfxRain;

    [Header("=== Event SFX ===")]
    [SerializeField] private AudioClip sfxLevelUp;
    [SerializeField] private AudioClip sfxPickUpMoney;
    [SerializeField] private AudioClip sfxDie;

    [SerializeField] private AudioClip missonComplete;

    [Header("=== Audio Sources ===")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambientSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        PlayBGMMainMenu();
    }

    // ==================== BACKGROUND MUSIC ====================

    public void PlayBGMMainMenu()
    {
        PlayBGM(bgmMainMenu);
    }

    public void PlayBGMForest()
    {
        PlayBGM(bgmForest);
    }

    public void PlayBGMBossCombat()
    {
        PlayBGM(bossCombat);
    }

    public void PlayBGMInDoor()
    {
        PlayBGM(bgmIndoor);
    }

    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }

    // ==================== PLAYER SFX ====================

    public void PlayFootstep()
    {
        PlaySFX(sfxFootstep);
    }

    public void PlayPunch()
    {
        PlaySFX(sfxPunch);
    }

    public void PlayWoodenMace()
    {
        PlaySFX(sfxWoodenMace);
    }

    public void PlayPistolShot()
    {
        PlaySFX(sfxPistolShot);
    }

    public void PlayRifleShot()
    {
        PlaySFX(sfxRifleShot);
    }

    public void PlayShotgunShot()
    {
        PlaySFX(sfxShotgunShot);
    }

    public void PlayEnergyGunShot()
    {
        PlaySFX(sfxEnergyGunShot);
    }

    public void PlayPlayerHit()
    {
        PlaySFX(sfxPlayerHit);
    }

    public void PlayPlayerHeal()
    {
        PlaySFX(sfxPlayerHeal);
    }

    // ==================== ENEMY SFX ====================

    public void PlayZombieHit()
    {
        PlaySFX(sfxZombieHit);
    }

    public void PlayBossHit()
    {
        PlaySFX(sfxBossHit);
    }

    public void PlayBossCharge()
    {
        PlaySFX(sfxBossCharge);
    }

    // ==================== UI SFX ====================

    public void PlayUIClick()
    {
        PlaySFX(sfxUIClick);
    }

    public void PlayItemEquip()
    {
        PlaySFX(sfxITemEquip);
    }

    // ==================== ENVIRONMENT SFX ====================

    public void PlayRain()
    {
        PlayAmbient(sfxRain);
    }

    public void StopRain()
    {
        if (ambientSource != null)
        {
            ambientSource.Stop();
        }
    }

    // ==================== EVENT SFX ====================

    public void PlayLevelUp()
    {
        PlaySFX(sfxLevelUp);
    }

    public void PlayPickUpMoney()
    {
        PlaySFX(sfxPickUpMoney);
    }

    public void PlayMissonComplete()
    {
        PlaySFX(missonComplete);
    }

    public void PlayDie()
    {
        PlaySFX(sfxDie);
    }

    // ==================== INTERNAL HELPERS ====================

    private void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    private void PlayAmbient(AudioClip clip)
    {
        if (clip == null || ambientSource == null) return;
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }
}
