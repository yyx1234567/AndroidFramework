using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShadow : UnityEngine.UI.Shadow
{
    public override void ModifyMesh(Mesh mesh)
    {
          base.ModifyMesh(mesh);
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        Debug.LogError(vh);

        base.ModifyMesh(vh);
    }
}
