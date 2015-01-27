using System;
using System.Linq;

namespace BigFishLittleFish
{
	public abstract class Fish
	{
		public Point Location { get; private set; }

		protected Fish(Point location)
		{
			Location = location;
		}

		private static readonly Random Random = new Random();
		public void Move(Ocean ocean)
		{
			var potentialMoves = (from x in Enumerable.Range(-1, 3)
				from y in Enumerable.Range(-1, 3)
				let m = new Point(x, y)
				where ocean.InBounds(Location.X + m.X, Location.Y + m.Y)
				where CanMove(ocean, m)
				select m).ToArray();

			var move = potentialMoves[Random.Next(potentialMoves.Length)];

			var newLocation = new Point(Location.X + move.X, Location.Y + move.Y);

			MoveTo(ocean, newLocation);
			Location = newLocation;
		}

		protected abstract void MoveTo(Ocean ocean, Point newLocation);


		protected abstract bool CanMove(Ocean aquarium, Point move);
	}

	public class SmallFish : Fish
	{
		public SmallFish(Point location) : base(location)
		{
		}

		protected override bool CanMove(Ocean aquarium, Point move)
		{
			var fishAtLocation = aquarium.GetFishAt(Location.X + move.X, Location.X + move.Y);
			return fishAtLocation == null || fishAtLocation == this;
		}



		protected override void MoveTo(Ocean ocean, Point location)
		{
			ocean.MoveTo(this, location);
		}


	}

	public class BigFish : Fish
	{
		public BigFish(Point location) : base(location)
		{
		}

		protected override bool CanMove(Ocean aquarium, Point move)
		{
			var fishAtLocation = aquarium.GetFishAt(Location.X + move.X, Location.X + move.Y);

			return fishAtLocation == this || fishAtLocation == null ||
			       fishAtLocation is SmallFish;
		}

		protected override void MoveTo(Ocean ocean, Point location)
		{
			var fishAtLocation = ocean.GetFishAt(location.X, location.Y);
			if (fishAtLocation != null)
			{
				ocean.Remove(fishAtLocation);
			}

			ocean.MoveTo(this, location);
		}

	}
}
