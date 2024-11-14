using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class WeaponManager : MonoBehaviour
{
    public static bool isChangeWeapon = false;
    //현재 무기와 현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기의 타입
    public string currentWeaponType;
    public string currentEquipWeapon;

    // 무기 종류들 관리
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;
    
    // 관리 차원에서 쉽게 무기 접근이 가능하도록 해줌
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();
    // 필요한 컴포넌트
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickAxeController thePickaxeController;
    
    private void Start() {
        for ( int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        } 
        for ( int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        } 
        for ( int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        } 
    }


    private void Update() {
        if(!isChangeWeapon)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1) && currentEquipWeapon != "Hand")
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "Hand"));
                //theHandController.enabled = true;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)&& currentEquipWeapon != "Axe")
            {
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
                //theAxeController.enabled = true;
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)&& currentEquipWeapon != "PickAxe")
            {
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "PickAxe"));
                //thePickaxeController.enabled = true;
            }
        }
    }


    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        SwitchController(_type);
        isChangeWeapon = true;
        CanclePreWeaponAction();
        WeaponChange(_type, _name);
        currentWeaponType = _type;
        isChangeWeapon = false;
        yield return null;
    }

    private void SwitchController(string _type)
    {
        switch (_type)
        {
            case "HAND":
            theHandController.enabled = true;
            currentEquipWeapon = "Hand";
            break;
            case "AXE":
            theAxeController.enabled = true;
            currentEquipWeapon = "Axe";
            break;
            case "PICKAXE":
            thePickaxeController.enabled = true;
            currentEquipWeapon = "PickAxe";
            break;
        }
    }

    // 무기 취소 함수
    private void CanclePreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "HAND":
                HandController.isActivate = false;
                theHandController.enabled = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                theAxeController.enabled = false;
                break;
            case "PICKAXE":
                PickAxeController.isActivate = false;
                thePickaxeController.enabled = false;
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "HAND")
        {
            theHandController.CloseWeaponChange(handDictionary[_name]);
        }
        else if(_type == "AXE")
        {
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        }
        else if(_type == "PICKAXE")
        {
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
        }
    }
}
