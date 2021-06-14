using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PhotonView myPhotonView;
    Rigidbody rb;

    public float speed = 1.0f;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //if (myPhotonView.IsMine)
        //    transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0, Input.GetAxis("Vertical") * Time.deltaTime));
        myPhotonView.RPC("RPC_SendPosition", RpcTarget.All, transform.position);
    }

    void FixedUpdate()
    {
        if(myPhotonView.IsMine)
            Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 newVector = new Vector3(horizontal, 0, vertical);

        rb.position += newVector;
    }

    [PunRPC]
    void RPC_SendPosition(Vector3 pos)
    {
        if (!myPhotonView.IsMine)
            transform.position = pos;
    }
}
