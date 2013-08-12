using System;
using Gtk;
using Summer_work;
using System.Collections.Generic;
using System.Timers;

namespace Summer_work
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}

	public enum Materials
	{
		GKL, Tree, Metal, Brick, Concrete, FoamBlock, Dowel, None = -1
	}

	public enum AnchorType
	{
		Driven, Frame, Sleeve, Wedged
	}

	public enum DowelType
	{
		Standart, Butterfly, Nail
	}

	public enum ScrewType
	{
		Tree, Metal, PO, PS, Roof, Uni, Capercaillie, Concerete
	}
}
