using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TicTacToeKata
{
	[TestFixture]
	public class TicTacToeTests
	{
		[Test]
		public void When_all_fields_are_taken_game_is_over()
		{
			var game = new TicTacToeGame();
			var placements = from x in Enumerable.Range(0, 3)
				from y in Enumerable.Range(0, 3)
				select new {x, y};

			var p = Player.Tic;
			foreach (var placement in placements)
			{
				game.Place(p, placement.x, placement.y);
				p = (p == Player.Tic) ? Player.Tac : Player.Tic;
			}

			game.IsComplete.Should().BeTrue();
		}

		[Test]
		public void A_single_placement_does_not_complete_the_game()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tic, 0, 0);

			game.IsComplete.Should().BeFalse();
		}

		[Test]
		public void Alternate_players_placing_a_row_does_not_complete_the_game()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tic, 0, 0);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tac, 1, 0);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tic, 2, 0);
			game.IsComplete.Should().BeFalse();
		}

		[Test]
		public void When_a_player_places_a_row_the_game_is_complete()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tic, 0, 0);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tac, 0, 1);
			game.Place(Player.Tic, 1, 0);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tac, 0, 2);
			game.Place(Player.Tic, 2, 0);
			game.IsComplete.Should().BeTrue();
		}

		[Test]
		public void When_a_player_places_a_column_the_game_is_complete()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tic, 0, 0);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tac, 1, 0);
			game.Place(Player.Tic, 0, 1);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tac, 2, 0);
			game.Place(Player.Tic, 0, 2);
			game.IsComplete.Should().BeTrue();
		}

		[Test]
		public void Placing_to_the_right_of_board_is_stupid()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tic, 3, 0);
		}


		[Test]
		public void Placing_in_diagonal_completes_the_game()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tic, 0, 0);
			game.Place(Player.Tac, 0, 1);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tic, 1, 1);
			game.Place(Player.Tac, 0, 2);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tic, 2, 2);
			game.IsComplete.Should().BeTrue();
		}

		[Test]
		public void Placing_in_other_diagonal_completes_the_game()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tic, 2, 0);
			game.Place(Player.Tac, 0, 1);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tic, 1, 1);
			game.Place(Player.Tac, 2, 1);
			game.IsComplete.Should().BeFalse();
			game.Place(Player.Tic, 0, 2);
			game.IsComplete.Should().BeTrue();
		}

		[Test]
		[ExpectedException(typeof(Exception))]
		public void Field_is_taken_when_tic_has_been_placed()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tac, 0, 0);
			game.Place(Player.Tic, 0, 0);
		}

		[Test]
		[ExpectedException(typeof (Exception))]
		public void Players_must_alternate_turns()
		{
			var game = new TicTacToeGame();
			game.Place(Player.Tac, 0, 0);
			game.Place(Player.Tac, 0, 1);
		}

	}

	public enum Player
	{
		Tic,
		Tac
	}

	public class TicTacToeGame
	{
	    private const int BoardDim = 3;

		private readonly Player?[,] _board = new Player?[BoardDim, BoardDim];

		private Player? _lastMoveBy;

		public void Place(Player player, int x, int y)
		{
			if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
			{
				return;
			}

			if (_lastMoveBy.HasValue && _lastMoveBy.Value == player)
			{
				throw new Exception("Not your turn");
			}

			if (_board[x, y] != null)
			{
				throw new Exception("spot is already taken.");
			}

			_board[x, y] = player;
			_lastMoveBy = player;
		}

	    private static IEnumerable<T> CreateSequence<T>(Func<int, T> select)
	    {
	        return Enumerable.Range(0, BoardDim).Select(select);
	    }

	    private IEnumerable<Player?> CreateDiagonal()
	    {
	        return CreateSequence(i => _board[i, i]);
	    }

	    private IEnumerable<Player?> CreateCrossDiagonal()
	    {
            return CreateSequence(i => _board[i, BoardDim - i - 1]);
	    }

	    private IEnumerable<Player?> CreateRow(int r)
	    {
            return CreateSequence(c => _board[r, c]);
	    }

        private IEnumerable<Player?> CreateColumn(int c)
        {
            return CreateSequence(r => _board[r, c]);
        }

	    private IEnumerable<IEnumerable<Player?>> CreateRows()
	    {
	        return CreateSequence(CreateRow);
	    }

        private IEnumerable<IEnumerable<Player?>> CreateColumns()
        {
            return CreateSequence(CreateColumn);
        }

	    private IEnumerable<IEnumerable<Player?>> CreateSequences()
	    {
	        return CreateRows().Concat(CreateColumns()).Concat(CreateDiagonals());
	    }

	    private IEnumerable<IEnumerable<Player?>> CreateDiagonals()
	    {
	        yield return CreateDiagonal();
	        yield return CreateCrossDiagonal();
	    }

	    private IEnumerable<Player?> GetBoardSequence()
	    {
	        for (int x = 0; x < _board.GetLength(0); x++)
	        {
	            for (int y = 0; y < _board.GetLength(1); y++)
	            {
	                yield return _board[x, y];
	            }
	        }
	    }

	    private bool IsBoardFull
	    {
	        get
	        {
	            return GetBoardSequence().All(it => it.HasValue);
	        }
	    }

	    public bool IsComplete
	    {
	        get
	        {
	            return IsBoardFull || CreateSequences().Any(it => IsCompleteSequence(it.ToList()));
	        }
	    }

	    private static bool IsCompleteSequence(IList<Player?> seq)
	    {
	        var fst = seq.First();
	        return fst.HasValue && seq.All(it => it.HasValue && fst.Value == it.Value);
	    }
	}
}
