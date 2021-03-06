using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAlphaSetter : MonoBehaviour
{
	public Image m_image;

	public CanvasGroup m_canvasGroup;

	public UIAlphaSetter()
	{
	}

	public void SetAlpha(float alpha)
	{
		if (this.m_image != null)
		{
			Color mImage = this.m_image.color;
			mImage.a = alpha;
			this.m_image.color = mImage;
		}
		if (this.m_canvasGroup != null)
		{
			this.m_canvasGroup.alpha = alpha;
		}
	}

	public void SetAlphaZero()
	{
		if (this.m_image != null)
		{
			Color mImage = this.m_image.color;
			mImage.a = 0f;
			this.m_image.color = mImage;
		}
		if (this.m_canvasGroup != null)
		{
			this.m_canvasGroup.alpha = 0f;
		}
	}
}