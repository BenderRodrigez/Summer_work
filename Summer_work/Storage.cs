using System;
using Summer_work;
using System.Collections.Generic;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Summer_work
{
	public class Storage//Contains all methods needed to operate with data (in files, resurces and programm)
	{
		public static List<Mount> passed = new List<Mount>();//Result of choose
		public static List<Anchor> anchorsDB = new List<Anchor>();//All known anchors, dowels,screws
		public static List<Dowel> dowelsDB = new List<Dowel>();
		public static List<Screw> screwDB = new List<Screw>();

		private static int passed_counter = 0;//navigation in passed DB in GUI...

		public int GetCounter ()
		{
			return passed_counter;
		}

		public static Mount GetNextPassed ()
		{
			if (passed_counter < (passed.Count-1)) {
				passed_counter++;
				return passed[passed_counter];
			}
			else
				return passed[passed_counter];
		}

		public static Mount GetPrevPassed ()
		{
			if (passed_counter > 0) {
				passed_counter--;
				return passed [passed_counter];
			}
			else
				return passed[passed_counter];
		}

		public static void ConvertDB (string filename)
		{
			StreamReader reader = new StreamReader (filename);
			StreamWriter writer1 = new StreamWriter("Anchors.dat");
			StreamWriter writer2 = new StreamWriter("Dowels.dat");
			StreamWriter writer3 = new StreamWriter("Screw.dat");

            string s;
            while ((s = reader.ReadLine()) != null)
            {
				AnchorType ant;
				DowelType dwl;
				ScrewType scrw;
                float max_avulsion_force;
		        float max_cut_force;
		        bool is_selfdrill;
                bool is_throughwall;
		        float d;
		        float lenght;
		        Materials[] accepted_material;
		        float max_a;
		        float max_s;
				float[] accepted_screw_d;
				string[] srs = s.Split (' ');
				string[] srs1;
				switch (srs [0]) {
				case "Анкер/болтсгайкой":
					ant = AnchorType.Wedged;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Brick, Materials.Concrete, Materials.FoamBlock};
					anchorsDB.Add (new Anchor (ant, max_avulsion_force, max_cut_force, is_throughwall, d, lenght, accepted_material, max_a, max_s, 0));
					break;
				case "Клиновойанкер":
					ant = AnchorType.Sleeve;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Brick, Materials.Concrete, Materials.FoamBlock};
					anchorsDB.Add (new Anchor (ant, max_avulsion_force, max_cut_force, is_throughwall, d, lenght, accepted_material, max_a, max_s, 0));
					break;
				case "Забивнойанкер":
					ant = AnchorType.Driven;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in KGS, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Brick, Materials.Concrete, Materials.FoamBlock};
					anchorsDB.Add (new Anchor (ant, max_avulsion_force, max_cut_force, is_throughwall, d, lenght, accepted_material, max_a, max_s, d));
					break;
				case "БолтDIN933":
					//
					break;
				case "ГлухарьDIN571":
					scrw = ScrewType.Capercaillie;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Tree, Materials.Dowel};
					screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
					break;
				case "Дюбель\"бабочка\"":
					dwl = DowelType.Butterfly;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(300f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = true;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.GKL, Materials.Tree};
					accepted_screw_d = new float[3];
					accepted_screw_d [0] = (float)4.2;
					accepted_screw_d [1] = (float)4.5;
					accepted_screw_d [2] = 5;
					max_cut_force = accepted_screw_d[2] * 5 * 100;
					dowelsDB.Add(new Dowel(dwl, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s,accepted_screw_d));
					break;
				case "Дюбельсшурупом":
					dwl = DowelType.Nail;
					srs1 = srs [srs.Length - 2].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow((d-1.5)/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (float)((0.07f*300f*Math.PI*Math.Pow(d/2, 2))/0.13f);
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Brick, Materials.FoamBlock, Materials.Concrete};
					accepted_screw_d = new float[3];
					accepted_screw_d[0] = (float)(d-2.5);
					accepted_screw_d[1] = d-2;
					accepted_screw_d[2] = (float)(d-1.5);
					max_cut_force = accepted_screw_d[2] * 5 * 100;
					dowelsDB.Add(new Dowel(dwl, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s,accepted_screw_d));
					break;
				case "Дюбельполипр.":
					dwl = DowelType.Standart;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(300f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.GKL, Materials.Tree};
					accepted_screw_d = new float[3];
					accepted_screw_d[0] = (float)(d-2.5);
					accepted_screw_d[1] = d-2;
					accepted_screw_d[2] = (float)(d-1.5);
					max_cut_force = accepted_screw_d[2] * 5 * 100;
					dowelsDB.Add(new Dowel(dwl, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s,accepted_screw_d));
					break;
				case "Рамныйанкер":
					ant = AnchorType.Frame;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Brick, Materials.Concrete, Materials.FoamBlock};
					anchorsDB.Add (new Anchor (ant, max_avulsion_force, max_cut_force, is_throughwall, d, lenght, accepted_material, max_a, max_s, 0));
					break;
				case "Саморезскруп.рез./по":
					scrw = ScrewType.Tree;
					srs1 = srs [srs.Length - 2].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.GKL, Materials.Tree, Materials.Dowel};
					screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
					break;
				case "Саморезсчаст.рез./по":
					scrw = ScrewType.Metal;
					srs1 = srs [srs.Length - 2].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Tree, Materials.Metal, Materials.GKL, Materials.Dowel};
					screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
					break;
				case "Саморезспрессшайбой":
					switch(srs[2])
					{
					case "острый":
						scrw = ScrewType.PO;
						srs1 = srs [srs.Length - 3].Split ('х');
						d = float.Parse (srs1 [0]);
						lenght = float.Parse (srs1 [1]);
						max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
						max_avulsion_force = (0.07f*max_cut_force)/0.13f;
						is_selfdrill = false;
						is_throughwall = false;
						max_a = lenght * (float)1.5;
						max_s = lenght * 3;
						accepted_material = new Materials[]{Materials.Tree, Materials.Metal, Materials.GKL, Materials.Dowel};
						screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
						break;
					case "сверло":
						scrw = ScrewType.PS;
						srs1 = srs [srs.Length - 3].Split ('х');
						d = float.Parse (srs1 [0]);
						lenght = float.Parse (srs1 [1]);
						max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
						max_avulsion_force = (0.07f*max_cut_force)/0.13f;
						is_selfdrill = true;
						is_throughwall = false;
						max_a = lenght * (float)1.5;
						max_s = lenght * 3;
						accepted_material = new Materials[]{Materials.Tree, Materials.Metal, Materials.GKL};
						screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
						break;
					}
					break;
				case "Саморезпо":
					scrw = ScrewType.Concerete;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Concrete, Materials.Brick, Materials.FoamBlock};
					screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
					break;
				case "Саморез":
					scrw = ScrewType.Roof;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = true;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Tree, Materials.Metal};
					screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
					break;
				case "Шуруп":
					scrw = ScrewType.Uni;
					srs1 = srs [srs.Length - 1].Split ('х');
					d = float.Parse (srs1 [0]);
					lenght = float.Parse (srs1 [1]);
					max_cut_force = (float)(400f*Math.PI*Math.Pow(d/2, 2));//in Newtons, http://rostfrei.ru/edelstahl.nsf/pages/grade
					max_avulsion_force = (0.07f*max_cut_force)/0.13f;
					is_selfdrill = false;
					is_throughwall = false;
					max_a = lenght * (float)1.5;
					max_s = lenght * 3;
					accepted_material = new Materials[]{Materials.Tree, Materials.Metal, Materials.GKL, Materials.Dowel};
					screwDB.Add (new Screw (scrw, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s));
					break;
				}
            }
			foreach (Anchor anch in anchorsDB)
				writer1.WriteLine (anch.ToString ());
			foreach (Dowel dwl in dowelsDB)
				writer2.WriteLine (dwl.ToString ());
			foreach (Screw scrw in screwDB)
				writer3.WriteLine (scrw.ToString ());
			reader.Close ();
			writer1.Close ();
			writer2.Close ();
			writer3.Close ();
		}

		public static Stream GetStreamFromResource (string resName)
		{
			Stream str;
			System.Reflection.Assembly _assembly;
			_assembly = Assembly.GetExecutingAssembly ();
			str = _assembly.GetManifestResourceStream(resName);
			return str;
		}



		public static void ReadDataResoursesAnchor ()
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
				if(s[0] == '#')
					while ((s = strr.ReadLine ()) != null && (s[0] == '#'));
				if(s == null)
					return;
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

		public static void ReadDataResoursesScrew ()
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
				if(s[0] == '#')
					while ((s = strr.ReadLine ()) != null && (s[0] == '#'));
				if(s == null)
					return;
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

		public static void ReadDataResoursesDowels ()
		{
			DowelType type;
			float max_avulsion_force;
			float max_cut_force;
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
				if(s[0] == '#')
					while ((s = strr.ReadLine ()) != null && (s[0] == '#'));
				if(s == null)
					return;
				string t;
				t = s.Substring (0, s.IndexOf (" "));
				s = s.Remove (0, s.IndexOf (" ") + 1);
				Summer_work.DowelType.TryParse<Summer_work.DowelType> (t, out type);
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

				int sizeOfScr = new Regex(";").Matches(s).Count;

				accepted_screw_d = new float[sizeOfScr+1];
				for(int i = 0; i < sizeOfScr; i++)
				{
					string t1 = s.Substring(0, s.IndexOf(";"));
					s =s.Remove(0, s.IndexOf(";")+1);
					accepted_screw_d[i] = float.Parse(t1);
				}
				accepted_screw_d[sizeOfScr] = float.Parse(s);
				max_cut_force = accepted_screw_d [sizeOfScr - 1] * 5 * 100;
				Dowel dw = new Dowel (type, max_avulsion_force, max_cut_force, is_throughwall, is_selfdrill, d, lenght, accepted_material, max_a, max_s, accepted_screw_d);

				Storage.dowelsDB.Add (dw);
			}
		}
	}
}

