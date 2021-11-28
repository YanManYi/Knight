using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.Events;
using System;//自带的好用

//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3> { }
public class MouseManager : SingLeton<MouseManager>
{
    public Texture2D point, attack, target, doorway, arrow;


    // public EventVector3 OnMouseClicked;
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    private RaycastHit hitInfo;
    public LayerMask layerMask;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }


    private void Update()
    {
        SetCursorTexture();
        MouseController();

    }

    /// <summary>
    /// 设置鼠标贴图
    /// </summary>
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 1000, layerMask))//不检测player层，这样子透过player不造成阻挡
        {
            //切换贴图

            switch (hitInfo.transform.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;

                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;

                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }

    }

    void MouseController()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)//主角死了就不能点了缺个判断
        {

            switch (hitInfo.transform.tag)
            {

                case "Ground":
                    OnMouseClicked?.Invoke(hitInfo.point);
                    break;

                case "Enemy":

                    OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
                    break;

                case "Portal":
                    OnMouseClicked?.Invoke(hitInfo.point);
                    break;

                default:
                    break;
            }


        }

    }

}
