using ETModel;
using UnityEngine;
//using UnityEngine.Rendering.Universal;

namespace ETHotfix
{
	[ObjectSystem]
	public class CameraComponentAwakeSystem : AwakeSystem<CameraComponent>
	{
		public override void Awake(CameraComponent self)
		{
			self.Awake();
		}
	}

	public class CameraComponent : Component
	{
		public Camera MainCamera;
		public  void Awake()
		{
 			if (Camera.main == null)
			{
				MainCamera = new GameObject("[MainCamera]").AddComponent<Camera>();
			}
			MainCamera = Camera.main;
			MainCamera.tag = "MainCamera";
			//var uicameradata = Game.Scene.GetComponent<UIComponent>().Camera.GetUniversalAdditionalCameraData();
			//uicameradata.renderType = CameraRenderType.Overlay;
			//var uicamera = Game.Scene.GetComponent<UIComponent>().Camera;
			//MainCamera.transform.SetParent(uicamera.transform.parent);
 		    //var cameradata = MainCamera.GetUniversalAdditionalCameraData();
			//cameradata.cameraStack.Add(uicamera);
 		}

		public override void Dispose()
		{
			base.Dispose();
			//var uicameradata = Game.Scene.GetComponent<UIComponent>().Camera.GetUniversalAdditionalCameraData();
			//uicameradata.renderType = CameraRenderType.Base;
		}
	}
}
