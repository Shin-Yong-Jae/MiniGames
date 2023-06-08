using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

enum ScrollDirection
{
    None,
    MoveLeftX,
    MoveRightX,
    MoveUpY,
    MoveDownY,
}

public class BoardPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables
    private Vector2 startPosition;
    private Vector2 endPosition;
    private ScrollDirection dragDirection;
    #endregion Variables

    #region UnityMethod
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPosition = eventData.position;
        dragDirection = GetDragDirection(startPosition, endPosition);

        Debug.Log(dragDirection.ToString());

    }
    #endregion UnityMethod

    #region MainMethod
    /// <summary> 드래그 이벤트 방향 결정. </summary>
    private ScrollDirection GetDragDirection(Vector2 prePosition, Vector2 currentPosition)
    {
        float movingValueX = currentPosition.x - prePosition.x;
        float movingValueY = currentPosition.y - prePosition.y;

        //절대값.
        float absX = Mathf.Abs(movingValueX);
        float absY = Mathf.Abs(movingValueY);

        // 움직임 최소치 값 도달 못한 경우.
        if(absX < 20 && absY < 20)
        {
            return ScrollDirection.None;
        }
        // Moving X
        else if(absX > absY)
        {
            if(movingValueX > 0)
            {
                return ScrollDirection.MoveLeftX;
            }
            else
            {
                return ScrollDirection.MoveRightX;
            }
        }
        // Moving Y
        else
        {
            if(movingValueY > 0)
            {
                return ScrollDirection.MoveDownY;
            }
            else
            {
                return ScrollDirection.MoveUpY;
            }
        }
    }
    #endregion MainMethod
}
