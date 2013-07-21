using System;
using Summer_work;
using System.Collections.Generic;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Summer_work
{
	public class Storage
	{
		public static List<Mount> passed = new List<Mount>();
		public static List<Anchor> anchorsDB = new List<Anchor>();
		public static List<Dowel> dowelsDB = new List<Dowel>();
		public static List<Screw> screwDB = new List<Screw>();


		public Stream GetStreamFromResource (string resName)
		{
			Stream str;
			System.Reflection.Assembly _assembly;
			_assembly = Assembly.GetExecutingAssembly ();
			str = _assembly.GetManifestResourceStream(resName);
			return str;
		}



		public void ReadDataResoursesAnchor ()
		{
			Summer_work.AnchorType type; 
			float max_avulsion_force;
			float max_cut_force;
			bool is_throughwall;
			float d;
			float lenght;
			Materials[] accepted_material;
			float max_a;
			float max_s;
			float bolt_d;

			string s;
			StreamReader strr = new StreamReader(GetStreamFromResource("Summer_work.Data.Anchors.dat"));
			while((s = strr.ReadLine()) != null)
			{
				while ((s = strr.ReadLine ()) != null && (s[0] == '#'));
				string t;
				t = s.Substring(0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				Summer_work.AnchorType.TryParse<Summer_work.AnchorType>(t,out type);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_avulsion_force = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_cut_force = float.Parse (t);
				if (s [0] == '1')
					is_throughwall = true;
				else
					is_throughwall = false;
				s = s.Remove (0, 2);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				d = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				lenght = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);

				//Используя регулярные выражения (pattern - что ищем, source - где ищем):
				int sizeOfMat = new Regex (",").Matches (t).Count;
				accepted_material = new Materials[sizeOfMat+1];
				for (int i = 0; i < sizeOfMat; i++) {
					string t1 = t.Substring(0, t.IndexOf(","));
					t = t.Remove(0, t.IndexOf(",")+1);
					Materials.TryParse<Summer_work.Materials>(t1, out accepted_material[i]);

				}
				Materials.TryParse<Summer_work.Materials>(t, out accepted_material[sizeOfMat]);

				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_a = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_s = float.Parse (t);
				bolt_d = float.Parse(s);
				Anchor anch = new Anchor(type, max_avulsion_force, max_cut_force, is_throughwall, d, lenght, accepted_material, max_a, max_s, bolt_d);
				Storage.anchorsDB.Add(anch);
			}
		}

		public void ReadDataResoursesScrew ()
		{
			ScrewType type;
			float max_avulsion_force;
			float max_cut_force;
			bool is_throughwall;
			bool is_selfdrill;
			float d;
			float lenght;
			Materials[] accepted_material;
			float max_a;
			float max_s;

			string s;
			StreamReader strr = new StreamReader(GetStreamFromResource("Summer_work.Data.Screw.dat"));
			while ((s = strr.ReadLine()) != null) {
				while ((s = strr.ReadLine ()) != null && (s[0] == '#'));
				string t;
				t = s.Substring(0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				Summer_work.ScrewType.TryParse<Summer_work.ScrewType>(t, out type);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_avulsion_force = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_cut_force = float.Parse (t);
				if (s [0] == '1')
					is_throughwall = true;
				else
					is_throughwall = false;
				s = s.Remove (0, 2);
				if (s [0] == '1')
					is_selfdrill = true;
				else
					is_selfdrill = false;
				s = s.Remove (0, 2);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				d = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				lenght = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);

				//Используя регулярные выражения (pattern - что ищем, source - где ищем):
				int sizeOfMat = new Regex (",").Matches (t).Count;
				accepted_material = new Materials[sizeOfMat+1];
				for (int i = 0; i < sizeOfMat; i++) {
					string t1 = t.Substring(0, t.IndexOf(","));
					t = t.Remove(0, t.IndexOf(",")+1);
					Materials.TryParse<Summer_work.Materials>(t1, out accepted_material[i]);

				}
				Materials.TryParse<Summer_work.Materials>(t, out accepted_material[sizeOfMat]);

				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_a = float.Parse (t);
				max_s = float.Parse (s);

				Screw sc = new Screw(type, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s);
				Storage.screwDB.Add(sc);
			}
		}

		public void ReadDataResoursesDowels ()
		{
			DowelType type;
			float max_avulsion_force;
			bool is_throughwall;
			bool is_selfdrill;
			float d;
			float lenght;
			Materials[] accepted_material;
			float max_a;
			float max_s;
			float[] accepted_screw_d;

			string s;
			StreamReader strr = new StreamReader(GetStreamFromResource("Summer_work.Data.Dowels.dat"));
			while ((s = strr.ReadLine()) != null) {
				while ((s = strr.ReadLine ()) != null && (s[0] == '#'))
					;
				string t;
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				Summer_work.DowelType.TryParse<Summer_work.DowelType> (t, out type);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_avulsion_force = float.Parse (t);
				if (s [0] == '1')
					is_throughwall = true;
				else
					is_throughwall = false;
				s = s.Remove (0, 2);
				if (s [0] == '1')
					is_selfdrill = true;
				else
					is_selfdrill = false;
				s = s.Remove (0, 2);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				d = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				lenght = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);

				//Используя регулярные выражения (pattern - что ищем, source - где ищем):
				int sizeOfMat = new Regex (",").Matches (t).Count;
				accepted_material = new Materials[sizeOfMat+1];
				for (int i = 0; i < sizeOfMat; i++) {
					string t1 = t.Substring (0, t.IndexOf (","));
					t = t.Remove (0, t.IndexOf (",") + 1);
					Materials.TryParse<Summer_work.Materials> (t1, out accepted_material [i]);

				}
				Materials.TryParse<Summer_work.Materials> (t, out accepted_material [sizeOfMat]);

				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_a = float.Parse (t);
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				max_s = float.Parse (t);

				int sizeOfScr = new Regex(",").Matches(s).Count;

				accepted_screw_d = new float[sizeOfScr+1];
				for(int i = 0; i < sizeOfScr; i++)
				{
					string t1 = s.Substring(0, s.IndexOf(","));
					s =s.Remove(0, s.IndexOf(",")+1);
					accepted_screw_d[i] = float.Parse(t1);
				}
				accepted_screw_d[sizeOfScr] = float.Parse(s);

				Dowel dw = new Dowel (type, max_avulsion_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s, accepted_screw_d);

				Storage.dowelsDB.Add (dw);
			}
		}
	}
}

