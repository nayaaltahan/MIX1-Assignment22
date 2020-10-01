using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera;

    public GameObject Video;

    public Material[] VideoMaterials;

    public Material PortalPlaneMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        VideoMaterials = Video.GetComponent<Renderer>().sharedMaterials;
        PortalPlaneMaterial = GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
        Vector3 camPositionInPortalSpace = transform.InverseTransformPoint(MainCamera.transform.position);

        if (camPositionInPortalSpace.y <= 0.0f)
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int) CompareFunction.NotEqual);
            }
            
            PortalPlaneMaterial.SetInt("_CullMode", (int) CullMode.Front);
        }
        
        else if (camPositionInPortalSpace.y < 0.5f)
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Always);
            }
            
            PortalPlaneMaterial.SetInt("_CullMode", (int) CullMode.Off);
        }
        else
        {
            for (int i = 0; i < VideoMaterials.Length; i++)
            {
                VideoMaterials[i].SetInt("_StencilComp", (int)CompareFunction.Equal);
            }
            
            PortalPlaneMaterial.SetInt("_CullMode", (int) CullMode.Back);
        }
    }
}
