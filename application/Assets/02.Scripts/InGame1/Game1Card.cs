using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game1Card : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text textNumber;

    public int cardNumber { get; private set; }
    public bool hasMerged { get; set; } // 병합 상태를 나타내는 변수

    private Game1TopPanel topPanel;
    #endregion Variables

    #region UnityMethod

    private void Awake()
    {
        topPanel = FindObjectOfType<Game1TopPanel>();
    }

    #endregion UnityMethod

    #region MainMethod
    public void ChangeNumber(int num)
    {
        if (num == 0)
        {
            textNumber.text = string.Empty;
        }
        else
        {
            textNumber.text = num.ToString();
        }

        cardNumber = num;
    }

    public bool Move(ScrollDirection direction, int currentX, int currentY, Game1Card[,] cards)
    {
        bool hasMoved = false; // 이동 여부를 나타내는 변수

        // 이동할 위치
        int newX = currentX;
        int newY = currentY;

        // 이동할 방향에 따라 새로운 위치 계산
        switch (direction)
        {
            case ScrollDirection.MoveUpY:
                newX--;
                break;
            case ScrollDirection.MoveDownY:
                newX++;
                break;
            case ScrollDirection.MoveLeftX:
                newY--;
                break;
            case ScrollDirection.MoveRightX:
                newY++;
                break;
        }

        // 이동 가능한 경우
        if (newX >= 0 && newX < cards.GetLength(0) && newY >= 0 && newY < cards.GetLength(1))
        {
            if (cards[currentX, currentY].hasMerged == true)
                return false;
            
            // 이동할 위치에 타일이 없으면 이동
            if (cards[newX, newY].cardNumber == 0)
            {
                cards[newX, newY].ChangeNumber(this.cardNumber);
                cards[currentX, currentY].ChangeNumber(0);

                hasMoved = true;
            }
            // 이동할 위치에 타일이 있고, 값이 같으면 병합
            else if (!cards[newX, newY].hasMerged && cards[newX, newY].cardNumber == this.cardNumber)
            {
                // 병합 처리
                int mergedValue = this.cardNumber * 2;
                cards[newX, newY].ChangeNumber(mergedValue);
                cards[newX, newY].hasMerged = true;
                
                //점수 처리
                topPanel.SetScore(this.cardNumber);
                
                // 병합 후 처리
                cards[currentX, currentY].ChangeNumber(0);

                hasMoved = true;
            }
        }

        return hasMoved && !hasMerged;
    }
    #endregion MainMethod
}