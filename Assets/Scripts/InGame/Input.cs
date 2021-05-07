using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour, MyInput.IMyControllerActions
{
    MyInput myInput;
    [SerializeField] LayerMask layer;

    void Awake()
    {
        myInput = new MyInput();
        myInput.MyController.SetCallbacks(this);
    }

    void OnEnable()
    {
        myInput.MyController.Enable();
    }

    void OnDisable()
    {
        myInput.MyController.Disable();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.canceled || context.performed)
            return;

        //if (Time.timeScale == 0)
        //    return;

        if (GameManager.Instance.IsWaiting)
           return;

        
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit rayHit;
        if(Physics.Raycast(ray.origin, ray.direction, out rayHit, Mathf.Infinity, layer))
        {
            Jelly jelly = rayHit.collider.GetComponent<Jelly>();

            if (jelly.CurPos.y >= GameManager.Instance.HalfLengthY)
                return;

            if (jelly.type == Type.Bomb)
            {
                GameManager.Instance.DoBomb(jelly);
                return;
            }
            else if(jelly.type == Type.LineBomb)
            {
                switch(Random.Range(0, 2))
                {
                    case 0:
                        GameManager.Instance.DoHorizontalLinePang(jelly);
                        return;
                    default:
                        GameManager.Instance.DoVerticalLinePang(jelly);
                        return;
                }
            }

           
            GameManager.Instance.DoPang(rayHit.collider.GetComponent<Jelly>());

            if (!GameManager.Instance.IsPangSuccess)
            {
                //Challenge에는 상관은 없으나 그냥 함
                --GameManager.Instance.MissCount;
                
                //Challenge인 경우에만
                if(GameManager.Instance.CurStage == 0)
                    GameManager.Instance.UpdateLife(2f);

                //없는 젤리 클릭시 콤보도 초기화
                GameManager.Instance.Combo = 0;
                //Miss글자도 띄움
                var missWord = GameManager.Instance.Pool.GetMissWord();
                missWord.SetActive(true);
                missWord.transform.position = jelly.transform.position;
            }
        }
    }
}
