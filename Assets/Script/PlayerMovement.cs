using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PhotonView myPhotonView;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (myPhotonView.IsMine)
            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0, Input.GetAxis("Vertical") * Time.deltaTime));
        myPhotonView.RPC("RPC_SendPosition", RpcTarget.All, transform.position);
    }

    [PunRPC]
    void RPC_SendPosition(Vector3 pos)
    {
        print(myPhotonView.InstantiationId);
        if (!myPhotonView.IsMine)
            transform.position = pos;
    }
}
