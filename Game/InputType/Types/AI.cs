using gomoku.Game.Player;
using gomoku.Game.Positioning;
using gomoku.GUI;
using gomoku.Misc.GamePool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace gomoku.Game.InputType.Types
{
    public class AI : BaseInputType
    {
        public override GameLoc GetNextMove(State state, BasePlayer player)
        {
            BasePlayer[] Enemies = GetEnemies(state, player);

            // Získáme Winning Move 
            GameLoc WinningMove = GetWinningMove(state, player);
            if (WinningMove != null)
            {
                state.Board.WinningMove.Add(WinningMove.ToString());
                // player.Write("Winning move found!");
                return WinningMove;
            }

            // Jestli má enemy příští tah Winning Move, tak jej zablokujeme!
            foreach (BasePlayer enemy in Enemies)
            {
                GameLoc EnemyWinningMove = GetWinningMove(state, enemy);
                if (EnemyWinningMove != null)
                {
                    state.Board.BlockedWinningMove.Add(EnemyWinningMove.ToString());
                    // player.Write("Blocked enemy winning move of  " + enemy.Label() + "!");
                    return EnemyWinningMove;
                }
            }

            //Console.WriteLine("EnemyWinningMove: " + s.ElapsedMilliseconds);

            // Lze-li udelat 4 in-a-row tak aby z obou stran strany nebyl soused!
            GameLoc FourInARow = GetXInARowMove(4, state, player);
            if (FourInARow != null)
            {
                //player.Write("FourInARow Found! move found!");
                state.Board.FourRow.Add(FourInARow.ToString());
                return FourInARow;
            }

            // Blokovat 4 in-a-row:
            foreach (BasePlayer enemy in Enemies)
            {
                GameLoc EnemyFourInARow = GetXInARowMove(4, state, enemy);
                if (EnemyFourInARow != null)
                {
                    //player.Write("FourInARow Move Blocked!");
                    state.Board.BlockedFourRow.Add(EnemyFourInARow.ToString());
                    return EnemyFourInARow;
                }
            }

            // Lze-li udelat 3 in-a-row tak aby z obou stran strany nebyl soused
            GameLoc ThreeInARow = GetXInARowMove(3, state, player);
            if (ThreeInARow != null)
            {
                //player.Write("ThreeInARow Found! move found!");
                state.Board.ThreeRow.Add(ThreeInARow.ToString());
                return ThreeInARow;
            }

            foreach (BasePlayer enemy in Enemies)
            {
                GameLoc EnemyThreeInARow = GetXInARowMove(4, state, enemy);
                if (EnemyThreeInARow != null)
                {
                    //player.Write("ThreeInARow Move Blocked!");
                    state.Board.BlockedThreeRow.Add(EnemyThreeInARow.ToString());
                    return EnemyThreeInARow;
                }
            }

            GameLoc FourInARowClosed = GetXInARowMoveClosed(4, state, player);
            if (FourInARowClosed != null)
            {
                //player.Write("FourInARowClosed Move!");
                return FourInARowClosed;
            }

            GameLoc ThreeInARowClosed = GetXInARowMoveClosed(3, state, player);
            if (ThreeInARowClosed != null)
            {
                //player.Write("ThreeInARowClosed Move!");
                return ThreeInARowClosed;
            }


            // Lze-li udelat 2 in-a-row tak aby z obou stran strany nebyl soused
            // (tohle neblokujeme nebo by jsme se zblaznili)
            GameLoc TwoInARow = GetXInARowMove(2, state, player);
            if (TwoInARow != null)
            {
                state.Board.TwoRow.Add(TwoInARow.ToString());
                //player.Write("TwoInARow Found! move found!");
                return TwoInARow;
            }



            //Console.WriteLine("TotalMoveSpeed: " + total.ElapsedMilliseconds);
            //Console.WriteLine("---------------------------------------------------------");


            return this.MoveRandom(state);
        }



        private GameLoc GetXInARowMoveClosed(int amount, State state, BasePlayer player)
        {

            #region Můžeme-li udělat 'amount' in a row tak aby nakonec bylo možné udělat 5 
            for (int x = 1; x <= state.Field.SizeX; x++)
            {
                for (int y = 1; y <= state.Field.SizeY; y++)
                {

                    

                    GameLoc checkLoc = GameLoc.Create(x, y);
                    GameLocMeta meta = checkLoc.Meta(state.Field);

                    foreach (KeyValuePair<LineType, GameLoc[]> linekv in meta.GetLines())
                    {
                        if (linekv.Value.Length == amount - 1)
                        {
                            //If all x are mine: 
                            bool fullMine = true;
                            foreach (GameLoc loc in linekv.Value)
                            {
                                if (!BasePlayer.IsSame(player, state.Field.Occupant(loc)))
                                {
                                    fullMine = false;
                                }
                            }

                            if (!fullMine)
                            {
                                continue;
                            }

                            GameLoc[] Expanded = GameLocMeta.RemoveInvalid(GameLocMeta.ExpandLine(linekv.Key, linekv.Value, 2, 2), state.Field);

                            if (Expanded.Length >= 5)
                            {
                                //DumpLine("CLOSED Original (len " + (amount - 1) + ")", linekv.Value, state.Field);
                                //DumpLine("CLOSED Expanded ", Expanded, state.Field);

                                bool minusTwoMineOrEmpty = !state.Field.IsOccuppied(Expanded[0]) || BasePlayer.IsSame(state.Field.Occupant(Expanded[0]), player);
                                bool minusOneMineOrEmpty = !state.Field.IsOccuppied(Expanded[1]) || BasePlayer.IsSame(state.Field.Occupant(Expanded[1]), player);

                                bool minusTwoEmpty = !state.Field.IsOccuppied(Expanded[0]);
                                bool minusOneEmpty = !state.Field.IsOccuppied(Expanded[1]);

                                bool plusOneMineOrEmpty = !state.Field.IsOccuppied(Expanded[Expanded.Length - 2]) || BasePlayer.IsSame(state.Field.Occupant(Expanded[Expanded.Length - 2]), player);
                                bool plusTwoMineOrEmpty = !state.Field.IsOccuppied(Expanded[Expanded.Length - 1]) || BasePlayer.IsSame(state.Field.Occupant(Expanded[Expanded.Length - 1]), player);

                                bool plusOneEmpty = !state.Field.IsOccuppied(Expanded[Expanded.Length - 2]);
                                bool plusTwoEmpty = !state.Field.IsOccuppied(Expanded[Expanded.Length - 1]);

                                int possibleLength = linekv.Value.Length;

                                if (minusOneMineOrEmpty) { possibleLength++; }
                                if (minusOneMineOrEmpty && minusTwoMineOrEmpty) { possibleLength++; }

                                if (plusOneMineOrEmpty) { possibleLength++; }
                                if (plusOneMineOrEmpty && plusTwoMineOrEmpty) { possibleLength++; }

                                if (possibleLength >= 5) {

                                    if (minusOneEmpty && minusTwoEmpty)
                                    {
                                        return Expanded[1];
                                    }

                                    if (plusOneEmpty && plusTwoEmpty)
                                    {
                                        return Expanded[Expanded.Length - 2];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion



            return null;
        }



        private GameLoc GetXInARowMove(int amount, State state, BasePlayer player)
        {

            #region Můžeme-li udělat 'amount' in a row tak aby nakonec bylo možné udělat 'amount + 1' 
            for (int x = 1; x <= state.Field.SizeX; x++)
            {
                for (int y = 1; y <= state.Field.SizeY; y++)
                {
                    GameLoc checkLoc = GameLoc.Create(x, y);
                    GameLocMeta meta = checkLoc.Meta(state.Field);

                    foreach (KeyValuePair<LineType, GameLoc[]> linekv in meta.GetLines())
                    {

                        if (linekv.Value.Length == amount - 1)
                        {

                            //If all x are mine: 
                            bool fullMine = true;
                            foreach (GameLoc loc in linekv.Value)
                            {
                                if (!BasePlayer.IsSame(player, state.Field.Occupant(loc)))
                                {
                                    fullMine = false;
                                }
                            }

                            if (!fullMine)
                            {
                                continue;
                            }

                            int requiredExpanded = amount - 1 + 6;


                            //DumpLine("Original (len " + (amount - 1) + ")", linekv.Value, state.Field);
                            GameLoc[] Expanded = GameLocMeta.RemoveInvalid(GameLocMeta.ExpandLine(linekv.Key, linekv.Value, 3, 3), state.Field);
                            //DumpLine("Expanded (possible " + requiredExpanded + ")", Expanded, state.Field);

                            if (Expanded.Length == requiredExpanded)
                            {
                                GameLoc MinusThree = Expanded[0];
                                GameLoc MinusTwo = Expanded[1];
                                GameLoc MinusOne = Expanded[2];

                                GameLoc PlusOne = Expanded[Expanded.Length - 3];
                                GameLoc PlusTwo = Expanded[Expanded.Length - 2];
                                GameLoc PlusThree = Expanded[Expanded.Length - 1];

                                bool MinusTwoOccupied = state.Field.IsOccuppied(MinusTwo);
                                bool MinusOneOccupied = state.Field.IsOccuppied(MinusOne);
                                bool MinusThreeOccupied = state.Field.IsOccuppied(MinusThree);

                                bool MinusTwoMine = MinusTwoOccupied && BasePlayer.IsSame(player, state.Field.Occupant(MinusTwo));
                                bool MinusOneMine = MinusOneOccupied && BasePlayer.IsSame(player, state.Field.Occupant(MinusOne));
                                bool MinusThreeMine = MinusThreeOccupied && BasePlayer.IsSame(player, state.Field.Occupant(MinusThree));

                                bool PlusOneOccupied = state.Field.IsOccuppied(PlusOne);
                                bool PlusTwoOccupied = state.Field.IsOccuppied(PlusTwo);
                                bool PlusThreeOccupied = state.Field.IsOccuppied(PlusThree);

                                bool PlusOneMine = PlusOneOccupied && BasePlayer.IsSame(player, state.Field.Occupant(PlusOne));
                                bool PlusTwoMine = PlusTwoOccupied && BasePlayer.IsSame(player, state.Field.Occupant(PlusTwo));
                                bool PlusThreeMine = PlusThreeOccupied && BasePlayer.IsSame(player, state.Field.Occupant(PlusThree));


                                // Winning Moves:
                                if (!MinusTwoOccupied && !MinusOneOccupied && !PlusOneOccupied)
                                {
                                    return MinusOne;
                                }

                                if (!MinusOneOccupied && !PlusOneOccupied && !PlusTwoOccupied)
                                {
                                    return PlusOne;
                                }

                                // Gaps
                                if (!PlusOneOccupied && PlusTwoMine && (!PlusThreeOccupied || PlusThreeMine))
                                {
                                    return PlusOne;
                                }

                                if (!MinusOneOccupied && MinusTwoMine && (!MinusThreeOccupied || MinusThreeMine))
                                {
                                    return MinusOne;
                                }

                            }
                        }
                    }
                }

            }
            #endregion



            return null;
        }


        private static void DumpLine(string title, GameLoc[] line, Field f)
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("-- " + title);
            foreach (GameLoc loc in line)
            {
                Console.WriteLine("--> " + loc.ToString() + " => " + f.Occupant(loc)?.Label() ?? "null");
            }
            Console.WriteLine("---------------------");
        }

        private BasePlayer[] GetEnemies(State state, BasePlayer player)
        {
            List<BasePlayer> players = new List<BasePlayer>();

            foreach (BasePlayer possibleEnemy in state.Players)
            {
                // Nejsem to ja? je to nepritel!
                if (!BasePlayer.IsSame(player, possibleEnemy))
                {
                    players.Add(possibleEnemy);
                }
            }

            return players.ToArray();
        }

        private GameLoc GetWinningMove(State state, BasePlayer player)
        {
            // Projdu všechny NULL occupanty a pokud mají za souseda mě, vytvořím
            // virtuální pole, hodim tam ten tah a zeptám se jestli je pole resolved...

            for (int x = 1; x <= state.Field.SizeX; x++)
            {
                for (int y = 1; y <= state.Field.SizeY; y++)
                {

                    GameLoc checkLoc = GameLoc.Create(x, y);

                    BasePlayer occupant = state.Field.Occupant(checkLoc);
                    if (occupant == null && HasNeighbor(checkLoc, state))
                    {

                        Field virtualField = state.Field.Clone();
                        virtualField.AddMove(checkLoc, player);
                        if (virtualField.IsResolved())
                        {
                            return checkLoc;
                        }


                    }

                }
            }

            return null;
        }

        private bool HasNeighbor(GameLoc loc, State state, int radius = 1)
        {
            //Console.WriteLine("Checking for Neighbor of "+ loc.ToString()+ "!");

            for (int x = loc.X - radius; x <= loc.X + radius; x++)
            {
                for (int y = loc.Y - radius; y <= loc.Y + radius; y++)
                {
                    GameLoc checkLoc = GameLoc.Create(x,y);

                    // Skip invalid
                    if (!state.Field.IsValid(loc)) {
                        continue;
                    }

                    // Skip Center
                    if (x == loc.X && y == loc.Y)
                    {
                        continue;
                    }

                    if (state.Field.Occupant(GameLoc.Create(x, y)) != null)
                    {
                        return true;
                    }

                }
            }


                    /*for (int x = 1; x <= state.Field.SizeX; x++)
                    {
                        for (int y = 1; y <= state.Field.SizeY; y++)
                        {


                            // Střed vynecháme!
                            if (x == loc.X && y == loc.Y)
                            {
                                continue;
                            }

                            if (state.Field.Occupant(GameLoc.Create(x, y)) != null)
                            {
                                return true;
                            }

                        }
                    }*/

                    return false;
        }
    }
}
