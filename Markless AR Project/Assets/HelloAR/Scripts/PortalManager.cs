using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera;

    public GameObject Video;

    public Material[] VideoMaterials;
    
    // Start is called before the first frame update
    void Start()
    {
        VideoMaterials = Video.GetComponent<Renderer>().sharedMaterials;
    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
        Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);

        if (camPositionInPortalSpace.y < 1.0f)
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
        }
        else
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
        }
    }
}
