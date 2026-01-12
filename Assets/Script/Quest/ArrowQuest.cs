
using UnityEngine;

public class ArrowQuest : MonoBehaviour
{
    [SerializeField] private Vector3 goalPosition;

    [SerializeField] private float goalRotate;

    [SerializeField]  private float arrowLength;

    public void SetGoalPosition(Vector3 _goalPosition)
    {
        goalPosition = _goalPosition;
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    private bool IsInsideCamera()
    {
        
        float heightCamera = GameManageMent.Instance.HeightCamera - arrowLength;
        float widthCamera = GameManageMent.Instance.WidthCamera - arrowLength;

        Vector2 playerPos = GameManageMent.Instance.PlayerManager.PlayerController.getPos();

        if ( Mathf.Abs(goalPosition.x - playerPos.x) >  widthCamera/2)
        {
            return false;
        }
        if(Mathf.Abs(goalPosition.y - playerPos.y) > heightCamera / 2)
        {
            return false;
        }
        Debug.Log("Arrow inside");
        return true;
    }
    private void CalculateRotateAndPos()
    {
        Vector2 dir = (goalPosition - gameObject.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f ;
        goalRotate = angle;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        float heightCamera = GameManageMent.Instance.HeightCamera - arrowLength;
        gameObject.transform.position = Vector3.Lerp(transform.position,GameManageMent.Instance.PlayerManager.PlayerController.getPos() + dir*(heightCamera/2), 0.1f);

    }
    
    void Update()
    {
    
        if (IsInsideCamera())
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, goalPosition, 0.1f);
        }
        else
        {
            CalculateRotateAndPos();
        }
        
    }

}
