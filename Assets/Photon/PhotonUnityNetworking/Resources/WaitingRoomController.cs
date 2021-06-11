using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    PhotonView myPhotonView;

    [SerializeField]
    int multiplayerSceneIndex;
    [SerializeField]
    int menuSceneIndex;

    int playerCount;
    int roomSize;
    [SerializeField]
    int minPlayerToStart;

    [SerializeField]
    TextMeshProUGUI roomCountDisplay;
    [SerializeField]
    TextMeshProUGUI timerToStartDisplay;

    bool readyToCountDown;
    bool readyToStart;
    bool startingGame;
    float timerToStartGame;
    float notFullGameTimer;
    float fullGameTimer;

    [SerializeField]
    float maxWaitTime;
    [SerializeField]
    float maxFullGameWaitTime;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        ResetTimer();

        PlayerCountUpdate();
    }

    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text = playerCount + ":" + roomSize;

        if (playerCount == roomSize)
            readyToStart = true;
        else if (playerCount >= minPlayerToStart)
            readyToCountDown = true;
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
    }

    /// <summary>
    /// Syncs timer for those that join after countdown has started
    /// </summary>
    /// <param name="timeIn"></param>
    [PunRPC]
    void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
            fullGameTimer = timeIn;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }

    private void WaitingForMorePlayers()
    {
        if (playerCount <= 1)
            ResetTimer();
        if (readyToStart)
        {
            //fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer -= Time.deltaTime;
        }
        else if (readyToCountDown)
        {
            //notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer -= Time.deltaTime;
        }

        timerToStartDisplay.text = string.Format("{0:00}", timerToStartGame);

        if (timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void Cancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
