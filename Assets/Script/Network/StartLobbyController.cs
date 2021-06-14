using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StartLobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject startBtn;
    [SerializeField]
    GameObject cancelBtn;
    [SerializeField]
    int RoomSize;

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;
        startBtn.SetActive(true);
    }

    public void DoStart()
    {
        startBtn.SetActive(false);
        cancelBtn.SetActive(true);
        PhotonNetwork.JoinRandomRoom();//try to join excisting room
        Debug.Log("Starting...");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) //Callback when JoinRandomRoom() fails
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomops = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomops);
        Debug.Log(randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Failed room creation... trying again");
        CreateRoom();
    }

    public void Cancel()
    {
        startBtn.SetActive(true);
        cancelBtn.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }
}
