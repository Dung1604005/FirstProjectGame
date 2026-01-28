using UnityEngine;

public class GhostKingManager : MonoBehaviour
{
    public void InitOutSide()
    {
        
        GameManageMent.Instance.PoolManager.InitSkillGhostKing();
    }

    void Start()
    {
        InitOutSide();
    }
}
