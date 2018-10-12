using UnityEngine;

/// <summary>
/// The purpose of this class is to effectively act as
/// a Singleton wrapper around the Board component with 
/// the same indexer properties.
/// </summary>
/// <typeparam name="GameBoard">Creates a singleton from a generic</typeparam>
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
	private new void Awake() {
        base.Awake();
		board = GetComponent<Board>();
	}

	#region Encapsulation Of Board Methods
	public void PlacePiece(Piece piece) {
        Debug.Log(board);
		board.PlacePiece(piece);
		StrategyGame.Instance.Pieces.Add(piece);
		MoveGeneration.GenerateMoves(StrategyGame.Instance.Pieces);
	}

	public void MovePiece(Move move) {
        // Check if this is going to trigger a capture
        if (move.Capture) {
            // A capture event occurs and we switch over to the Action phase
            MasterGame.Instance.CaptureAttempted.Invoke(move);
        }
        else {
            board.MovePiece(move);
            MoveGeneration.GenerateMoves(StrategyGame.Instance.Pieces);
            Debug.Log("================== Moves Generated ==================");
            Debug.Log("Moves:");
            foreach (var m in StrategyGame.Instance.Pieces[2].Moves) {
                Debug.Log("Move: " + m.From + " - " + m.To);
            }
        }
    }

    public void RemovePiece(int index) {
        board.RemovePiece(index);
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
