using Unity.VisualScripting;
using UnityEngine;

namespace JumpChess
{        
    public enum Direction { Left, Right, Top, Bottom }

    public class ChessPiece : MonoBehaviour
    {
        private JumpChessMG _mg;
        private ChessPiece[,] Board => _mg.Board;
        private (bool top, bool right, bool bottom, bool left) isFree;
        public (int row, int col) pos;

        public void Initialize(JumpChessMG mg,(int row, int col) position)
        {
            _mg = mg;
            pos = position;
        }
        
        public bool IsDirectionAvailabe(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return pos.col - 1 >= 0 && Board[pos.row, pos.col - 1] != null;

                case Direction.Right:
                    return pos.col + 1 < Board.GetLength(1) && Board[pos.row, pos.col + 1] != null;

                case Direction.Top:
                    return pos.row - 1 >= 0 && Board[pos.row - 1, pos.col] != null;

                case Direction.Bottom:
                    return pos.row + 1 < Board.GetLength(0) && Board[pos.row + 1, pos.col] != null;
            }

            return false; 
        }


        public ChessPiece GetNeighbor(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (pos.col - 1 >= 0)
                        return Board[pos.row, pos.col - 1];
                    break;

                case Direction.Right:
                    if (pos.col + 1 < Board.GetLength(1))
                        return Board[pos.row, pos.col + 1];
                    break;

                case Direction.Top:
                    if (pos.row - 1 >= 0)
                        return Board[pos.row - 1, pos.col];
                    break;

                case Direction.Bottom:
                    if (pos.row + 1 < Board.GetLength(0))
                        return Board[pos.row + 1, pos.col];
                    break;
            }

            return null; // Return null if no valid neighbor is found
        }
        
    }
}