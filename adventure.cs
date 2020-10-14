using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/
namespace asciiadventure
{
    public class Game
    {
        private Random random = new Random();
        private int turnCount = 0;
        private int tempCount = 0;
        private static int treasureCount = 0;
        private static List<trap> hiddenTraps = new List<trap>();
        private static int score = 0;
        private static Boolean Eq(char c1, char c2)
        {
            return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string Menu()
        {
            return "Score: " + score + "\nWASD to move"+ hiddenTraps.Count+"\nIJKL to attack/interact\nHidden Traps will appear marked with an ^ and change every 10 turns\nMobs will also spawn frequently (#)\nTeleportaion available, use if trapped(you might die though so be careful)\nCollect 15 Treasure to Win, Current Treasures:" + treasureCount + "\nEnter command: ";
        }

        private static void PrintScreen(Screen screen, string message, string menu)
        {
            Console.Clear();
            Console.WriteLine(screen);
            Console.WriteLine($"\n{message}");
            Console.WriteLine($"\n{menu}");
        }
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Screen screen = new Screen(10, 10);
            // add a couple of walls
            for (int i = 0; i < 3; i++)
            {
                new Wall(1, 2 + i, screen);
            }
            for (int i = 0; i < 4; i++)
            {
                new Wall(3 + i, 4, screen);
            }
            List<trap> traps = new List<trap>();


            // add a player
            Player player = new Player(0, 0, screen, "Zelda");

            // add a treasure
            List<Treasure> treasures = new List<Treasure>();
            treasures.Add(new Treasure(3,3,screen));

            // add some mobs
            List<Mob> mobs = new List<Mob>();
            mobs.Add(new Mob(9, 9, screen));

            // initially print the game board
            PrintScreen(screen, "Welcome!", Menu());

            Boolean gameOver = false;

            while (!gameOver)
            {
                char input = Console.ReadKey(true).KeyChar;

                String message = "";

                if (Eq(input, 'q'))
                {
                    break;
                }
                else if (Eq(input, 'w'))
                {
                    player.Move(-1, 0);
                    //Console.WriteLine(mobs.Count);
                }
                else if (Eq(input, 's'))
                {
                    player.Move(1, 0);
                }
                else if (Eq(input, 'a'))
                {
                    player.Move(0, -1);
                }
                else if (Eq(input, 'd'))
                {
                    player.Move(0, 1);
                }
                else if (Eq(input, 'i')) //ijkl each hold the sword mechanic ability.
                {
                    if (player.Row != 0)
                    {
                        GameObject other = screen[player.Row - 1, player.Col];
                        bool attack = player.Attack(screen[player.Row - 1, player.Col]);
                        if (other is Mob && attack == true)
                        {
                            message = "Mob Slain" + "\n";
                            for (int i = 0; i < mobs.Count; i++)
                            {
                                {
                                    if (mobs[i].Row == player.Row - 1 && mobs[i].Col == player.Col)
                                    {
                                        mobs.Remove(mobs[i]);
                                    }
                                }
                            }
                            screen[player.Row - 1, player.Col].Delete();
                            score += 50;
                        }
                        else if (other is Mob && attack == false)
                        {
                            message = "You Missed!" + "\n";
                        }
                        else
                        {
                            message += player.Action(-1, 0) + "\n";
                        }
                    }
                    

                }
                else if (Eq(input, 'k'))
                {
                    if (player.Row != 9)
                    {
                        GameObject other = screen[player.Row + 1, player.Col];
                        bool attack = player.Attack(screen[player.Row + 1, player.Col]);
                        if (other is Mob && attack == true)
                        {
                            message = "Mob Slain" + "\n";
                            for (int i = 0; i < mobs.Count; i++)
                            {
                                {
                                    if (mobs[i].Row == player.Row + 1 && mobs[i].Col == player.Col)
                                    {
                                        mobs.Remove(mobs[i]);
                                    }
                                }

                            }
                            screen[player.Row + 1, player.Col].Delete();
                            score += 50;
                        }
                        else if (other is Mob && attack == false)
                        {
                            message = "You Missed!" + "\n";
                        }
                        else
                        {
                            message += player.Action(1, 0) + "\n";
                        }
                    }
                    
                }
                else if (Eq(input, 'j'))
                {
                    if (player.Col != 0)
                    {
                        GameObject other = screen[player.Row, player.Col - 1];
                        bool attack = player.Attack(screen[player.Row, player.Col - 1]);
                        if (other is Mob && attack == true)
                        {
                            message = "Mob Slain" + "\n";
                            for (int i = 0; i < mobs.Count; i++)
                            {
                                {
                                    if (mobs[i].Row == player.Row && mobs[i].Col == player.Col - 1)
                                    {
                                        mobs.Remove(mobs[i]);
                                    }
                                }

                            }
                            screen[player.Row, player.Col - 1].Delete();
                            score += 50;
                        }
                        else if (other is Mob && attack == false)
                        {
                            message = "You Missed!" + "\n";
                        }
                        else
                        {
                            message += player.Action(0, -1) + "\n";
                        }
                    }
                    
                }
                else if (Eq(input, 'l'))
                {
                    if (player.Col != 9)
                    {
                        GameObject other = screen[player.Row, player.Col + 1];
                        bool attack = player.Attack(screen[player.Row, player.Col + 1]);
                        if (other is Mob && attack == true)
                        {
                            message = "Mob Slain" + "\n";
                            for (int i = 0; i < mobs.Count; i++)
                            {
                                {
                                    if (mobs[i].Row == player.Row && mobs[i].Col == player.Col + 1)
                                    {
                                        mobs.Remove(mobs[i]);
                                    }
                                }
                            }
                            screen[player.Row, player.Col + 1].Delete();
                            score += 50;
                        }
                        else if (other is Mob && attack == false)
                        {
                            message = "You Missed!" + "\n";
                        }
                        else
                        {
                            message += player.Action(0, 1) + "\n";
                        }
                    }
                   
                }
                else if (Eq(input, 'v'))
                {
                    // TODO: handle inventory
                    message = "You have nothing\n";
                }
                else if (Eq(input, 't'))//teleportation move
                {
                        int tele = random.Next(screen.GetEmptySpaces().Count);
                        player.Move(player.Row - (player.Row * 2), player.Col - (player.Col * 2));
                        player.Move(screen.GetEmptySpaces()[tele].Item1, screen.GetEmptySpaces()[tele].Item2);
                        message = "You Teleported";
                }
                else
                {
                    message = $"Unknown command: {input}";
                }
                if(message == "Yay, we got the treasure!" + "\n")
                {
                    treasureCount++;
                }
                // OK, now move the mobs
                foreach (Mob mob in mobs)
                {
                    // TODO: Make mobs smarter, so they jump on the player, if it's possible to do so
                    List<Tuple<int, int>> moves = screen.GetLegalMoves(mob.Row, mob.Col);
                    if (moves.Count == 0)
                    {
                        continue;
                    }
                    // mobs move randomly
                    var (deltaRow, deltaCol) = moves[random.Next(moves.Count)];

                    if (screen[mob.Row + deltaRow, mob.Col + deltaCol] is Player)
                    {
                        // the mob got the player!
                        mob.Token = "*";
                        message += "A MOB GOT YOU! GAME OVER\n";
                        gameOver = true;
                    }
                    mob.Move(deltaRow, deltaCol);
                }
                for (int t = 0; t < hiddenTraps.Count; t++)
                {
                    Console.WriteLine("");
                    if(screen[hiddenTraps[t].Row, hiddenTraps[t].Col] is Player)
                    {
                        player.Token = "*";
                        message += "You were Trapped!\n";
                        gameOver = true;
                    }
                }
                for (int t = 0; t < treasures.Count; t++)
                {
                    GameObject other = screen[treasures[t].Row, treasures[t].Col];
                    if (other is null && message != "Yay, we got the treasure!" + "\n")
                    {
                        screen[treasures[t].Row, treasures[t].Col] = treasures[t];
                    }
                }
                for (int t = 0; t < treasures.Count; t++)
                {
                    GameObject other = screen[treasures[t].Row, treasures[t].Col];
                    if (other is null)
                    {
                        treasures.Remove(treasures[t]);
                    }
                }
                if(turnCount % 6 == 0 && turnCount != 0 && treasures.Count <= 2) { //treasure spawner
                    int spawnTreasure = random.Next(screen.GetEmptySpaces().Count);
                    treasures.Add(new Treasure(screen.GetEmptySpaces()[spawnTreasure].Item1, screen.GetEmptySpaces()[spawnTreasure].Item2, screen));
                }

                if (turnCount % 5 == 0 && turnCount != 0 && mobs.Count < 10)// mob spawner
                {
                    for(int i = 0; i < 2; i++)
                    {
                        int spawn = random.Next(screen.GetEmptySpaces().Count);
                        mobs.Add(new Mob(screen.GetEmptySpaces()[spawn].Item1, screen.GetEmptySpaces()[spawn].Item2, screen));

                    }
                }
                if (turnCount % 10 == 0 && turnCount != 0)//spawn traps
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int spawnTrap = random.Next(screen.GetEmptySpaces().Count);
                        traps.Add(new trap(screen.GetEmptySpaces()[spawnTrap].Item1, screen.GetEmptySpaces()[spawnTrap].Item2, screen));
                    }
                    tempCount = turnCount;
                }
                if (turnCount == tempCount+1) //trap mechanic handler, makes them hidden
                {
                    hiddenTraps.Clear();
                    for (int i = 0; i < traps.Count; i++)
                    {
                        screen[traps[i].Row, traps[i].Col].Delete();
                        hiddenTraps.Add(traps[i]);
                        traps.Remove(traps[i]);
                        i--;
                    }
                }
                if (treasureCount == 15)
                {
                    message = "   :::   :::  ::::::::  :::    :::        :::       ::: ::::::::::: ::::    ::: :::" + "\n" + "  :+:   :+: :+:    :+: :+:    :+:        :+:       :+:     :+:     :+:+:   :+: :+:" + "\n" + "+:+  +:+ +:+    +:+ +:+     +:+         +:+ +:+ +:+     :+:     +:+   +:+ +:+" + "\n" + "+#++:   +#+    +:+ +#+    +:+        +#+  +:+  +#+     +#+     +#+ +:+ +#+ +#+ " + "\n" + "+#+    +#+    +#+ +#+    +#+        +#+ +#+#+ +#+     +#+     +#+  +#+#+# +#+  " + "\n" + "#+#    #+#    #+# #+#    #+#         #+#+# #+#+#      #+#     #+#   #+#+#   " + "\n" + "###     ########   ########           ###   ###   ########### ###    #### ###  ";
                    PrintScreen(screen, message, Menu());
                    gameOver = true;
                }
                turnCount++;
                //if (deferrence++ < 4)
                //{
                //    deferrence++;
                //}


                PrintScreen(screen, message, Menu());
            }
        }

        public static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}