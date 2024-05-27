using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScrollbar : MonoBehaviour
{
    public ScrollRect Rect;
	public RectTransform Rect2;
	public Slider ScrollSlider;

	public bool usePivot;

	public void Start()
	{
		Update();
	}

	private void Update()
	{
		
		if(!ScrollSlider)
			return;
			
		if(Rect){
			Rect.horizontalNormalizedPosition = ScrollSlider.value;
		}
		if(Rect2){
			var factor = (ScrollSlider.value/ScrollSlider.maxValue);
			if(usePivot){
				factor -= Rect2.pivot.x;
			}
			Rect2.position = new Vector3( Rect2.rect.width * factor, Rect2.position.y,Rect2.position.z);
		}
	}
}
