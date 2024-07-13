using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField] Texture2D mouseDefault;
    [SerializeField] Texture2D mouseClick;

    [SerializeField] Vector2 cursorAim = new Vector2(0.5f, 0.5f);
    Vector2 fixedCursorAim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        fixedCursorAim = new Vector2(mouseDefault.width * cursorAim.x, mouseDefault.height * cursorAim.y);
        Cursor.SetCursor(mouseDefault, fixedCursorAim, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Cursor.SetCursor(mouseClick, fixedCursorAim, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(mouseDefault, fixedCursorAim, CursorMode.Auto);
        }
    }
}
