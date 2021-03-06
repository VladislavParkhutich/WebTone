namespace Core.SeleniumUtils.WindowsHandling.Windows
{
	/// <summary>
	///     PrintDialog class.
	/// </summary>
	public class PrintDialog : ClosingWindow
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="PrintDialog" /> class.
		/// </summary>
		public PrintDialog() : base("Print")
		{
		}

		/// <summary>
		///     Closes this instance.
		/// </summary>
		public override void Close()
		{
			var possibleNames = GetPossibleName();
			WinAPIUtils.CloseWindow(possibleNames);
		}
	}
}