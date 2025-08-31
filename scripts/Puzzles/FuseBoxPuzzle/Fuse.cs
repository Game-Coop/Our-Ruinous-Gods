using Godot;

[GlobalClass]
public partial class Fuse : Resource
{
    public bool IsAlligned { get; private set; }
    [Export(PropertyHint.Range, "0, 10, 1")]
    public int value;
    [Export(PropertyHint.Range, "0, 10, 1")]
    public int target;
    [Export(PropertyHint.Range, "0, 10, 1")]
    public int moveAmount;
    private int tempMoveAmount = 1;
    public FuseSlider fuseSlider;
    public bool IsSelected { get; private set; }
    public void Setup(FuseSlider slider)
    {
        this.fuseSlider = slider;
        slider.SetTarget(target);
    }
    public void Select()
    {
        IsSelected = true;
        tempMoveAmount = moveAmount;
        moveAmount = 1;
        fuseSlider.slider.CallDeferred("grab_focus");
    }
    public void UnSelect()
    {
        moveAmount = tempMoveAmount;
    }
    public void MoveRight()
    {
        value += moveAmount;
        value = Mathf.Wrap(value, 0, 10);
        fuseSlider.SetValue(value);

        GD.Print("Value is: " + value);
        SetStatus();
    }
    public void MoveLeft()
    {
        value -= moveAmount;
        value = Mathf.Wrap(value, 0, 10);
        fuseSlider.SetValue(value);

        GD.Print("Value is: " + value);
        SetStatus();
    }
    private void SetStatus()
    {
        IsAlligned = target == value;
    }
}