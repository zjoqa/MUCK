using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool InventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;

    private void Start() {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    private void Update() {
        TryOpenInventory();
    }

    private void TryOpenInventory() 
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryActivated = !InventoryActivated;

            if(InventoryActivated)
                OpenInventory();
            else
                CloseInventory();   
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    public void AcquireItem(Item _item , int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)
        {
            for(int i = 0; i < slots.Length; i++) // 이미 아이템이 있으면 개수 증가
            {
                if(slots[i].item != null)
                {
                    if(slots[i].item.itemName == _item.itemName)
                    {
                        Debug.Log("같은 이름");
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null) // 아이템이 없으면 빈자리 찾아서 추가
            {
                Debug.Log("!!");
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
