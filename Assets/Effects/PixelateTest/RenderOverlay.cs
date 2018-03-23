using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderOverlay : MonoBehaviour
{
	[SerializeField]
	private Material filter;

	public void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if(filter != null)
			Graphics.Blit (src, dest, filter);
	}
}
