using UnityEngine;
using TMPro;

public class TimerUpdater : MonoBehaviour
{
	private TextMeshProUGUI textMesh;

	public void Start()
	{
		textMesh = transform.GetComponent<TextMeshProUGUI>();
	}

	public void UpdateTimer(float time)
	{
		int timeInt = Mathf.FloorToInt(time);
		int m = Mathf.FloorToInt(timeInt / 60);
		int s = timeInt - (m * 60);

		string _text;
		float _textSize;

		if (m >= 1)
		{
			_textSize = 30f;
			if (s < 10)
			{
				_text = $"{m}:0{s}";
			}
			else
			{
				if (s.ToString().Length == 2)
				{
					_text = $"{m}:{s}";
					 
				}
				else
				{
					_text = $"{m}:{s}0";
				}
			}
		}
		else
		{
			_textSize = 35f;

			if (s.ToString().Length == 2 || s < 10)
			{
				_text = $"{s}";

			}
			else
			{
				_text = $"{s}0";
			}
		}

		textMesh.text = _text;
		textMesh.fontSize = _textSize;
	}
}
