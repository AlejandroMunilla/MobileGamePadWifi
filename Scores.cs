using UnityEngine;
using Mirror;

public class Scores : MonoBehaviour
{


    public class ScoreMessage : MessageBase
    {
        public float xAxis;
        public float yAxis;
        public int attack;
        public int block;
        public int jump;
        public int special;
        public int left;
        public int right;
    }

    public void SendScore(float xAxis, float yAxis, int attack, int block, int jump, int special, int left, int right)
    {
        Debug.Log("test");
        ScoreMessage msg = new ScoreMessage()
        {
            xAxis = xAxis,
            yAxis = yAxis,
            attack = attack,
            block = block,
            jump = jump,
            special = special,
            left = left,
            right = right
        


        };

        NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
        NetworkServer.RegisterHandler<ScoreMessage>(OnScore);
        NetworkClient.Send(msg);
        NetworkServer.SendToAll(msg);
    }


    public void OnScore(NetworkConnection conn, ScoreMessage msg)
    {
        Debug.Log("OnScoreMessage " + msg);
    }


    /* Only for testing
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
            NetworkServer.RegisterHandler<ScoreMessage>(OnScore);
            Debug.Log("update");
            SendScore(3, 5, 1, 0, 0, 0, 0, 0);
        }*/
    }
}