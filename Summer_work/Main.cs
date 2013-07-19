using System;
using Gtk;

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

	enum Materials
	{
		GKL, Tree, Metal, Brick, Concrete, FoamBlock
	}

	enum AnchorType
	{
		Driven, Frame, Sleeve, Wedged
	}

	enum DowelType
	{
		Standart, Butterfly, Nail
	}

	enum ScrewType
	{
		Tree, Metal, PO, PS, Roof, Uni, Capercaillie
	}
}
