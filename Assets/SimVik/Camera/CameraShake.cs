using DG.Tweening;
using SimVik.Utility;

public class CameraShake : Singleton<CameraShake>
{
	// shake with dotween

	 public static void Shake()
	 {
		 Instance.transform.DOShakePosition( 0.5f, 0.5f, 10, 90, false, true);
	 }
}