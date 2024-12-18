using UIKit;

namespace HelloMaui;

public class CustomButton : UIButton
{
	public UIView? Parent { get; set; }

	public void Add(CustomButton subview)
	{
		subview.Parent = this;
		AddSubview(subview);
	}
}