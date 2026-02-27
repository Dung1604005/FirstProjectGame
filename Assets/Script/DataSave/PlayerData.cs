[System.Serializable]
public class PlayerData
{
    public float currentHpPlayer;

    public int levelPlayer;

    public int expToLevelUp ;

    public int pointStat ;

    public int pointMaxHp ;

    public int pointAtk ;

    public int pointCritRate ;

    public int curGold ;

    public string currentMapName ;

    public float posX ;

    public float posY ;

    public float posZ ;

    public int shotgunBullet;
    
    public int cur_ShotgunBullet;
    

    public  int pistolBullet;   
    
    public int cur_PistolBullet;

   public int gunBullet;    
    
    public int cur_GunBullet;

    public PlayerData(float currentHpPlayer, int levelPlayer, int expToLevelUp, int pointStat,
        int pointMaxHp, int pointAtk, int pointCritRate, int curGold,
        string currentMapName, float posX, float posY, float posZ,
        int shotgunBullet, int cur_ShotgunBullet,
        int pistolBullet, int cur_PistolBullet,
        int gunBullet, int cur_GunBullet)
    {
        this.currentHpPlayer = currentHpPlayer;
        this.levelPlayer = levelPlayer;
        this.expToLevelUp = expToLevelUp;
        this.pointStat = pointStat;
        this.pointMaxHp = pointMaxHp;
        this.pointAtk = pointAtk;
        this.pointCritRate = pointCritRate;
        this.curGold = curGold;
        this.currentMapName = currentMapName;
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
