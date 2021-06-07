using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.Sungjin.FPSpractice
{


    public class Look : MonoBehaviour
    {
        public static bool cursorLocked = true;
        public Transform orientation;
        public Transform cams;

        [Range(0, 100)] public float xSensitivity;
        [Range(0, 100)] public float ySensitivity;
        public float maxAngle;

        private float YClamp;
        const float sensitivityConstant = 0.025f;

        void Start()
        {
            orientation = transform.GetChild(0);
        }


        void Update()
        {
            SetXY();
            UpdateCursorLock();
        }

        void SetXY()
        {
            // 마우스 입력
            var mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            // 민감도가 적용된 마우스 X,Y 입력
            var X = mouseInput.x * xSensitivity * sensitivityConstant;
            var Y = mouseInput.y * ySensitivity * sensitivityConstant;

            // 플레이어 시점 계산
            var XDir = cams.localRotation.eulerAngles.y + X;

            YClamp -= Y;
            YClamp = Mathf.Clamp(YClamp, -maxAngle, maxAngle); //위 아래 제한

            // 적용
            orientation.localRotation = Quaternion.Euler(0, XDir, 0);
            cams.localRotation = Quaternion.Euler(YClamp, XDir, 0);
        }

        void UpdateCursorLock()
        {
            // 키 체킹
            if (Input.GetKeyDown(KeyCode.Escape)) cursorLocked = !cursorLocked;
            
            // 마우스 상태 변경 
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !cursorLocked;
        }
    }

}
