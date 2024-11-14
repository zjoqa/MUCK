using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject itemPrefab; // 드롭할 아이템의 프리팹
    [SerializeField]
    private float dropDistance = 2f; // 드롭할 위치 (플레이어 주변)

    // 아이템을 드롭하는 메소드
    public void DropItem(Item _item)
    {
        itemPrefab = _item.itemPrefab;
        if(_item != null)
        {
            // 드롭할 아이템 포지션 설정
            Vector3 dropPosition = transform.position + transform.forward * dropDistance;
            Instantiate(itemPrefab, dropPosition, Quaternion.identity);
        }
    }
}
