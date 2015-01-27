using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
	class Program
	{
		public class Animal
		{
			public virtual string Name
			{

				get { return "Animal"; }
			}
			public virtual string GetSkin()
			{
				return "integument";
			}
		}

		public class Reptile : Animal
		{
			public override string Name
			{
				get
				{
					return "Reptile";
				}
			}
			public override string GetSkin()
			{
				return "scales";
			}
		}
		public class Bird : Animal
		{
			public override string Name
			{
				get
				{
					return "Bird";
				}
			}
			public override string GetSkin()
			{
				return "feathers";
			}
		}
		public class Mammal : Animal
		{
			public override string Name
			{
				get
				{
					return "Mammal";
				}
			}
			public override string GetSkin()
			{
				return "hair";
			}
		}

		static void Main(string[] args)
		{
			var animals = new Animal[] { new Mammal(), new Bird(), new Reptile() };
			foreach (var animal in animals)
			{
				Console.WriteLine("{0}: {1}", animal.Name, animal.GetSkin());
			}
		}
	}
}
