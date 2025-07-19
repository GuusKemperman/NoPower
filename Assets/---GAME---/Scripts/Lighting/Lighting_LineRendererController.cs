using System.Collections.Generic;
using UnityEngine;

public class Lighting_LineRendererController : MonoBehaviour
{
    //[SerializeField] List<LineRenderer> LineRenderers = new List<LineRenderer>();

    LineRenderer myRendererUwu = null;

    private void Start()
    {
        myRendererUwu = GetComponent<LineRenderer>();
    }



    public void SetPosition(Vector3 PositionStart, Vector3 PositionEnd)
    {
        myRendererUwu.positionCount = myRendererUwu.positionCount + 3;
        myRendererUwu.SetPosition(myRendererUwu.positionCount - 3, PositionStart);
        myRendererUwu.SetPosition(myRendererUwu.positionCount - 2, PositionEnd);

        //if (LineRenderers.Count > 0)
        {
            //for (int i = 0; i < LineRenderers.Count; i++)
            {
                //if (LineRenderers[i].positionCount >= 2)
                //{
                //    LineRenderers[i].SetPosition(0, PositionStart);
                //    LineRenderers[i].SetPosition(1, PositionEnd);
                //}
                //else { Debug.Log("WARN: @LineRendererController, SetPosition(). Lines should have at least 2 positions."); }
            }
        }
       // else { Debug.Log("WARN: @LineRendererController, SetPosition(). LineRenderers serialized variable is empty."); }
    }

    public void SetPosition(Transform PositionStart, Transform PositionEnd)
    {
        SetPosition(PositionStart.position, PositionEnd.position);
    }
}
