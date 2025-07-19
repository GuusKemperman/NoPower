using System.Collections.Generic;
using UnityEngine;

public class Lighting_LineRendererController : MonoBehaviour
{
    [SerializeField] List<LineRenderer> LineRenderers = new List<LineRenderer>();

    public void SetPosition(Transform PositionStart, Transform PositionEnd)
    {
        if (LineRenderers.Count > 0)
        {
            for (int i = 0; i < LineRenderers.Count; i++)
            {
                if (LineRenderers[i].positionCount >= 2)
                {
                    LineRenderers[i].SetPosition(0, PositionStart.position);
                    LineRenderers[i].SetPosition(1, PositionEnd.position);
                }
                else { Debug.Log("WARN: @LineRendererController, SetPosition(). Lines should have at least 2 positions."); }
            }
        }
        else { Debug.Log("WARN: @LineRendererController, SetPosition(). LineRenderers serialized variable is empty."); }
    }
}
