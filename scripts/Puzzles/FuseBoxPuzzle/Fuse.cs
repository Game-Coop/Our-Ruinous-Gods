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
    private int originalValue;
    public void Setup(FuseSlider slider)
    {
        this.fuseSlider = slider;
        originalValue = value;
        slider.SetTarget(target);
        fuseSlider.SetMoveAmount(moveAmount);
    }
    public void Reset()
    {
        value = originalValue;
        fuseSlider.SetValue(originalValue);
        fuseSlider.SetMoveAmount(moveAmount);
    }
    public void Select()
    {
        IsSelected = true;
        tempMoveAmount = moveAmount;
        moveAmount = 1;
        fuseSlider.SetMoveAmount(moveAmount);
        fuseSlider.slider.CallDeferred("grab_focus");
    }
    public void UnSelect()
    {
        moveAmount = tempMoveAmount;
        fuseSlider.SetMoveAmount(moveAmount);
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