using System;
using Gtk;
using Summer_work;
using System.Collections.Generic;

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

	class Storage
	{
		public static List<Mount> passed;
		public static List<Anchor> anchorsDB;
		public static List<Dowel> dowelsDB;
		public static List<Screw> screwDB;
	}

	public enum Materials
	{
		GKL, Tree, Metal, Brick, Concrete, FoamBlock
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
		Tree, Metal, PO, PS, Roof, Uni, Capercaillie
	}
}
