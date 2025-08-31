using Godot;

public partial class FuseSlider : Control
{
    [Export] public Slider slider;
    [Export] private Slider targetSlider;
    [Export] private Label moveAmountLabel;
    public void SetTarget(int target)
    {
        targetSlider.Value = target;
    }
    public void SetMoveAmount(int amount)
    {
        moveAmountLabel.Text = $"[{amount}]";
    }
    public void SetValue(int value)
    {
        slider.Value = value;
    }
}