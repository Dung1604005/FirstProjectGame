using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{

    [SerializeField] private int npcId;

    [SerializeField] private string nameNpc;

    [SerializeField] private bool interacting = false;

    [SerializeField] private GameObject interactingKey;

    [SerializeField] private float interactRadius;

    [SerializeField] private List<int> ItemIndexList;

    void Update()
    {
         if (!interacting)
        {
            if((GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.gameObject.transform.position).sqrMagnitude <= interactRadius * interactRadius)
            {
                interactingKey.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    UIManageMent.Instance.ShopSystem.TurnOn();
                    UIManageMent.Instance.ShopSystem.SetItemShop(ItemIndexList);
                }
            }
            else
            {
                interactingKey.SetActive(false);
            }
            return;
        }
    }


}
