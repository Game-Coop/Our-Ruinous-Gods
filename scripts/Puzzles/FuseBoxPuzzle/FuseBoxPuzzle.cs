using System.Linq;
using Godot;

public partial class FuseBoxPuzzle : BasePuzzle
{
    [Export] public Node fuseParent;
    [Export] public PackedScene fuseSlider;
    [Export] Fuse[] fuses;
    private int currentSelectedFuseIndex;
    public override void _EnterTree()
    {
        base._EnterTree();
        foreach (var fuse in fuses)
        {
            var slider = fuseSlider.Instantiate() as FuseSlider;
            fuseParent.AddChild(slider);
            fuse.Setup(slider);
        }
    }
    public override void Interact()
    {
        base.Interact();
        if (!fuses[currentSelectedFuseIndex].IsSelected)
            fuses[currentSelectedFuseIndex].Select();
    }
    public override void Back()
    {
        base.Back();
        if (fuses[currentSelectedFuseIndex].IsSelected)
            fuses[currentSelectedFuseIndex].UnSelect();
    }
    public override void Input(Vector2 value)
    {
        base.Input(value);
        if (value.X == 1)
        {
            foreach (var item in fuses)
            {
                item.MoveRight();
            }
        }
        else if (value.X == -1)
        {
            foreach (var item in fuses)
            {
                item.MoveLeft();
            }
        }
        else if (value.Y == -1)
        {
            fuses[currentSelectedFuseIndex].UnSelect();
            currentSelectedFuseIndex += 1;
            currentSelectedFuseIndex = Mathf.Wrap(currentSelectedFuseIndex, 0, fuses.Length);
            fuses[currentSelectedFuseIndex].Select();
        }
        else if (value.Y == 1)
        {
            fuses[currentSelectedFuseIndex].UnSelect();
            currentSelectedFuseIndex -= 1;
            currentSelectedFuseIndex = Mathf.Wrap(currentSelectedFuseIndex, 0, fuses.Length);
            fuses[currentSelectedFuseIndex].Select();
        }
        CheckStatus();
    }

    public void CheckStatus()
    {
        if (fuses.All(x => x.IsAlligned))
        {
            SetSolved(true);
        }
    }
}
