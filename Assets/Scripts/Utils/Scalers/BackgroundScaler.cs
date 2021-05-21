using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
	[SerializeField] private Camera _camera;
    [SerializeField] private bool _keepAspectRatio;

	private void SetScale(int size)
    {
		if(Screen.orientation == ScreenOrientation.Portrait)
        {
			_camera.orthographicSize = size;
		}

		var topRightCorner = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _camera.transform.position.z));
		var worldSpaceWidth = topRightCorner.x * 2;
		var worldSpaceHeight = topRightCorner.y * 2;

		var spriteSize = GetComponent<SpriteRenderer>().bounds.size;

		var scaleFactorX = worldSpaceWidth / spriteSize.x;
		var scaleFactorY = worldSpaceHeight / spriteSize.y;

		if (_keepAspectRatio)
		{
			if (scaleFactorX > scaleFactorY)
			{
				scaleFactorY = scaleFactorX;
			}
			else
			{
				scaleFactorX = scaleFactorY;
			}
		}

		transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
	}
}
