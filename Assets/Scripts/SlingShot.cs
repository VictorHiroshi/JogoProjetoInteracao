using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour {

	public Vector3 positionOffset;
	public float width;
	public Transform slingShotBase;

	LineRenderer lineRenderer;

	void Start() {

		lineRenderer = gameObject.GetComponent<LineRenderer>();

		lineRenderer.material = new Material(Shader.Find("Custom/Solid Color"));
		lineRenderer.SetWidth(width, width);

		lineRenderer.SetPosition((int)SLINGSHOT_LINE_POS.SLING, transform.position + positionOffset);
		lineRenderer.enabled = false;
	}

    void Update()
    {
        if (lineRenderer)
        {
            lineRenderer.enabled = true;
			lineRenderer.SetPosition((int)SLINGSHOT_LINE_POS.BIRD, slingShotBase.position);
        }
        else

            lineRenderer.enabled = false;
    }
}
