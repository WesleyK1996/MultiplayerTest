using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    int waitingRoomSceneIndex;

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SceneManager.LoadScene(waitingRoomSceneIndex);
    }
}
