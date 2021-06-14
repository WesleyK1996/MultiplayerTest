using Photon.Pun;
using System.IO;
using UnityEngine;

public class MyPhotonPlayer : MonoBehaviour
{
    PhotonView PV;
    GameObject PlayerAvatar;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
            PlayerAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AU_Player"), Vector3.zero, Quaternion.identity);
    }
}
