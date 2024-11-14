using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 인벤토리 열려있는지 확인
    public static bool InventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;

    private void Start() {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>(); // Grid Setting의 자식에서 Slot 오브젝트의 Slot스크립트를 slots 배열에 저장
    }

    private void Update() {
        TryOpenInventory();
    }
    // 인벤토리 오픈 시도
    private void TryOpenInventory() 
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryActivated = !InventoryActivated; // InventoryActivated 전환 스위치

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
        if(Item.ItemType.Equipment != _item.itemType) // _item.itemType가 Equipment가 아니면
        {
            
            for(int i = 0; i < slots.Length; i++) // Slots의 개수만큼
            {
                // 이미 아이템이 있으면 개수 증가
                if(slots[i].item != null) // slots i번째의 item이 null이 아니면 (이미 아이템이 있으면 개수 증가)
                {
                    if(slots[i].item.itemName == _item.itemName) // slots의 item 변수에 할당 된 item의 itemName 가 _item.itemName와 같다면
                    {
                        Debug.Log("같은 이름");
                        slots[i].SetSlotCount(_count); // slots i 번째의 SetSlotcount으로 itemCount를 늘려주고 ToString화 시켜줘서 적용
                        return;
                    }
                }
            }
        }

        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null) // 아이템이 없으면 빈자리 찾아서 추가
            {
                Debug.Log("새로운 아이템 추가");
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
