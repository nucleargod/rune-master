using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class redEffect : MonoBehaviour
{
	
	public Vector4 colorAdd;
	public Shader   shaderRGB;
	private Material m_MaterialRGB;
	
	protected void Start ()
	{
		// Disable if we don't support image effects
		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		
		if( shaderRGB == null)
		{
			Debug.Log( "Noise shaders are not set up! Disabling noise effect." );
			enabled = false;
		}
	}
	
	protected Material material {
		get {
			if( m_MaterialRGB == null ) {
				m_MaterialRGB = new Material( shaderRGB );
				m_MaterialRGB.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_MaterialRGB;
		}
	}
	
	protected void OnDisable() {
		if( m_MaterialRGB )
			DestroyImmediate( m_MaterialRGB );
	}

	// Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		Material mat = material;

		mat.SetVector("colorAdd", colorAdd);
		Graphics.Blit (source, destination, mat);
	}
}
