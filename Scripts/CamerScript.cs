using UnityEngine;

public class CamerScript : MonoBehaviour
{
    Chessboard board;
    Vector3 whitePlayPosition = new Vector3(0, 10.3000002f, -8.31999969f);
    Vector3 blackPlayPosition = new Vector3(0, 10.3000002f, 8.31999969f);
    void Start()
    {
        board = GameObject.Find("ChessBoard").GetComponent<Chessboard>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(board.transform.position);
        if (board.isWhiteTurn)
            transform.position = Vector3.Lerp(transform.position, whitePlayPosition, Time.deltaTime * 40);
        else
            transform.position = Vector3.Lerp(transform.position, blackPlayPosition, Time.deltaTime * 40);

    }
}
