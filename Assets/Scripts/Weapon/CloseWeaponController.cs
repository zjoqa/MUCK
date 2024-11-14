using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 미완성 클래스 (추상 클래스)
public abstract class CloseWeaponController : MonoBehaviour
{ 
    // 현재 장착된 Hand형 타입 무기 
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    // 공격중인지 체크 하는 상태 변수
    protected bool isAttack = false;
    protected bool isSwing = false;
    
    protected RaycastHit hitInfo;

    protected void Start() {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    protected void TryAttack()
    {
        if(Input.GetButton("Fire1") && Inventory.InventoryActivated == false)
        {
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        WeaponManager.currentWeaponAnim.SetTrigger("Attack");
        
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;
        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;
    }
    // 미완성 (추상 코루틴)
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject() // currentHand.range 의 거리만큼 충돌한 물체가 있으면 true
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo,currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }


    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }

}
