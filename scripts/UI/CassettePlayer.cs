using Godot;
using System;

public partial class CassettePlayer : Control
{
    [Export] private Control cassetteContainer;
    [Export] private Control musicIcon;
    [Export] private Control borderLines;
    [Export] private AnimationPlayer animationPlayer;

    private bool isShowing;

    public override void _Ready()
    {
        ShowCassette(false);
        ShowMusicIcon(false);

        animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    public void ShowCassette(bool visible)
    {
        cassetteContainer.Visible = visible;
    }

    public void ShowMusicIcon(bool visible)
    {
        musicIcon.Visible = visible;
    }

    public void ShowBorderLines(bool visible)
    {
        borderLines.Visible = visible;
    }

    public void ShowMusicPlaying(bool playing)
    {
        if (playing)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        if (isShowing)
        {
            return;
        }

        isShowing = true;

        cassetteContainer.Visible = true;
        musicIcon.Visible = true;

        animationPlayer.Play("cassette_show");
    }

    private void FadeOut()
    {
        if (!isShowing)
        {
            return;
        }

        isShowing = false;
        animationPlayer.Play("cassette_hide");
    }

    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "cassette_hide")
        {
            cassetteContainer.Visible = false;
            musicIcon.Visible = false;
        }
    }
}
