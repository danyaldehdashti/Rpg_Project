using UnityEngine;

namespace Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(PlayerController collingController);
    }
}
