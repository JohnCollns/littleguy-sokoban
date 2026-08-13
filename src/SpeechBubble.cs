using Godot;

public partial class SpeechBubble : Node2D
{
	// =========================================================================
	// DIALOGUE SETTINGS
	// =========================================================================
	[ExportGroup("Dialogue Settings")]
	[Export] private string[] _chatLines = new string[]
	{
		"Mein Gott Leute",
		"Meine Mama hat mir einfach erlaubt dass ich Cola trinken darf!",
		"Wie cool ist das bitte?",
		"Jetzt zocke ich Fortnite und trinke Cola!",
        "YIPPEE!"
	};

	[Export] private bool _autoStartTimer = true;
	[Export] private float _displayDuration = 3.0f; // Time bubble stays visible

	[ExportSubgroup("Timers")]
	// Initial delay before first line triggers (Wider stagger window)
	[Export] private float _initialMinDelay = 3.0f;    
	[Export] private float _initialMaxDelay = 15.0f;   
	
	// Cooldown interval between lines (9 to 20 seconds)
	[Export] private float _minChatInterval = 9.0f;    
	[Export] private float _maxChatInterval = 20.0f;   

	// =========================================================================
	// VISUAL CUSTOMIZATION & STYLING
	// =========================================================================
	[ExportGroup("Visual Customization")]
	[Export] private Vector2 _bubbleOffset = new Vector2(0, -165);
	[Export] private int _fontSize = 72;
	[Export] private float _maxWidth = 750f;

	// Default Palette: Soft Cream + Deep Slate Indigo (Thicker 8px Border)
	[Export] private Color _backgroundColor = new Color("FFFDF0"); 
	[Export] private Color _textColor = new Color("1E1E38");       
	[Export] private Color _borderColor = new Color("1E1E38");     
	[Export] private int _borderWidth = 8;                         
	[Export] private int _cornerRadius = 22;

	// =========================================================================
	// PRIVATE VARIABLES
	// =========================================================================
	private Label _label;
	private PanelContainer _panel;
	private Node2D _parent2D;
	private Timer _loopTimer;
	private Timer _hideTimer;

	public override void _Ready()
	{
		_parent2D = GetParent<Node2D>();
		if (_parent2D == null)
		{
			GD.PrintErr("[SpeechBubble] ERROR: Parent node is null!");
			return;
		}

		TopLevel = true;
		ZIndex = 100;

		BuildBubbleUI();

		_loopTimer = new Timer { OneShot = true };
		_loopTimer.Timeout += OnLoopTimeout;
		AddChild(_loopTimer);

		_hideTimer = new Timer { OneShot = true };
		_hideTimer.Timeout += Clear;
		AddChild(_hideTimer);

		Clear();

		if (_autoStartTimer)
		{
			// First line picks a random time between 3.0 and 15.0 seconds
			float initialWait = (float)GD.RandRange(_initialMinDelay, _initialMaxDelay);
			_loopTimer.Start(initialWait);
		}
	}

	public override void _Process(double delta)
	{
		if (_parent2D == null || !GodotObject.IsInstanceValid(_parent2D))
			return;

		GlobalPosition = _parent2D.GlobalPosition + _bubbleOffset;
	}

	// =========================================================================
	// PUBLIC API
	// =========================================================================

	public void Say(string text)
	{
		if (string.IsNullOrEmpty(text) || _label == null || _panel == null) return;

		_label.Text = text;
		_panel.Visible = true;

		Font font = _label.GetThemeFont("font") ?? ThemeDB.FallbackFont;

		Vector2 singleLineSize = font.GetStringSize(text, HorizontalAlignment.Left, -1, _fontSize);
		Vector2 exactTextSize;

		if (singleLineSize.X <= _maxWidth)
		{
			_label.AutowrapMode = TextServer.AutowrapMode.Off;
			exactTextSize = singleLineSize;
		}
		else
		{
			_label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
			exactTextSize = font.GetMultilineStringSize(text, HorizontalAlignment.Center, _maxWidth, _fontSize);
		}

		_label.CustomMinimumSize = exactTextSize;
		CallDeferred(nameof(UpdatePanelAlignment));

		float duration = _displayDuration > 0 ? _displayDuration : 3.0f;
		_hideTimer.Start(duration);
	}

	public void SayRandom()
	{
		string[] lines = (_chatLines != null && _chatLines.Length > 0) 
			? _chatLines 
			: new string[] { "YIPPEE!", "Cola!" };

		int index = GD.RandRange(0, lines.Length - 1);
		Say(lines[index]);
	}

	public void Clear()
	{
		if (_panel != null) _panel.Visible = false;
	}

	// =========================================================================
	// PRIVATE HELPERS
	// =========================================================================

	private void OnLoopTimeout()
	{
		SayRandom();
		StartNextCycleTimer();
	}

	private void StartNextCycleTimer()
	{
		if (_loopTimer == null) return;

		// Subsequent lines pick a random cooldown between 9.0 and 20.0 seconds
		float min = _minChatInterval > 0 ? _minChatInterval : 9.0f;
		float max = _maxChatInterval >= min ? _maxChatInterval : min + 11.0f;

		float wait = (float)GD.RandRange(min, max);
		_loopTimer.Start(wait);
	}

	private void BuildBubbleUI()
	{
		_panel = new PanelContainer();

		var styleBox = new StyleBoxFlat
		{
			BgColor = _backgroundColor,
			CornerRadiusTopLeft = _cornerRadius,
			CornerRadiusTopRight = _cornerRadius,
			CornerRadiusBottomLeft = _cornerRadius,
			CornerRadiusBottomRight = _cornerRadius,

			BorderWidthLeft = _borderWidth,
			BorderWidthTop = _borderWidth,
			BorderWidthRight = _borderWidth,
			BorderWidthBottom = _borderWidth,
			BorderColor = _borderColor,

			ContentMarginLeft = 32,
			ContentMarginRight = 32,
			ContentMarginTop = 18,
			ContentMarginBottom = 18
		};
		_panel.AddThemeStyleboxOverride("panel", styleBox);

		_label = new Label();
		_label.AddThemeColorOverride("font_color", _textColor);
		_label.AddThemeFontSizeOverride("font_size", _fontSize);
		_label.HorizontalAlignment = HorizontalAlignment.Center;
		_label.VerticalAlignment = VerticalAlignment.Center;

		_panel.AddChild(_label);
		AddChild(_panel);
	}

	private void UpdatePanelAlignment()
	{
		if (_panel == null) return;

		_panel.ResetSize();
		_panel.Position = new Vector2(-_panel.Size.X / 2.0f, -_panel.Size.Y);
	}
}
