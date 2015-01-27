using System;

namespace BigFishLittleFish
{
	public class Ocean
	{
		private readonly Fish[,] _fishes = new Fish[80, 25];

		public Fish GetFishAt(int x, int y)
		{
			return _fishes[x, y];
		}

		public void MoveTo(Fish fish, Point newLocation)
		{
			Remove(fish);
			_fishes[newLocation.X, newLocation.Y] = fish;
		}

		public void Remove(Fish fish)
		{
			_fishes[fish.Location.X, fish.Location.Y] = null;
		}

		public void Render(Action<Fish> renderFunc)
		{
			for (int x = 0; x < 80; x++)
			{
				for (int y = 0; y < 25; y++)
				{
					var fish = _fishes[x, y];

					if (fish != null)
					{
						renderFunc(fish);
					}
				}
			}
		}

		public void AddFish(Fish fish)
		{
			_fishes[fish.Location.X, fish.Location.Y] = fish;
		}

		public bool InBounds(int x, int y)
		{
			return x >= 0 && x < 80 && y >= 0 && y <= 25;
		}
	}
}
