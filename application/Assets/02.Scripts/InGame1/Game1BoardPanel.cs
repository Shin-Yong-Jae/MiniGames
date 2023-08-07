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
    [SerializeField] private Game1Card[,] cards;

    [SerializeField] private Game1Card prefabCard;
    [SerializeField] private RectTransform rectBoard;
    [SerializeField] private Game1TopPanel topPanel;
    
    private int gridSize = 4;
    #endregion Variables

    #region UnityMethod
    private void Start()
    {
        InitializeBoard();
        AddRandomCard(2);
        AddRandomCard(2);
    }
    #endregion UnityMethod
    
    #region EventSystem
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
    #endregion

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
    private void AddRandomCard(int value)
    {
        // 빈 칸 중 하나를 무작위로 선택하여 해당 타일에 숫자 추가
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (cards[x, y].cardNumber == 0)
                {
                    emptyCells.Add(new Vector2Int(x, y));
                }
            }
        }

        if (emptyCells.Count > 0)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            Vector2Int randomCell = emptyCells[randomIndex];
            cards[randomCell.x, randomCell.y].ChangeNumber(value);
        }
        else
        {
            // Game Over.
            topPanel.GameOver();
            // isGameOver = true;
            //     게임 오버 처리
            //     게임 오버 화면 표시 등
        }
    }

    /// <summary> 카드 전체 이동 Method. </summary>
    private void MoveCards(ScrollDirection direction) 
    {
        if (direction == ScrollDirection.None)
            return;

        bool hasMoved = false; // 이동 여부를 나타내는 변수
        bool isMoved = false; // 한 번이라도 이동했는지 여부.
     
        // 타일을 주어진 방향으로 이동시키고 병합 처리
        do
        {
            hasMoved = false; // 이동 여부 초기화

            switch (direction) 
            {
                case ScrollDirection.MoveUpY:
                     for (int y = 0; y < gridSize; y++) 
                     {
                         for (int x = 1; x < gridSize; x++) // 수정: x값 범위를 변경
                         {
                             if (cards[x, y].cardNumber != 0)
                             { 
                                 hasMoved |= cards[x, y].Move(direction, x, y, cards);
                                 isMoved = (isMoved == false) ? hasMoved : true;
                             }
                         }
                     }
                     break;
             case ScrollDirection.MoveDownY:
                 for (int y = 0; y < gridSize; y++)
                 {
                     for (int x = gridSize - 2; x >= 0; x--) // 수정: x값 범위를 변경
                     {
                         if (cards[x, y].cardNumber != 0)
                         {
                             hasMoved |= cards[x, y].Move(direction, x, y, cards);
                             isMoved = (isMoved == false) ? hasMoved : true;
                         }
                     }
                 }
                 break;
             case ScrollDirection.MoveLeftX:
                 for (int x = 0; x < gridSize; x++)
                 {
                     for (int y = 1; y < gridSize; y++) // 수정: y값 범위를 변경
                     {
                         if (cards[x, y].cardNumber != 0)
                         {
                             hasMoved |= cards[x, y].Move(direction, x, y, cards);
                             isMoved = (isMoved == false) ? hasMoved : true;
                         }
                     }
                 }
                 break;
             case ScrollDirection.MoveRightX:
                 for (int x = 0; x < gridSize; x++)
                 {
                     for (int y = gridSize - 2; y >= 0; y--) // 수정: y값 범위를 변경
                     {
                         if (cards[x, y].cardNumber != 0)
                         {
                             hasMoved |= cards[x, y].Move(direction, x, y, cards);
                             isMoved = (isMoved == false) ? hasMoved : true;
                         }
                     }
                 }
                 break;
         }
        } while (hasMoved);
     
        // 랜덤 카드 소환.
        if (isMoved)
        {
            AddRandomCard(Random.value < 0.9f ? 2 : 4); // 90% 확률로 2를 생성, 10% 확률로 4를 생성
        }

        // Merge 정보 리셋.
        foreach (var card in cards)
        {
            card.hasMerged = false;
        }
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
                return ScrollDirection.MoveRightX;
            }
            else
            {
                return ScrollDirection.MoveLeftX;
            }
        }
        // Moving Y
        else
        {
            if (movingValueY > 0)
            {
                return ScrollDirection.MoveUpY;
            }
            else
            {
                return ScrollDirection.MoveDownY;
            }
        }
    }
    #endregion MainMethod
}