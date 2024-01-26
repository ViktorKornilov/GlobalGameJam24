using UnityEngine;

public static class ColorExtensions
{
	public static Color Alpha(this Color color, float alpha)
	{
		color.a = alpha;
		return color;
	}


}