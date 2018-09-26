using UnityEngine;

/// <summary>
/// The purpose of this class is to effectively act as
/// a Singleton wrapper around the Board component with 
/// the same indexer properties.
/// 
/// </summary>
/// <typeparam name="GameBoard"></typeparam>
public class GameBoard : Singleton<GameBoard> {
	#region Members
	private Board board;

	/// <summary>
	/// This is an indexer that allows you to access the
	/// board component's indexer from a higher level.
	/// </summary>
	public EPieceType this[int i] => board[i];
	#endregion

	#region Methods
	public void Start() {
		board = GetComponent<Board>();
	}

	#region Encapsulation Of Board Methods
	public void PlacePiece(Piece piece) {
		board.PlacePiece(piece);
		StrategyGame.Instance.Pieces.Add(piece);
		MoveGeneration.GenerateMoves(StrategyGame.Instance.Pieces);
	}

	public void MovePiece(Move move) {
		board.MovePiece(move);
        MoveGeneration.GenerateMoves(StrategyGame.Instance.Pieces);
        Debug.Log("================== Moves Generated ==================");
        Debug.Log("Moves:");
        foreach (var m in StrategyGame.Instance.Pieces[0].Moves) {
            Debug.Log("Move: " + m.From + " - " + m.To);
        }
    }

    public bool PieceAt(int index) {
        return board.PieceAt(index);
    }

    public bool PieceAt(int index, EDirection direction) {
        return board.PieceAt(index, direction);
    }

    public bool PieceAt(int index, EPieceType pieceType, EDirection direction) {
        return board.PieceAt(index, pieceType, direction);
    }
    #endregion
    #endregion
}
