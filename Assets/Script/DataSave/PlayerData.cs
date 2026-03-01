[System.Serializable]
public class PlayerData
{
    public float currentHpPlayer = 100;

    public int levelPlayer = 1;

    public int currentExp = 0 ;

    public int pointStat = 0 ;

    public int pointMaxHp = 0 ;

    public int pointAtk = 0 ;

    public int pointCritRate = 0;

    public int curGold = 0 ;

    public float posX = -116.5f  ;

    public float posY = -214.8f;

    public float posZ = 0f;

    public int shotgunBullet  = 0;
    
    public int cur_ShotgunBullet = 0;
    

    public  int pistolBullet = 0;   
    
    public int cur_PistolBullet = 0;

   public int gunBullet = 0;    
    
    public int cur_GunBullet = 0;

    public PlayerData() { }

    public PlayerData(float currentHpPlayer, int levelPlayer, int _currentExp, int pointStat,
        int pointMaxHp, int pointAtk, int pointCritRate, int curGold,
        float posX, float posY, float posZ,
        int shotgunBullet, int cur_ShotgunBullet,
        int pistolBullet, int cur_PistolBullet,
        int gunBullet, int cur_GunBullet)
    {
        this.currentHpPlayer = currentHpPlayer;
        this.levelPlayer = levelPlayer;
        this.currentExp = _currentExp;
        this.pointStat = pointStat;
        this.pointMaxHp = pointMaxHp;
        this.pointAtk = pointAtk;
        this.pointCritRate = pointCritRate;
        this.curGold = curGold;
        this.posX = posX;
        this.posY = posY;
        this.posZ = posZ;
        this.shotgunBullet = shotgunBullet;
        this.cur_ShotgunBullet = cur_ShotgunBullet;
        this.pistolBullet = pistolBullet;
        this.cur_PistolBullet = cur_PistolBullet;
        this.gunBullet = gunBullet;
        this.cur_GunBullet = cur_GunBullet;
    }

}
