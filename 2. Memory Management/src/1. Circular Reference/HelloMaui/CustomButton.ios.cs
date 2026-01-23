using UIKit;

namespace HelloMaui;

public class CustomButton : UIButton
{
	public void Add(CustomButton subview)
	{
		subview.Superview = this;
		AddSubview(subview);
	}
}