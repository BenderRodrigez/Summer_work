using System;
using Summer_work;
using System.Collections.Generic;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Summer_work
{
	public class Calculator
	{
		public static Dowel DowelToScrew(Screw scr, Materials what, Materials wher)
		{
			foreach(Dowel dwl in Storage.dowelsDB)
			{
				if(dwl.type != DowelType.Nail){
					if((scr.lenght < (dwl.lenght/0.8)) && (scr.lenght > (dwl.lenght*0.5))){
						if(scr.d >= dwl.accepted_screw_d[0] && scr.d <= dwl.accepted_screw_d[dwl.accepted_screw_d.Length-1])
							return dwl;
					}
				}
			}
			return null;
		}

		public static void GenerateByMaterialList (Materials what, Materials wall)//In Dowel is only Nail in work! Standart, Butterfly in Screw
		{
			if (Storage.passed.Count < 1) {
				foreach (Anchor anch in Storage.anchorsDB) {
					if (anch.CanByMaterial (what, wall))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (anch);
					}
				}
				foreach (Dowel dow in Storage.dowelsDB) {
					if(dow.type != DowelType.Butterfly && dow.type != DowelType.Standart){
						if (dow.CanByMaterial (what,  wall))
						{
							if(Storage.passed.Capacity-1 == Storage.passed.Count)
								Storage.passed.Capacity*=2;
							Storage.passed.Add (dow);
						}
					}
				}
				foreach (Screw scr in Storage.screwDB) {
					if (scr.CanByMaterial (what, wall))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (scr);
					}
				}
			} else {
				List<Mount> toDelete = new List<Mount>();
				foreach(Mount mn in Storage.passed){
					if(!mn.CanByMaterial(what, wall))
						toDelete.Add(mn);
				}
				foreach(Mount mn in toDelete){
						Storage.passed.Remove(mn);
				}
			}
		}

		public static void GenerateByLenght (float wallLenght, float objLenght)
		{
			if (Storage.passed.Count < 1) {
				foreach (Anchor anch in Storage.anchorsDB) {
					if (anch.CanPassByLenght (wallLenght, objLenght))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (anch);
					}
				}
				foreach (Dowel dow in Storage.dowelsDB) {
					if (dow.CanPassByLenght (wallLenght, objLenght))
					{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (dow);
					}				}
				foreach (Screw scr in Storage.screwDB) {
					if (scr.CanPassByLenght (wallLenght, objLenght))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (scr);
					}
				}
			}
			else {
				List<Mount> toDelete = new List<Mount>();
				foreach(Mount mn in Storage.passed){
					if(!mn.CanPassByLenght(wallLenght, objLenght))
						toDelete.Add(mn);
				}
				foreach(Mount mn in toDelete){
						Storage.passed.Remove(mn);
				}
			}
		}

		public static void GenerateByForce (int vector, float force)
		{
			if (Storage.passed.Count < 1) {
				foreach (Anchor anch in Storage.anchorsDB) {
					if (anch.CanPassByForce(vector, force))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (anch);
					}
				}
				foreach (Dowel dow in Storage.dowelsDB) {
					if (dow.CanPassByForce(vector, force))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (dow);
					}
				}
				foreach (Screw scr in Storage.screwDB) {
					if (scr.CanPassByForce(vector, force))
						{
						if(Storage.passed.Capacity-1 == Storage.passed.Count)
							Storage.passed.Capacity*=2;
						Storage.passed.Add (scr);
					}
				}
			}
			else {
				List<Mount> toDelete = new List<Mount>();
				foreach(Mount mn in Storage.passed){
					if(!mn.CanPassByForce(vector, force))
						toDelete.Add(mn);
				}
				foreach(Mount mn in toDelete){
						Storage.passed.Remove(mn);
				}
			}
		}

		public Calculator ()
		{
		}
	}
}

