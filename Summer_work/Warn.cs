using System;

namespace Summer_work
{
	public partial class Warn : Gtk.Dialog
	{
		public Warn ()
		{
			this.Build();
		}

		public void SetLabel(string message)
		{
			showed_message.LabelProp = message;
		}
	}
}

