using System.Linq;
using Godot;
using PhantomCamera;

public partial class FuseBoxPuzzle : BasePuzzle
{
    [Export] public Node fuseParent;
    [Export] public PackedScene fuseSlider;
    [Export] Fuse[] fuses;
    [Export] protected Label3D nameLabel;
    [Export] protected Label3D descriptionLabel;
    private int currentSelectedFuseIndex;
    public override void _EnterTree()
    {
        base._EnterTree();
        if (nameLabel != null)
            nameLabel.Text = Data.Name;
        if (descriptionLabel != null)
            descriptionLabel.Text = Data.Description;
        for (int i = 0; i < fuses.Length; i++)
        {
            var slider = fuseSlider.Instantiate() as FuseSlider;
            fuseParent.AddChild(slider);
            fuseParent.MoveChild(slider, i);
            fuses[i].Setup(slider);
        }
    }
    public override void Interact()
    {
        base.Interact();
        foreach (var fuse in fuses)
        {
            if (fuse.IsSelected)
                fuse.UnSelect();
        }
        currentSelectedFuseIndex = 0;
        if (!fuses[currentSelectedFuseIndex].IsSelected)
            fuses[currentSelectedFuseIndex].Select();
    }
    public override void Back()
    {
        base.Back();
        if (fuses[currentSelectedFuseIndex].IsSelected)
            fuses[currentSelectedFuseIndex].UnSelect();
    }
    public override void Reset()
    {
        base.Reset();
        foreach (var fuse in fuses)
        {
            fuse.Reset();
        }
        fuses[currentSelectedFuseIndex].UnSelect();
        currentSelectedFuseIndex = 0;
        fuses[currentSelectedFuseIndex].Select();
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
