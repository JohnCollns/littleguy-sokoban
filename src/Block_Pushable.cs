using Godot;
using System;

[Tool]
public partial class Block_Pushable : Block_Base
{
    [Export] public bool bCanBePushed { get; protected set; }
    
    // Problem with Godot itself, need to copy paste this in all child classes :(
    [ExportToolButton("Apply Settings")]
    public override Callable ApplySettingsButtonCallable => Callable.From(ApplySettings);
}
