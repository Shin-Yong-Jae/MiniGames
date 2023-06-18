using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ScrollDirection
{
    None,
    MoveLeftX,
    MoveRightX,
    MoveUpY,
    MoveDownY,
}

public class Game1BoardPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables
    private Vector2 startPosition;
    private Vector2 endPosition;
    private int score;
    private Game1Card[,] cards;

    [SerializeField] private Game1Card prefabCard;
    [SerializeField] private RectTransform rectBoard;

    private int gridSize = 4;
    #endregion Variables

    #region UnityMethod
    private void Start()
    {
        InitializeBoard();
        GetRandomEmptyCard().ChangeNumber(2);
        GetRandomEmptyCard().ChangeNumber(2);
    }

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
        MoveCards(GetDragDirection(startPosition, endPosition));
    }
    #endregion UnityMethod

    #region MainMethod
    /// <summary> 게임 보드 초기화 </summary>
    private void InitializeBoard()
    {
        cards = new Game1Card[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                cards[i, j] = Instantiate(prefabCard, rectBoard).GetComponent<Game1Card>();
                cards[i, j].ChangeNumber(0);
            }
        }
    }

    /// <summary> 랜덤 카드 추가. </summary>
    private Game1Card GetRandomEmptyCard()
    {
        // 빈 칸 중 하나를 무작위로 선택하여 반환
        List<Game1Card> emptyCells = new List<Game1Card>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (cards[x, y].number == 0)
                {
                    emptyCells.Add(cards[x, y]);
                }
            }
        }

        if (emptyCells.Count > 0)
        {
            return emptyCells[Random.Range(0, emptyCells.Count)];
        }
        else
        {
            return null;
        }
    }

    private void MoveCards(ScrollDirection direction)
    {
        Debug.Log($"이동 방향 = {direction}");
        if (direction == ScrollDirection.None)
            return;

        // 타일을 주어진 방향으로 이동시키고 병합 처리
        bool hasMoved = false;

        if (direction == ScrollDirection.MoveUpY)
        {
            // 위쪽으로 이동
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (cards[x, y].number != 0)
                    {
                        hasMoved = cards[x,y].Move(direction, x, y, cards);
                    }
                }
            }
        }
        else if (direction == ScrollDirection.MoveDownY)
        {
            // 아래쪽으로 이동
            for (int x = gridSize - 1; x >= 0; x--)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (cards[x, y].number != 0)
                    {
                        hasMoved = cards[x, y].Move(direction, x, y, cards);
                    }
                }
            }
        }
        else if (direction == ScrollDirection.MoveLeftX)
        {
            // 왼쪽으로 이동
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (cards[x, y].number != 0)
                    {
                        hasMoved = cards[x, y].Move(direction, x, y, cards);
                    }
                }
            }
        }
        else if (direction == ScrollDirection.MoveRightX)
        {
            // 오른쪽으로 이동
            for (int y = gridSize - 1; y >= 0; y--)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (cards[x, y].number != 0)
                    {
                        hasMoved = cards[x, y].Move(direction, x, y, cards);
                    }
                }
            }
        }

        if (hasMoved)
        {
            // 이동 후에 빈 칸이 있다면 랜덤한 타일 생성
            GetRandomEmptyCard().ChangeNumber(2);
        }

        // 게임 오버 조건 체크
        //if (IsGameOver())
        //{
        //isGameOver = true;
        // 게임 오버 처리
        // 게임 오버 화면 표시 등
        //}
    }

    /// <summary> 드래그 이벤트 방향 결정. </summary>
    private ScrollDirection GetDragDirection(Vector2 prePosition, Vector2 currentPosition)
    {
        float movingValueX = currentPosition.x - prePosition.x;
        float movingValueY = currentPosition.y - prePosition.y;

        //절대값.
        float absX = Mathf.Abs(movingValueX);
        float absY = Mathf.Abs(movingValueY);

        // 움직임 최소치 값 도달 못한 경우.
        if (absX < 20 && absY < 20)
        {
            return ScrollDirection.None;
        }
        // Moving X
        else if (absX > absY)
        {
            if (movingValueX > 0)
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
            if (movingValueY > 0)
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
