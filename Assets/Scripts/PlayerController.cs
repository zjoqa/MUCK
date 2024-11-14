using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;
    [SerializeField]
    private float jumpForce;
    // 상태 변수
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = false;

    // 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    [SerializeField]
    private float originPosY;
    private float applyCrouchPosY;
    private CapsuleCollider capsuleCollider; // 땅 착지 여부에 활용될 콜라이더
    // 감도
    [SerializeField]
    private float lookSensitivity;
    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;
    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera; 
    private Rigidbody myRigid;
    private StatusController theStatusController;

    private void Start() {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theStatusController = FindObjectOfType<StatusController>();
        applySpeed = walkSpeed;
        applyCrouchPosY = originPosY;
    }
    private void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
    }


    private void FixedUpdate() {
        Move();
    }
    private void LateUpdate() {
        if(!Inventory.InventoryActivated)
        {
            CameraRotation();
            CharacterRotation();
        }
    }

    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl)) {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCroutine());
    }

    IEnumerator CrouchCroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count ++;
            _posY = Mathf.Lerp(_posY , applyCrouchPosY , 0.3f);
            theCamera.transform.localPosition = new Vector3(0 , _posY , 0);
            if(count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);
    }

    private void IsGround()
    { 
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.3f);
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSp() > 0)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if(isCrouch)
            Crouch();
            theStatusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;
    }
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSp() > 0)
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSp() <= 0)
        {
            RunningCancle();
        }
    }

    private void Running()
    {
        if(isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
        theStatusController.DecreaseStamina(1);
        WeaponManager.currentWeaponAnim.SetBool("Run" , true);
    }

    private void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
        WeaponManager.currentWeaponAnim.SetBool("Run" , false);
    }

    private void Move()
    {
        float _moveDirx = Input.GetAxis("Horizontal");
        float _moveDirz = Input.GetAxis("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirx; // (1,0,0) * -1 || 1
        Vector3 _moveVertical = transform.forward * _moveDirz;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed; // 둘의 합이 1이 나오도록 정규화
        MoveAnimation(_velocity);

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void MoveAnimation(Vector3 _velocity)
    {
        if (applySpeed == walkSpeed)
        {
            WeaponManager.currentWeaponAnim.SetBool("Walk" , true);
        }
        if (_velocity == Vector3.zero)
        {
            WeaponManager.currentWeaponAnim.SetBool("Walk" , false);
        }
        if (isCrouch == true)
        {
            WeaponManager.currentWeaponAnim.SetBool("Walk" , false);
        }
    }


    private void CameraRotation() // 상/하 카메라 회전
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
    }

    private void CharacterRotation() // 좌 / 우 캐릭터 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
