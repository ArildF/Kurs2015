using System;
using System.Linq;
using System.Runtime;
using System.Threading;

namespace BigFishLittleFish
{
	class Program
	{
		static void Main(string[] args)
		{
			var ocean = new Ocean();
			var random = new Random();


			var fishes = Enumerable.Range(0, 20)
				.Select(_ => new Point(random.Next(80), random.Next(25)))
				.Select(pt => random.Next(2) == 0
					? (Fish) new BigFish(pt)
					: new SmallFish(pt)).ToArray();

			foreach (var fish in fishes)
			{
				ocean.AddFish(fish);
			}

			while (true)
			{
				Console.Clear();
				ocean.Render(f =>
				{
					Console.SetCursorPosition(f.Location.X, f.Location.Y);
					Console.Write(f.ToString());
				});

				foreach (var fish in fishes)
				{
					fish.Move(ocean);
				}

				Thread.Sleep(TimeSpan.FromSeconds(1));
			}
		}
	}
}
