using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceCursor : MonoBehaviour {
    [SerializeField] Texture2D cursor = default;
    void Start() {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    
}
