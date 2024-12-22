using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollwingCamera : MonoBehaviour
{

    public Transform player;
    public Vector3 offset;

    Renderer ObstacleRenderer;
    Renderer preRenderer;

    void Update()
    {
        transform.position = player.position + offset;

        // 캐릭터 가리면 반 투명 로직
        float Distance = Vector3.Distance(transform.position, player.transform.position);

        Vector3 Direction = (player.transform.position - transform.position).normalized * 15f;
        

        RaycastHit hit;

        Debug.DrawRay(transform.position, Direction, Color.green);


        if (Physics.Raycast(transform.position, Direction, out hit, 15f))
        {

            // 2.맞았으면 Renderer를 얻어온다.
            ObstacleRenderer = hit.transform.gameObject.GetComponentInChildren<Renderer>();
            preRenderer = ObstacleRenderer;
            if (ObstacleRenderer != null)

            {

                // 3. Metrial의 Aplha를 바꾼다.

                Material Mat = ObstacleRenderer.material;

                Color matColor = Mat.color;

                matColor.a = 0.5f;

                Mat.color = matColor;

            }

        }
        else if(preRenderer != null)
        {
            Material Mat = preRenderer.material;

            Color matColor = Mat.color;

            matColor.a = 1f;

            Mat.color = matColor;
        }
    }

    public void GetPlayer(Transform target)
    {
        player = target;
    }
}
