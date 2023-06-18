using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game1Card : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text textNumber;

    public int number { get; set; }
    #endregion Variables

    #region UnityMethod
    #endregion UnityMethod

    #region MainMethod
    public void ChangeNumber(int num)
    {
        if (num == 0)
        {
            textNumber.text = string.Empty;
            return;
        }

        textNumber.text = num.ToString();

    }

    public bool Move(ScrollDirection direction, int currentX, int currentY, Game1Card[,] cards)
    {
        bool hasMoved = false; // 이동 여부를 나타내는 변수

        // 이동할 위치
        int newX = currentX;
        int newY = currentY;

        // 이동할 방향에 따라 새로운 위치 계산
        if (direction == ScrollDirection.MoveUpY)
        {
            newY++;
        }
        else if (direction == ScrollDirection.MoveDownY)
        {
            newY--;
        }
        else if (direction == ScrollDirection.MoveLeftX)
        {
            newX--;
        }
        else if (direction == ScrollDirection.MoveRightX)
        {
            newX++;
        }

        // 이동 가능한 경우
        if (newX >= 0 && newX < cards.GetLength(0) && newY >= 0 && newY < cards.GetLength(1))
        {
            // 이동할 위치에 타일이 없으면 이동
            if (cards[newX, newY].number == 0)
            {
                cards[newX, newY].number = this.number;
                cards[newX, newY].ChangeNumber(this.number);
                cards[currentX, currentY].number = 0;
                cards[currentX, currentY].ChangeNumber(0);

                hasMoved = true;
            }
            // 이동할 위치에 타일이 있고, 값이 같으면 병합
            else if (cards[newX, newY].number == cards[currentX, currentY].number)
            {
                // 병합 처리
                int mergedValue = cards[currentX, currentY].number * 2;
                cards[newX, newY].number = mergedValue;
                cards[newX, newY].ChangeNumber(mergedValue);
                cards[currentX, currentY].number = 0;
                cards[currentX, currentY].ChangeNumber(0);
                hasMoved = true;

                // 점수 갱신
                //GameManager.Instance.AddScore(mergedValue);
            }
        }

        return hasMoved;
    }
    #endregion MainMethod
}
